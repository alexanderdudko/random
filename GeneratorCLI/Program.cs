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
            var randomSource = new DiscreteFiniteSetValueGenerator<string>(new TwoSymbolDistribution(0.5), new SystemUniformRandomSource());
            var sg = new SequenceGenerator<string>(randomSource);
            foreach (var item in sg.Generate(10))
            {
                Console.WriteLine(item);
            }
            var test = sg.Generate(1000000).ToArray();
            double pA = (double)test.Count(x => x == "Alpha") / test.Length;
            Console.WriteLine($"Probability(Alpha) = {pA:N5}");
        }
    }

    class TwoSymbolDistribution : IDiscreteValueDistribution<string>
    {
        private const string Alpha = "Alpha";
        private const string Beta = "Beta";

        public IEnumerable<string> Values => new string[] { Alpha, Beta };

        private double mProbabilityAlpha;

        public TwoSymbolDistribution(double probabilityAlpha)
        {
            mProbabilityAlpha = probabilityAlpha;
        }

        public double ValueProbability(string value)
        {
            switch (value)
            {
                case Alpha:
                    return mProbabilityAlpha;
                case Beta:
                    return 1 - mProbabilityAlpha;
                default:
                    throw new ArgumentOutOfRangeException($"Value \"{value}\" is not supported.");
            }
        }
    }
}
