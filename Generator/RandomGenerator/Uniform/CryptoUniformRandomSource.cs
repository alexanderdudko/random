using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Generator.RandomGenerator.Uniform
{
    public class CryptoUniformRandomSource : IUniformDistributionGenerator
    {
        // http://eimagine.com/how-to-generate-better-random-numbers-in-c-net-2/

        private RandomNumberGenerator mRandom = RandomNumberGenerator.Create();

        public double Next()
        {
            byte[] b = new byte[4];
            mRandom.GetBytes(b);
            return (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
        }
    }
}
