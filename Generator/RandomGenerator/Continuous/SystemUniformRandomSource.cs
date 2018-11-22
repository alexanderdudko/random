using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.RandomGenerator.Continuous
{
    public class SystemUniformRandomSource : IUniformDistributionGenerator
    {
        private Random mRandom;

        public SystemUniformRandomSource()
        {
            mRandom = new Random();
        }

        public SystemUniformRandomSource(int seed)
        {
            mRandom = new Random(seed);
        }

        public double Next()
        {
            return mRandom.NextDouble();
        }
    }
}
