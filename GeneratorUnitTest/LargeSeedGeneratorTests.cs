using Generator.RandomGenerator.Continuous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GeneratorUnitTest
{
    public class LargeSeedGeneratorTests
    {
        [Fact]
        public void DifferentSeedGeneratesDifferentSequenceTest()
        {
            byte[] seed1 = Guid.NewGuid().ToByteArray();
            byte[] seed2 = Guid.NewGuid().ToByteArray();
            var generator1 = new LargeSeedUniformRandomSource();
            var generator2 = new LargeSeedUniformRandomSource();
            generator1.SetSeed(seed1);
            generator2.SetSeed(seed2);

            var sequence1 = Enumerable.Range(0, 100).Select(x => generator1.Next()).ToArray();
            var sequence2 = Enumerable.Range(0, 100).Select(x => generator2.Next()).ToArray();

            Assert.NotEqual(seed1, seed2);
            Assert.NotEqual(sequence1, sequence2);
        }

        [Fact]
        public void SameSeedGeneratesSameSequenceTest()
        {
            byte[] seed = Guid.NewGuid().ToByteArray();
            var generator1 = new LargeSeedUniformRandomSource();
            var generator2 = new LargeSeedUniformRandomSource();
            generator1.SetSeed(seed);
            generator2.SetSeed(seed);

            var sequence1 = Enumerable.Range(0, 100).Select(x => generator1.Next()).ToArray();
            var sequence2 = Enumerable.Range(0, 100).Select(x => generator2.Next()).ToArray();

            Assert.Equal(sequence1, sequence2);
        }
    }
}
