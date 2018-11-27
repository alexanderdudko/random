using Generator.Distribution.Entropy;
using Generator.RandomGenerator.Continuous;
using Generator.ResourceAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace GeneratorUnitTest
{
    public class QuantumRandomSourceTests
    {
        [Fact(Skip = "Access to web service")]
        public void QuantumSourceAccessibleTest()
        {
            int retrieveBytesCount = 10;
            var uniformRandomSource = new WebRandomSource(new QuantumRngAnuEduAccessDetails());
            byte[] bytes = uniformRandomSource.GetBytesSource().Take(retrieveBytesCount).ToArray();

            Assert.Equal(retrieveBytesCount, bytes.Length);
        }

        [Fact(Skip = "Access to web service")]
        public void QuantumSourceDistributionIsUniformTest()
        {
            var uniformRandomSource = new WebRandomSource(new QuantumRngAnuEduAccessDetails());
            byte[] bytes = uniformRandomSource.GetBytesSource().Take(10000).ToArray();

            double entropy = EntropyCalculator.CalculateForData(bytes);

            Assert.Equal(8, entropy, 1);
        }

        [Fact(Skip = "Access to web service")]
        public void QuantumSourceLargeDataDistributionIsUniformTest()
        {
            var uniformRandomSource = new WebRandomSource(new LargeDataQuantumRngAnuEduAccessDetails());
            byte[] bytes = uniformRandomSource.GetBytesSource().Take(1024 * 1024).ToArray();

            double entropy = EntropyCalculator.CalculateForData(bytes);

            Assert.Equal(8, entropy, 3);
        }

    }
}
