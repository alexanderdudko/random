using Generator.Distribution;
using Generator.Distribution.Entropy;
using Generator.RandomGenerator.Continuous;
using Generator.RandomGenerator.Discrete;
using Generator.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GeneratorUnitTest
{
    public class DiscreteFiniteSetValueGeneratorTests
    {
        [Fact]
        public void OutputEmpiricalEntropyEqualToTheoreticalEntropyTest()
        {
            int count = 1000000;
            var data = new char[] { 'A', 'A', 'A', 'A', 'B', 'B', 'C', 'D' };
            var distribution = new EmpiricalDataDistribution<char>(data);
            var randomSource = new DiscreteFiniteSetValueGenerator<char>(distribution, new SystemUniformRandomSource());
            var sg = new SequenceGenerator<char>(randomSource);
            var generatedData = sg.Generate(count);

            double theoreticalEntropy = EntropyCalculator.CalculateForDistribution(distribution);
            double empiricalEntropy = EntropyCalculator.CalculateForData(generatedData);

            Assert.Equal(theoreticalEntropy, empiricalEntropy, 2);
        }

        [Fact]
        public void GeneratorCalculatedEntropyEqualToTheoreticalEntropyTest()
        {
            int count = 1000000;
            var data = new char[] { 'A', 'A', 'A', 'A', 'B', 'B', 'C', 'D' };
            var distribution = new EmpiricalDataDistribution<char>(data);
            var randomSource = new DiscreteFiniteSetValueGenerator<char>(distribution, new SystemUniformRandomSource());
            var sg = new SequenceGenerator<char>(randomSource);
            var generatedData = sg.Generate(count).ToArray();

            double theoreticalEntropy = EntropyCalculator.CalculateForDistribution(distribution);
            double generatorEntropy = randomSource.TotalGeneratedEntropy;

            Assert.Equal(theoreticalEntropy, generatorEntropy, 2);
        }

        [Fact]
        public void EmpiricalEntropyForDataWithOneByteDependenceIsLowerForTwoByteStepTest()
        {
            int count = 1000000;
            var data = new char[] { 'A', 'A', 'A', 'A', 'B', 'B', 'C', 'D' };
            var distribution = new EmpiricalDataDistribution<char>(data);
            var randomSource = new DiscreteFiniteSetValueGenerator<char>(distribution, new SystemUniformRandomSource());
            var sg = new SequenceGenerator<char>(randomSource);
            var generatedData = sg.Generate(count).ToArray();

            double empiricalEntropy = EntropyCalculator.CalculateForData(generatedData);

            int[] twoByteData = new int[count / 2];
            for (int i = 0; i < count / 2; i++)
                twoByteData[i] = generatedData[i * 2] * 256 + generatedData[i * 2 + 1];

            double twoByteEntropy = EntropyCalculator.CalculateForData(twoByteData);
            double twoByteEntropyPerByte = twoByteEntropy / 2;

            Assert.Equal(empiricalEntropy, twoByteEntropyPerByte, 3);
        }
    }
}
