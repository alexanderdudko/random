using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generator.Distribution.Entropy
{
    public static class EntropyCalculator
    {
        public static double CalculateForDistribution<T>(IDiscreteValueDistribution<T> distribution)
        {
            double sum = 0;
            foreach (var value in distribution.Values)
            {
                double valueP = distribution.ValueProbability(value);
                if (valueP > 0)
                    sum += valueP * Math.Log(valueP, 2);
            }

            return -sum;
        }
        public static double CalculateForData<T>(IEnumerable<T> data)
        {
            return CalculateForDistribution(new EmpiricalDataDistribution<T>(data));
        }
    }
}
