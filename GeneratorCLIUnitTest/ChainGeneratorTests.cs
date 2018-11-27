using Generator.Distribution.Entropy;
using Generator.RandomGenerator.Continuous;
using GeneratorCLI.Tasks.Generate;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GeneratorCLIUnitTest
{
    public class ChainGeneratorTests
    {
        [Fact]
        public void SymbolMappingReproducibleTest()
        {
            var cg = new ChainGenerator();
            byte[] initSeq = Encoding.UTF8.GetBytes("Some key 1");

            byte out1 = cg.MapIndex(0, initSeq);
            byte out2 = cg.MapIndex(0, initSeq);

            Assert.Equal(out1, out2);
        }

        [Fact]
        public void SymbolMappingDifferentForDifferentInitSequenceTest()
        {
            var cg = new ChainGenerator();
            byte[] initSeq1 = Encoding.UTF8.GetBytes("Some key 1");
            byte[] initSeq2 = Encoding.UTF8.GetBytes("Some key 2 with different length");
            int N = 10;

            byte[] seq1out = new byte[N];
            byte[] seq2out = new byte[N];

            for (int i = 0; i < N; i++)
            {
                seq1out[i] = cg.MapIndex(i, initSeq1);
                seq2out[i] = cg.MapIndex(i, initSeq2);
            }

            Assert.NotEqual(seq1out, seq2out);
        }

        [Fact]
        public void GenerateChainGivesSameSequenceOnSameInitSequenceTest()
        {
            byte[] initSequence = Encoding.UTF8.GetBytes("Some initial values");
            int N = 100;
            byte[] data1 = new byte[N];
            byte[] data2 = new byte[N];
            int seed = 47;
            var cg1 = new ChainGenerator(new SystemUniformRandomSource(seed));
            var cg2 = new ChainGenerator(new SystemUniformRandomSource(seed));
            int dependenceDepth = initSequence.Length;
            int startPosition = initSequence.Length;
            initSequence.CopyTo(data1, 0);
            initSequence.CopyTo(data2, 0);

            cg1.GenerateChain(data1, 1, startPosition, N - startPosition, dependenceDepth);
            cg2.GenerateChain(data2, 1, startPosition, N - startPosition, dependenceDepth);

            Assert.Equal(data1, data2);
        }

        [Fact]
        public void GenerateInitSequenceGivesExpectedEntropyTest()
        {
            int N = 1000000;
            var chainGenerator = new ChainGenerator();

            byte[] initSequence = chainGenerator.GenerateInitSequence(1.1, N);
            double entropy = EntropyCalculator.CalculateForData(initSequence);
            double generatorEntropy = chainGenerator.TotalGeneratedEntropy;

            Assert.Equal(generatorEntropy, entropy, 3);
        }

        [Fact]
        public void GenerateDataWithZeroDependenceDepthGivesExpectedEntropyTest()
        {
            int N = 100000;
            var chainGenerator = new ChainGenerator();

            byte[] data = chainGenerator.GenerateData(1.1, 0, N, 0);
            double entropy = EntropyCalculator.CalculateForData(data);
            double generatorEntropy = chainGenerator.TotalGeneratedEntropy;

            Assert.Equal(generatorEntropy, entropy, 1);
        }

        [Fact]
        public void GenerateDataWithDependenceGivesLowerEntropyTest()
        {
            int N = 100000;
            double progressionRate = 1.1;
            int dependanceDepth = 10;

            var chainGenerator1 = new ChainGenerator();
            byte[] dataWithoutDependance = chainGenerator1.GenerateData(progressionRate, 0, N, 0);
            double dataWithoutDependanceEntropy = chainGenerator1.TotalGeneratedEntropy;

            var chainGenerator2 = new ChainGenerator();
            byte[] dataWithDependance = chainGenerator2.GenerateData(progressionRate, dependanceDepth, N, dependanceDepth);
            double dataWithDependanceEntropy = chainGenerator2.TotalGeneratedEntropy;

            Assert.InRange(dataWithDependanceEntropy, 0, dataWithoutDependanceEntropy);
        }
    }
}
