using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generator.Distribution
{
    public class EmpiricalDataDistribution<T> : IDiscreteValueDistribution<T>
    {
        private Dictionary<T, double> mProbabilities;

        public EmpiricalDataDistribution(IEnumerable<T> data)
        {
            CalculateProbabilities(data);
        }

        public IEnumerable<T> Values => mProbabilities.Keys;

        public void CalculateProbabilities(IEnumerable<T> data)
        {
            mProbabilities = new Dictionary<T, double>();
            int count = 0;
            foreach (var item in data)
            {
                mProbabilities[item]++;
                count++;
            }
            foreach (var key in mProbabilities.Keys)
            {
                mProbabilities[key] /= count;
            }
            double totalProbability = mProbabilities.Values.Sum();
            if (totalProbability - 1 > 1e-10) throw new ArithmeticException($"Total probability must be equal to 1, but was {totalProbability}.");
        }

        public double ValueProbability(T value)
        {
            if (mProbabilities.ContainsKey(value))
                return mProbabilities[value];
            else
                return 0;
        }
    }

}
