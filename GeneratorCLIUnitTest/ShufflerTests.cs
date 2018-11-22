using Generator.RandomGenerator.Continuous;
using GeneratorCLI.Shuffle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GeneratorCLIUnitTest
{
    public class ShufflerTests
    {
        [Fact]
        public void PickSequenceContainsAllElementsTest()
        {
            var shuffler = new Shuffler(new SystemUniformRandomSource());

            int elementsCount = 256;

            int[] picks = shuffler.GetPickSequence(elementsCount).ToArray();
            var set = new HashSet<int>(picks);
            int minValue = picks.Min();
            int maxValue = picks.Max();

            Assert.Equal(picks.Length, set.Count);
            Assert.Equal(0, minValue);
            Assert.Equal(elementsCount - 1, maxValue);
        }

        [Fact]
        public void SeedRandomizesPickSequenceTest()
        {
            int elementsCount = 1024;

            int seed1 = 47;
            int seed2 = 48;
            var shuffler1 = new Shuffler(new SystemUniformRandomSource(seed1));
            var shuffler2 = new Shuffler(new SystemUniformRandomSource(seed2));

            int[] picks1 = shuffler1.GetPickSequence(elementsCount).ToArray();
            int[] picks2 = shuffler2.GetPickSequence(elementsCount).ToArray();

            Assert.NotEqual(seed1, seed2);
            Assert.NotEqual(picks1, picks2);
        }

        [Fact]
        public void PickSequenceIsReproducibleTest()
        {
            int elementsCount = 1024;

            int seed = 47;
            var shuffler1 = new Shuffler(new SystemUniformRandomSource(seed));
            var shuffler2 = new Shuffler(new SystemUniformRandomSource(seed));

            int[] picks1 = shuffler1.GetPickSequence(elementsCount).ToArray();
            int[] picks2 = shuffler2.GetPickSequence(elementsCount).ToArray();

            Assert.Equal(picks1, picks2);
        }
    }
}
