using System.Collections.Generic;
using System.Linq;
using OOPT.Optimization.Algebra.DiophantineEquations;
using Xunit;

namespace OptimizationTest
{
    public class DiophantineEquationTests
    {
        [Theory]
        [InlineData(true, 1L, 1L)]
        [InlineData(false, 5L, 3L)]
        [InlineData(false, 5L, 2L, 2L)]
        [InlineData(true, 1L, 1L, 1L)]
        [InlineData(true, 5L, 3L, 2L)]
        [InlineData(true, 16L, 8L, 4L)]
        public void OneEquation(bool result, long right, params long[] parameters)
        {
            var de = new DiophantineEquation(1, parameters.Length);
            de.SetMatrix(new[] {parameters.Append(right)});
            var solve = de.Solve();
            Assert.Equal(solve, result);

            if (solve)
            {
                Assert.True(de.CheckResult());
            }
        }

        private Dictionary<string, long[][]> DictionaryOfSlae = new Dictionary<string, long[][]>()
        {
            ["1"] = new[]
                {new[] {1, 0, 0, 1L}, new[] {0, 1, 0, 2L}, new[] {0, 0, 1, 3L}},
            ["2"] = new[]
                {new[] {1, 0, 1, 1L}, new[] {0, 1, 0, 2L}, new[] {1, 0, 1, 3L}},
            ["3"] = new[]
                {new[] {1, 0, 1, 1L}, new[] {0, 1, 0, 2L}, new[] {2, 0, 2, 2L}},
            ["4"] = new[]
                {new[] {1, 0, 1, 1L}, new[] {0, 1, 0, 2L}},
            ["5"] = new[]
                {new[] {1, 0, 1, 1L}, new[] {0, 1, -1, 2L}},
            ["6"] = new[]
                {new[] {1, 1, 1, 1L}, new[] {2, 2, 2, 4L}},
        };

        [Theory]
        [InlineData("1", true)]
        [InlineData("2", false)]
        [InlineData("3", true)]
        [InlineData("4", true)]
        [InlineData("5", true)]
        [InlineData("6", false)]
        public void SlaeEquation(string name, bool result)
        {
            CheckSlae(DictionaryOfSlae[name], result);
        }

        public void CheckSlae(long[][] slae, bool result)
        {
            var de = new DiophantineEquation(slae.Length, slae.Any() ? slae[0].Length - 1 : 0);
            de.SetMatrix(slae);
            var solve = de.Solve();
            Assert.Equal(result, solve);

            if (solve)
            {
                Assert.True(de.CheckResult());
            }
        }
    }
}