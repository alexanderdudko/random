using Generator.RandomGenerator.Continuous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneratorCLI.Shuffle
{
    public class Shuffler
    {
        private IUniformDistributionGenerator mRandomGenerator;

        public Shuffler(IUniformDistributionGenerator randomGenerator)
        {
            mRandomGenerator = randomGenerator;
        }

        public IEnumerable<T> Shuffle<T>(IEnumerable<T> collection)
        {
            var data = collection.ToArray();

            foreach (var index in GetPickSequence(data.Length))
                yield return data[index];
        }

        /// <summary>
        /// Returns a complete sequence of elements between 0 and elementCount - 1 without equal elements
        /// </summary>
        /// <param name="elementCount"></param>
        /// <returns></returns>
        public IEnumerable<int> GetPickSequence(int elementCount)
        {
            List<int> result = new List<int>();
            List<int> set = new List<int>(Enumerable.Range(0, elementCount));

            while (set.Any())
            {
                int index = (int)Math.Floor(mRandomGenerator.Next()* set.Count);
                result.Add(set[index]);
                set.RemoveAt(index);
            }

            return result;
        }
    }
}
