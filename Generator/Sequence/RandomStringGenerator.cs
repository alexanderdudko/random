using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generator.Distribution;
using Generator.RandomGenerator;
using Generator.RandomGenerator.Discrete;
using Generator.RandomGenerator.Uniform;

namespace Generator.Sequence
{
    public class RandomStringGenerator : SequenceGenerator<char>
    {
        public RandomStringGenerator(string chars = "abcdefghijklmnopqrstuvwxyz") : base(new DiscreteFiniteSetValueGenerator<char>(new EmpiricalDataDistribution<char>(chars.ToCharArray()), new SystemUniformRandomSource()))
        { }

        public string GetString(int length)
        {
            return new string(Generate(length).ToArray());
        }
    }
}
