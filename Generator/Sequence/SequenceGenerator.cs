using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.Sequence
{
    class SequenceGenerator<T> : ISequenceGenerator<T>
    {
        public IEnumerable<T> Generate()
        {
            throw new NotImplementedException();
        }
    }
}
