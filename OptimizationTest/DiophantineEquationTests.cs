using System.Collections.Generic;
using System.Linq;
using OOPT.Optimization.Algebra;
using OOPT.Optimization.Algebra.DiophantineEquations;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functionals;
using OOPT.Optimization.FunctionalAnalysis.Functions;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;
using OOPT.Optimization.OptimizationMethods;
using Xunit;

namespace OptimizationTest
{
    public class L1Functional
    {
        [Fact]
        public void Value()
        {
            var f = new LinearFunction<double>();
            var point1 = new Vector<double>(new double[0]);
            var point2 = new Vector<double>(new[] { 2d });
            var point3 = new Vector<double>(new[] { 3d });

            var functional = new L1Functional<double>(new[]
            {
                ((IVector<double>)point1, 3d)
            });
            Assert.True(1e-14 > 0 - functional.Value(f.Bind(new Vector<double>(new[] { 3d }))));
            Assert.True(1e-14 > 1 - functional.Value(f.Bind(new Vector<double>(new[] { 2d }))));
            functional = new L1Functional<double>(new[]
            {
                ((IVector<double>)point2, 3d)
            });
            Assert.True(1e-14 > 2 - functional.Value(f.Bind(new Vector<double>(new[] { 1d, 3d }))));
            Assert.True(1e-14 > 3 - functional.Value(f.Bind(new Vector<double>(new[] { 2d, 2d }))));
            functional = new L1Functional<double>(new[]
            {
                ((IVector<double>)point2, 3d),
                ((IVector<double>)point3, 7d)
            });
            Assert.True(1e-14 > 3 - functional.Value(f.Bind(new Vector<double>(new[] { 1d, 3d }))));
            Assert.True(1e-14 > 4 - functional.Value(f.Bind(new Vector<double>(new[] { 2d, 2d }))));
        }

        [Fact]
        public void Gradient()
        {
            var f = (IParametricFunction<double>)new LinearFunction<double>();
            var point1 = new Vector<double>(new double[0]);
            var point2 = new Vector<double>(new[] { 2d });
            var point3 = new Vector<double>(new[] { 3d });
            var point4 = new Vector<double>(new[] { 1d, 4d });
            var point5 = new Vector<double>(new[] { 6d, 1d });

            var functional = new L1Functional<double>(new[]
            {
                ((IVector<double>)point1, 3d)
            });

            var grad = functional.Gradient(f.Bind(new Vector<double>(new[] { 3d })));
            Assert.True(1e-14 > 0 - grad[0]);
            Assert.Single(grad);
            grad = functional.Gradient(f.Bind(new Vector<double>(new[] { 2d })));
            Assert.True(1e-14 > 0 - grad[0]);
            Assert.Single(grad);
            functional = new L1Functional<double>(new[]
            {
                ((IVector<double>)point2, 3d)
            });

            grad = functional.Gradient(f.Bind(new Vector<double>(new[] { 1d, 3d })));
            Assert.True(1e-14 > 2 - grad[0]);
            Assert.True(1e-14 > 0 - grad[1]);
            Assert.True(grad.Count == 2);
            functional = new L1Functional<double>(new[]
            {
                ((IVector<double>)point4, 3d)
            });
            grad = functional.Gradient(f.Bind(new Vector<double>(new[] { 1d, 2d, 3d })));
            Assert.True(1e-14 > 1 - grad[0]);
            Assert.True(1e-14 > 4 - grad[1]);
            Assert.True(1e-14 > 0 - grad[2]);
            Assert.True(grad.Count == 3);
            functional = new L1Functional<double>(new[]
            {
                ((IVector<double>)point4, 3d),
                ((IVector<double>)point5, 3d)

            });
            grad = functional.Gradient(f.Bind(new Vector<double>(new[] { 1d, 2d, 3d })));
            Assert.True(1e-14 > 7 - grad[0]);
            Assert.True(1e-14 > 5 - grad[1]);
            Assert.True(1e-14 > 0 - grad[2]);
            Assert.True(grad.Count == 3);
            functional = new L1Functional<double>(new[]
            {
                ((IVector<double>)point4, f.Bind(new Vector<double>(new[] { 1d, 2d, 3d })).Value(point4)),
               // ((IVector<double>)point5, f.Bind(new Vector<double>(new[] { 1d, 2d, 3d })).Value(point5))
            });
            var optimizer = new NonLinearConjugateGradientMethod<double>(1000,1e-14,100);
            optimizer.Minimize(functional, f, new Vector<double>(new[] {1d,10d, 10d}));
        }
    }
    public class PolynomialFunction
    {
        [Fact]
        public void Value()
        {
            var f = new PolynomialFunction<double>();

            Assert.True(1e-14 > 2 - f.Value(new Vector<double>(new[] { 2d }),
                                            new Vector<double>(new[] { 2d })));


            Assert.True(1e-14 > 3 - f.Value(new Vector<double>(new[] { 3d }),
                                            new Vector<double>(new[] { 2d })));

            Assert.True(1e-14 > 7 - f.Value(new Vector<double>(new[] { 3d, 1d }),
                                            new Vector<double>(new[] { 2d })));

            Assert.True(1e-14 > 34 - f.Value(new Vector<double>(new[] { 3d, 2d, 1d }),
                                             new Vector<double>(new[] { 3d })));
        }

