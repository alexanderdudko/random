using Generator.Distribution;
using Generator.RandomGenerator.Continuous;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.RandomGenerator.Discrete
{
    public class DiscreteFiniteSetValueGenerator<T> : IDistributionGenerator<T>
    {
        private IDiscreteValueDistribution<T> mDistribution;
        private IUniformDistributionGenerator mUniformDistributionGenerator;

        public DiscreteFiniteSetValueGenerator(IDiscreteValueDistribution<T> distribution, IUniformDistributionGenerator uniformDistributionGenerator)
        {
            mDistribution = distribution;
            mUniformDistributionGenerator = uniformDistributionGenerator;
        }

        public T Next()
        {
            double p = mUniformDistributionGenerator.Next();
            double cumulativeP = 0;
            foreach (var item in mDistribution.Values)
            {
                double valueP = mDistribution.ValueProbability(item);
                cumulativeP += valueP;
                if (cumulativeP > p)
                    return item;
            }
            throw new ArithmeticException($"Cumulative probability reached value {cumulativeP}, but that not enough to be greater than uniform random value equal to {p}.");
        }
    }
}
