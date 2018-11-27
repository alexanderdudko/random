using Generator.Distribution;
using Generator.RandomGenerator.Continuous;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.RandomGenerator.Discrete
{
    public class DiscreteFiniteSetValueGenerator<T> : IRandomGenerator<T>
    {
        private IDiscreteValueDistribution<T> mDistribution;
        private IUniformDistributionGenerator mUniformDistributionGenerator;

        private double mCumulativeEntropy = 0;
        private int mValuesCount = 0;
        public double TotalGeneratedEntropy => mValuesCount > 0 ? mCumulativeEntropy / mValuesCount : 0;

        public DiscreteFiniteSetValueGenerator(IDiscreteValueDistribution<T> distribution, IUniformDistributionGenerator uniformDistributionGenerator)
        {
            mDistribution = distribution;
            mUniformDistributionGenerator = uniformDistributionGenerator;
        }

        public T Next()
        {
            double x = mUniformDistributionGenerator.Next();
            double cumulativeP = 0;
            foreach (var valueItem in mDistribution.Values)
            {
                double valueItemProbability = mDistribution.ValueProbability(valueItem);
                cumulativeP += valueItemProbability;
                if (cumulativeP > x)
                {
                    mValuesCount++;
                    mCumulativeEntropy += -Math.Log(valueItemProbability, 2);
                    return valueItem;
                }
            }
            throw new ArithmeticException($"Cumulative probability reached value {cumulativeP}, but that not enough to be greater than uniform random value equal to {x}.");
        }

        public void ClearTotalEntropy()
        {
            mCumulativeEntropy = 0;
            mValuesCount = 0;
        }
    }
}
