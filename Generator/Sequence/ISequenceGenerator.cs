using System;
using System.Collections.Generic;
using System.Text;

namespace Generator
{
    public interface ISequenceGenerator<T>
    {
        IEnumerable<T> Generate();
    }
}
