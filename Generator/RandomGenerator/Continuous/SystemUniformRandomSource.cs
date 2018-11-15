using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.RandomGenerator.Continuous
{
    public class SystemUniformRandomSource : IUniformDistributionGenerator
    {
        private Random mRandom = new Random();

        public double Next()
        {
            return mRandom.NextDouble();
        }
    }
}
