using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.Sequence
{
    public interface ISequenceGenerator<T>
    {
        IEnumerable<T> Generate(int count);
    }
}
