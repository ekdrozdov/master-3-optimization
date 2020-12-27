using System;
using OOPT.Optimization.Algebra;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functionals;
using OOPT.Optimization.FunctionalAnalysis.Functions;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;
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
            var point2 = new Vector<double>(new[] {2d});
            var point3 = new Vector<double>(new[] {3d});

            var functional = new L1Functional<double>(new[]
            {
                ((IVector<double>) point1, 3d)
            });

            Assert.True(1e-14 > Math.Abs(0 - functional.Value(f.Bind(new Vector<double>(new[] {3d})))));
            Assert.True(1e-14 > Math.Abs(1 - functional.Value(f.Bind(new Vector<double>(new[] {2d})))));

            functional = new L1Functional<double>(new[]
            {
                ((IVector<double>) point2, 3d)
            });

            Assert.True(1e-14 > Math.Abs(2 - functional.Value(f.Bind(new Vector<double>(new[] {1d, 3d})))));
            Assert.True(1e-14 > Math.Abs(3 - functional.Value(f.Bind(new Vector<double>(new[] {2d, 2d})))));

            functional = new L1Functional<double>(new[]
            {
                ((IVector<double>) point2, 3d),
                ((IVector<double>) point3, 7d)
            });

            Assert.True(1e-14 > Math.Abs(3 - functional.Value(f.Bind(new Vector<double>(new[] {1d, 3d})))));
            Assert.True(1e-14 > Math.Abs(4 - functional.Value(f.Bind(new Vector<double>(new[] {2d, 2d})))));
        }

        [Fact]
        public void Gradient()
        {
            var f = (IParametricFunction<double>) new LinearFunction<double>();
            var point1 = new Vector<double>(new double[0]);
            var point2 = new Vector<double>(new[] {2d});
            var point3 = new Vector<double>(new[] {3d});
            var point4 = new Vector<double>(new[] {1d, 4d});
            var point5 = new Vector<double>(new[] {6d, 1d});

            var functional = new L1Functional<double>(new[]
            {
                ((IVector<double>) point1, 3d)
            });

            var grad = functional.Gradient(f.Bind(new Vector<double>(new[] {3d})));
            Assert.True(1e-14 > Math.Abs(1 - grad[0]));
            Assert.Single(grad);
            grad = functional.Gradient(f.Bind(new Vector<double>(new[] {2d})));
            Assert.True(1e-14 > Math.Abs(-1 - grad[0]));
            Assert.Single(grad);

            functional = new L1Functional<double>(new[]
            {
                ((IVector<double>) point2, 3d)
            });

            grad = functional.Gradient(f.Bind(new Vector<double>(new[] {1d, 3d})));
            Assert.True(1e-14 > Math.Abs(2 - grad[0]));
            Assert.True(1e-14 > Math.Abs(1 - grad[1]));
            Assert.True(grad.Count == 2);

            functional = new L1Functional<double>(new[]
            {
                ((IVector<double>) point4, 3d)
            });

            grad = functional.Gradient(f.Bind(new Vector<double>(new[] {1d, 2d, 3d})));
            Assert.True(1e-14 > Math.Abs(1 - grad[0]));
            Assert.True(1e-14 > Math.Abs(4 - grad[1]));
            Assert.True(1e-14 > Math.Abs(1 - grad[2]));
            Assert.True(grad.Count == 3);

            functional = new L1Functional<double>(new[]
            {
                ((IVector<double>) point4, 3d),
                ((IVector<double>) point5, 3d)
            });

            grad = functional.Gradient(f.Bind(new Vector<double>(new[] {1d, 2d, 3d})));
            Assert.True(1e-14 > Math.Abs(7 - grad[0]));
            Assert.True(1e-14 > Math.Abs(5 - grad[1]));
            Assert.True(1e-14 > Math.Abs(2 - grad[2]));
            Assert.True(grad.Count == 3);
        }
    }
}