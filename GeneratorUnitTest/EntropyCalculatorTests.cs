using Generator.Distribution.Entropy;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GeneratorUnitTest
{
    public class EntropyCalculatorTests
    {
        [Fact]
        public void CalculateEntropyForEmpiricalDataTest()
        {
            var data = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            double entropy = EntropyCalculator.CalculateForData(data);

            Assert.Equal(3, entropy);
        }
    }
}
