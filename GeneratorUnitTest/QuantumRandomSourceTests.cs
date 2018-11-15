using Generator.Distribution.Entropy;
using Generator.RandomGenerator.Continuous;
using Generator.ResourceAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GeneratorUnitTest
{
    public class QuantumRandomSourceTests
    {
        [Fact]
        public void QuantumSourceAccessibleTest()
        {
            int retrieveBytesCount = 10;
            var uniformRandomSource = new WebRandomSource(new QuantumRngAnuEduAccessDetails());
            byte[] bytes = uniformRandomSource.GetBytesSource().Take(retrieveBytesCount).ToArray();

            Assert.Equal(retrieveBytesCount, bytes.Length);
        }

        [Fact]
        public void QuantumSourceDistributionIsUniformTest()
        {
            var uniformRandomSource = new WebRandomSource(new QuantumRngAnuEduAccessDetails());
            byte[] bytes = uniformRandomSource.GetBytesSource().Take(10000).ToArray();

            double entropy = EntropyCalculator.CalculateForData(bytes);

            Assert.Equal(8, entropy, 1);
        }
    }
}
