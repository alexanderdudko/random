using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.RandomGenerator
{
    public interface IRandomSource<T>
    {
        T Next();
    }
}
