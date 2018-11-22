using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.RandomGenerator
{
    public interface IRandomGenerator<T>
    {
        T Next();
    }
}
