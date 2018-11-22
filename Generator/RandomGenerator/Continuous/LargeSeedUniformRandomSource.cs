using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.RandomGenerator.Continuous
{
    public class LargeSeedUniformRandomSource : IUniformDistributionGenerator
    {
        private Queue<Random> mQueue = new Queue<Random>();

        public void SetSeed(byte[] seed)
        {
            if (seed.Length % 4 != 0) throw new ArgumentException("The length of the seed must be a multiple of 4.");

            mQueue.Clear();
            for (int i = 0; i < seed.Length; i += 4)
            {
                int currentSeed = BitConverter.ToInt32(seed, i);
                mQueue.Enqueue(new Random(currentSeed));
            }
        }

        public double Next()
        {
            var random = mQueue.Dequeue();

            mQueue.Enqueue(random); // Add to the end of the queue again

            return random.NextDouble();
        }
    }
}
