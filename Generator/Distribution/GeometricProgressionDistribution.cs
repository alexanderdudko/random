using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generator.Distribution
{
    public class GeometricProgressionDistribution<T> : IDiscreteValueDistribution<T>
    {
        private T[] mValues;
        private double mProgressionRate = 0;
        private Dictionary<T, double> mProbabilities;
        public IEnumerable<T> Values => mValues;

        public GeometricProgressionDistribution(IEnumerable<T> values, double progressionRate)
        {
            if (progressionRate <= 0) throw new ArgumentOutOfRangeException("Progression rate must be greater than zero.");

            mValues = values.ToArray();
            SetProgressionRate(progressionRate);
        }

        public void SetProgressionRate(double progressionRate)
        {
            if (progressionRate == mProgressionRate) return;

            mProgressionRate = progressionRate;
            mProbabilities = new Dictionary<T, double>();
            double sum = 1;
            double x = 1;
            for (int i = 2; i <= mValues.Length; i++)
            {
                x /= progressionRate;
                sum += x;
            }
            double probability = 1.0 / sum;
            foreach (var item in mValues)
            {
                mProbabilities[item] = probability;
                probability /= progressionRate;
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
