using Generator.RandomGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.Sequence
{
    public class SequenceGenerator<T> : ISequenceGenerator<T>
    {
        private IRandomGenerator<T> mSource;

        public SequenceGenerator(IRandomGenerator<T> source)
        {
            mSource = source;
        }

        public IEnumerable<T> Generate(int count)
        {
            for (int i = 0; i < count; i++)
                yield return mSource.Next();
        }
    }
}
