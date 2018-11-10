using Generator.Distribution;
using System;
using Xunit;

namespace GeneratorUnitTest
{
    public class EmpiricalDataDistributionTests
    {
        [Fact]
        public void EmpiricalDataDistribution_CalculateProbabilities_ProbabilitiesTest()
        {
            var data = new char[] { 'A', 'A', 'A', 'A', 'B', 'B', 'C', 'D' };
            var probabilities = EmpiricalDataDistribution<char>.CalculateProbabilities(data);

            Assert.Equal(0.5, probabilities['A']);
            Assert.Equal(0.25, probabilities['B']);
            Assert.Equal(0.125, probabilities['C']);
            Assert.Equal(0.125, probabilities['D']);
        }
    }
}
