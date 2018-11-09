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
            return -distribution.Values.Sum(x => distribution.ValueProbability(x) * Math.Log(distribution.ValueProbability(x), 2));
        }
    }
}
