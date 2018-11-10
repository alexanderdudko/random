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
            mProbabilities = CalculateProbabilities(data);
        }

        public IEnumerable<T> Values => mProbabilities.Keys;

        public static Dictionary<T, double> CalculateProbabilities(IEnumerable<T> data)
        {
            var probabilities = new Dictionary<T, double>();
            int count = 0;
            foreach (var item in data)
            {
                if (probabilities.ContainsKey(item))
                    probabilities[item]++;
                else
                    probabilities[item] = 1;
                count++;
            }
            foreach (var key in probabilities.Keys.ToArray())
            {
                probabilities[key] /= count;
            }

            double totalProbability = probabilities.Values.Sum();
            if (totalProbability - 1 > 1e-10) throw new ArithmeticException($"Total probability must be equal to 1, but was {totalProbability}.");

            return probabilities;
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
