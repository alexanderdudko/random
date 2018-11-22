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
            int numberOfValues = 100;

            var sequence1 = Enumerable.Range(0, numberOfValues).Select(x => generator1.Next()).ToArray();
            var sequence2 = Enumerable.Range(0, numberOfValues).Select(x => generator2.Next()).ToArray();

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
            int numberOfValues = 100;

            var sequence1 = Enumerable.Range(0, numberOfValues).Select(x => generator1.Next()).ToArray();
            var sequence2 = Enumerable.Range(0, numberOfValues).Select(x => generator2.Next()).ToArray();

            Assert.Equal(sequence1, sequence2);
        }

        [Fact]
        public void AllBytesOfTheSeedAreTakenIntoAccountTest()
        {
            byte mutation = 47;
            int byteNumber = 0;

            byte[] originalSeed = Guid.NewGuid().ToByteArray();
            byte[] seed1 = originalSeed.ToArray();
            byte[] seed2 = originalSeed.ToArray();
            seed1[0 + byteNumber] = (byte)((seed1[0 + byteNumber] + mutation) % 256);
            seed2[4 + byteNumber] = (byte)((seed2[4 + byteNumber] + mutation) % 256);
            var originalGenerator = new LargeSeedUniformRandomSource();
            var generator1 = new LargeSeedUniformRandomSource();
            var generator2 = new LargeSeedUniformRandomSource();
            originalGenerator.SetSeed(originalSeed);
            generator1.SetSeed(seed1);
            generator2.SetSeed(seed2);
            int numberOfValues = 100;

            var original = Enumerable.Range(0, numberOfValues).Select(x => originalGenerator.Next()).ToArray();
            var sequence1 = Enumerable.Range(0, numberOfValues).Select(x => generator1.Next()).ToArray();
            var sequence2 = Enumerable.Range(0, numberOfValues).Select(x => generator2.Next()).ToArray();

            Assert.NotEqual(originalSeed, seed1);
            Assert.NotEqual(originalSeed, seed2);
            Assert.NotEqual(seed1, seed2);

            Assert.NotEqual(original, sequence1);
            Assert.NotEqual(original, sequence2);
            Assert.NotEqual(sequence1, sequence2);
        }
    }
}
