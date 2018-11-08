using System;
using System.Collections.Generic;
using System.Text;

namespace Generator
{
    public interface IRandomSource<T>
    {
        T Next();
    }
}
