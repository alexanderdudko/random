using Generator;
using Generator.Distribution;
using Generator.RandomGenerator.Discrete;
using Generator.RandomGenerator.Uniform;
using Generator.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneratorCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var distribution = new ThreeSymbolDistribution(0.5, 1.0 / 3.0);
            var randomSource = new DiscreteFiniteSetValueGenerator<string>(distribution, new SystemUniformRandomSource());
            var sg = new SequenceGenerator<string>(randomSource);
            foreach (var item in sg.Generate(10))
            {
                Console.WriteLine(item);
            }
            var test = sg.Generate(1000000).ToArray();
            Func<string,double> pFunc = v => (double)test.Count(x => x == v) / test.Length;
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
