using GeneratorCLI.Search;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GeneratorCLIUnitTest
{
    public class ParameterSearchTests
    {
        [Fact]
        public void SearchIncreasingLinearFunctionTest()
        {
            Func<double, double> f = x => 5 * x + 9;
            double searchTargetY = 15;
            double minX = 0;
            double maxX = 4;

            double searchResult = ParameterSearch.SearchParameterValue(f, searchTargetY, minX, maxX, 0.001);

            Assert.Equal(1.2, searchResult, 3);
        }

        [Fact]
        public void SearchDecreasingLinearFunctionTest()
        {
            Func<double, double> f = x => -2 * x + 6;
            double searchTargetY = 15;
            double minX = -10;
            double maxX = 10;

            double searchResult = ParameterSearch.SearchParameterValue(f, searchTargetY, minX, maxX, 0.001);

            Assert.Equal(-4.5, searchResult, 3);
        }

        [Fact]
        public void SearchTargetOutsideBoudariesTest()
        {
            Func<double, double> f = x => 5 * x + 9;
            double searchTargetY = 15;
            double minX = 5;
            double maxX = 10;

            double searchResult = ParameterSearch.SearchParameterValue(f, searchTargetY, minX, maxX, 0.001);

            Assert.Equal(minX, searchResult, 3);
        }
    }
}