        [Fact]
        public void Gradient()
        {
            var f = new PolynomialFunction<double>();

            var grad = f.Gradient(new Vector<double>(new[] { 2d }),
                                  new Vector<double>(new[] { 3d })).ToArray();

            Assert.Single(grad);

            Assert.True(1e-14 > 0 - grad[0]);

            grad = f.Gradient(new Vector<double>(new[] { 2d, 1d }),
                              new Vector<double>(new[] { 2d })).ToArray();

            Assert.True(grad.Length == 2);
            Assert.True(1e-14 > 2 - grad[0]);
            Assert.True(1e-14 > 0 - grad[1]);

            grad = f.Gradient(new Vector<double>(new[] { 2d, 3d, 1d }),
                              new Vector<double>(new[] { 2d })).ToArray();

            Assert.True(grad.Length == 3);
            Assert.True(1e-14 > 4 - grad[0]);
            Assert.True(1e-14 > 2 - grad[1]);
            Assert.True(1e-14 > 0 - grad[2]);
        }
    }
    public class LinearFunctionTest
    {
        [Fact]
        public void Value()
        {
            var f = new LinearFunction<double>();

            Assert.True(1e-14 > 2 - f.Value(new Vector<double>(new[] { 2d }),
                                            new Vector<double>(new double[0])));

            Assert.True(1e-14 > 3 - f.Value(new Vector<double>(new[] { 3d }),
                                            new Vector<double>(new double[0])));

            Assert.True(1e-14 > 7 - f.Value(new Vector<double>(new[] { 3d, 1d }),
                                            new Vector<double>(new[] { 2d })));

            Assert.True(1e-14 > 10 - f.Value(new Vector<double>(new[] { 3d, 1d }),
                                             new Vector<double>(new[] { 3d })));
        }

        [Fact]
        public void Gradient()
        {
            var f = new LinearFunction<double>();

            var grad = f.Gradient(new Vector<double>(new[] { 2d }),
                                  new Vector<double>(new double[0])).ToArray();

            Assert.Single(grad);

            Assert.True(1e-14 > 0 - grad[0]);

            grad = f.Gradient(new Vector<double>(new[] { 2d, 1d }),
                              new Vector<double>(new[] { 2d })).ToArray();

            Assert.True(grad.Length == 2);
            Assert.True(1e-14 > 2 - grad[0]);
            Assert.True(1e-14 > 0 - grad[1]);

            grad = f.Gradient(new Vector<double>(new[] { 2d, 3d, 1d }),
                              new Vector<double>(new[] { 2d, 3d })).ToArray();

            Assert.True(grad.Length == 3);
            Assert.True(1e-14 > 2 - grad[0]);
            Assert.True(1e-14 > 3 - grad[1]);
            Assert.True(1e-14 > 0 - grad[2]);
        }
    }

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
            de.SetMatrix(new[] { parameters.Append(right) });
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