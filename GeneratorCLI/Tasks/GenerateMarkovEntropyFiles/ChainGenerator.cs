using Generator.Distribution;
using Generator.RandomGenerator;
using Generator.RandomGenerator.Continuous;
using Generator.RandomGenerator.Discrete;
using Generator.Sequence;
using GeneratorCLI.Shuffle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GeneratorCLI.Tasks.Generate
{
    internal class ChainGenerator
    {
        private IUniformDistributionGenerator mUniformRandomSource;
        public double TotalGeneratedEntropy { get; private set; } = 0;

        private HashAlgorithm mHash;
        private LargeSeedUniformRandomSource mShuffleRandom;
        private Shuffler mShuffler;

        public ChainGenerator(IUniformDistributionGenerator uniformRandomSource = null)
        {
            if (uniformRandomSource == null)
                mUniformRandomSource = new CryptoUniformRandomSource();
            else
                mUniformRandomSource = uniformRandomSource;

            mShuffleRandom = new LargeSeedUniformRandomSource();
            mShuffler = new Shuffler(mShuffleRandom);
            mHash = SHA512.Create();
        }

        public byte[] GenerateData(double progression, int initSize, int totalSize, int dependanceDepth)
        {
            if (initSize < dependanceDepth) throw new ArgumentException("Dependency depth must be less or equal to init size.");
            if (totalSize < initSize) throw new ArgumentException("Init size must be less or equal to total size.");

            byte[] result = new byte[totalSize];

            TotalGeneratedEntropy = 0;

            // Generating init sequence
            byte[] initSequence = GenerateInitSequence(progression, initSize);
            initSequence.CopyTo(result, 0);

            // Generating markov chain
            GenerateChain(result, progression, initSize, totalSize - initSize, dependanceDepth);

            return result;
        }

        internal byte[] GenerateInitSequence(double progression, int initSize)
        {
            var values = Enumerable.Range(0, 256).Select(x => (byte)x).ToArray();

            var distribution = new GeometricProgressionDistribution<byte>(values, progression);
            var randomSource = new DiscreteFiniteSetValueGenerator<byte>(distribution, mUniformRandomSource);
            var sg = new SequenceGenerator<byte>(randomSource);
            byte[] bytes = sg.Generate(initSize).ToArray();

            TotalGeneratedEntropy += randomSource.TotalGeneratedEntropy;

            return bytes;
        }

        internal void GenerateChain(byte[] data, double progression, int startPosition, int count, int dependanceDepth)
        {
            if (data.Length < startPosition + count) throw new ArgumentOutOfRangeException("Start position + count must not exceed the data length");

            int[] indexValues = Enumerable.Range(0, 256).ToArray();
            var distribution = new GeometricProgressionDistribution<int>(indexValues, progression);
            var randomSource = new DiscreteFiniteSetValueGenerator<int>(distribution, mUniformRandomSource);

            for (int i = 0; i < count; i++)
            {
                byte[] dependanceData = data.AsSpan(startPosition + i - dependanceDepth, dependanceDepth).ToArray();
                data[startPosition + i] = GenerateNextItem(dependanceData, randomSource);
            }

            TotalGeneratedEntropy += randomSource.TotalGeneratedEntropy;
        }

        internal byte GenerateNextItem(byte[] initSequence, IRandomGenerator<int> randomSource)
        {
            int nextIndex = randomSource.Next();
            return MapIndex(nextIndex, initSequence);
        }

        internal byte MapIndex(int nextIndex, byte[] initSequence)
        {
            var hash = mHash.ComputeHash(initSequence);
            mShuffleRandom.SetSeed(hash);
            return (byte)mShuffler.GetPickSequence(256).ElementAt(nextIndex);
        }

    }
}
