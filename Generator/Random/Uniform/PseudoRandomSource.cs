using System;
using System.Collections.Generic;
using System.Text;

namespace Generator
{
    public class PseudoRandomSource : IUniformDistributionGenerator
    {
        private Random mRandom = new Random();

        public double Next()
        {
            return mRandom.NextDouble();
        }
    }
}
