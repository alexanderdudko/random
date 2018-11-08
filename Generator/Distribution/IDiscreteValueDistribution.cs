using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.Distribution
{
    public interface IDiscreteValueDistribution<T>
    {
        IEnumerable<T> Values { get; }
        double ValueProbability(T value);
    }
}
