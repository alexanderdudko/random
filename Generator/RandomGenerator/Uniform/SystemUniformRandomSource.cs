using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.RandomGenerator.Uniform
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
