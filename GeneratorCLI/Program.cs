using Generator;
using Generator.Distribution;
using Generator.Distribution.Entropy;
using Generator.RandomGenerator.Discrete;
using Generator.RandomGenerator.Uniform;
using Generator.Sequence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GeneratorCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string basePath = @"H:\projects\data\random";
            var path = Directory.CreateDirectory(Path.Combine(basePath, DateTime.Now.ToString("yyyyMMdd-HHmmss"))).FullName;
            int fileSize = 1024 * 1024; // 1MB
            int numberOfFiles = 16;

            GenerateFiles(path, fileSize, numberOfFiles);
        }

        private static void GenerateFiles(string path, int fileSize, int numberOfFiles)
        {
            int n = 256;
            var d = new GeometricProgressionDistribution<byte>(Enumerable.Range(0, n).Select(x => (byte)x), 1);
            double k = 1;
            for (int i = 0; i < numberOfFiles; i++)
            {
                double progression = Math.Pow(k, 1.0 / (n - 1));
                d.SetProgressionRate(progression);

                string filename = $"{i}.data";
                Console.WriteLine($"Generating file {filename}.");
                GenerateFile(d, Path.Combine(path, filename), fileSize);

                k *= 2;
            }
        }

        private static void GenerateFile(IDiscreteValueDistribution<byte> distribution, string filename, int size)
        {
            double generatorEntropy = EntropyCalculator.CalculateForDistribution(distribution);
            Console.WriteLine($"  Generator entropy {generatorEntropy:N5}");

            var randomSource = new DiscreteFiniteSetValueGenerator<byte>(distribution, new SystemUniformRandomSource());
            var sg = new SequenceGenerator<byte>(randomSource);
            byte[] bytes;
            double difference;
            do
            {
                bytes = sg.Generate(size).ToArray();

                double empiricalDataEntropy = EntropyCalculator.CalculateForData(bytes);
                difference = Math.Abs(generatorEntropy - empiricalDataEntropy);
                Console.WriteLine($"  Generated empirical data with entropy {empiricalDataEntropy:N5}. Difference: {difference:N5}.");
            } while (difference > 0.001);
            File.WriteAllBytes(filename, bytes);
        }

        private static void Test_EightBitProgressionDistGenerator_Entropy()
        {
            int n = 256;
            var d = new GeometricProgressionDistribution<int>(Enumerable.Range(0, n), 1);
            double k = 1;
            for (int i = 0; i < 200; i++)
            {
                double progression = Math.Pow(k, 1.0 / (n - 1));
                d.SetProgressionRate(progression);
                Console.WriteLine($"{EntropyCalculator.CalculateForDistribution(d)}");
                k *= 2;
            }
        }

        private static void Test_EightSymbolsProgressionDistribution_Entropy()
        {
            var d = new GeometricProgressionDistribution<string>(
                new string[] { "A", "B", "C", "D", "E", "F", "G", "H" },
                2
            );
            foreach (var item in d.Values)
            {
                Console.WriteLine($"Probability({item}) = {d.ValueProbability(item)}");
            }
            Console.WriteLine($"Entropy = {EntropyCalculator.CalculateForDistribution(d)}");
        }

        private static void Test_ThreeSymbolDistribution_OutputProbability()
        {
            var distribution = new ThreeSymbolDistribution(0.5, 1.0 / 3.0);
            var randomSource = new DiscreteFiniteSetValueGenerator<string>(distribution, new SystemUniformRandomSource());
            var sg = new SequenceGenerator<string>(randomSource);
            foreach (var item in sg.Generate(10))
            {
                Console.WriteLine(item);
            }
            var test = sg.Generate(1000000).ToArray();
            Func<string, double> pFunc = v => (double)test.Count(x => x == v) / test.Length;
            foreach (var item in distribution.Values)
            {
                Console.WriteLine($"Probability({item}) = {pFunc(item):N5}");
            }
        }
    }

    class ThreeSymbolDistribution : IDiscreteValueDistribution<string>
    {
        public const string Alpha = "Alpha";
        public const string Beta = "Beta";
        public const string Gamma = "Gamma";

        public IEnumerable<string> Values => new string[] { Alpha, Beta, Gamma };

        private double mProbabilityAlpha;
        private double mProbabilityBeta;

        public ThreeSymbolDistribution(double probabilityAlpha, double probabilityBeta)
        {
            mProbabilityAlpha = probabilityAlpha;
            mProbabilityBeta = probabilityBeta;
        }

        public double ValueProbability(string value)
        {
            switch (value)
            {
                case Alpha:
                    return mProbabilityAlpha;
                case Beta:
                    return mProbabilityBeta;
                case Gamma:
                    return 1 - mProbabilityAlpha - mProbabilityBeta;
                default:
                    throw new ArgumentOutOfRangeException($"Value \"{value}\" is not supported.");
            }
        }
    }
}
