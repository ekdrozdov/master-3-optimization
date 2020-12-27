using System;
using System.Collections.Generic;
using System.Linq;
using OOPT.Optimization.Algebra;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functions;
using Xunit;

namespace OptimizationTest
{
    public class PiecewiseLinearFunctionTest
    {
        [Fact]
        public void Value()
        {
            var f = new PiecewiseLinearFunction<double>(new List<IVector<double>>() {new Vector<double>(1d), new Vector<double>(2d)});

            Assert.True(1e-14 > Math.Abs(0 - f.Value(new Vector<double>(new[] {0d, 0d, 3d, 3d, 10d, 10d}),
                                                     new Vector<double>(0.5d))));

            Assert.True(1e-14 > Math.Abs(7.5 - f.Value(new Vector<double>(new[] {0d, 0d, 3d, 3d, 10d, 10d}),
                                                       new Vector<double>(1.5d))));

            Assert.True(1e-14 > Math.Abs(40 - f.Value(new Vector<double>(new[] {0d, 0d, 3d, 3d, 10d, 10d}),
                                                      new Vector<double>(3d))));

            Assert.True(1e-14 > Math.Abs(30 - f.Value(new Vector<double>(new[] {0d, 0d, 3d, 3d, 10d, 10d}),
                                                      new Vector<double>(new[] {2d}))));

            Assert.True(1e-14 > Math.Abs(6 - f.Value(new Vector<double>(new[] {0d, 0d, 3d, 3d, 10d, 10d}),
                                                     new Vector<double>(new[] {1d}))));
        }

        [Fact]
        public void Gradient()
        {
            var f = new PiecewiseLinearFunction<double>(new List<IVector<double>>() {new Vector<double>(1d), new Vector<double>(2d)});

            var grad = f.Gradient(new Vector<double>(new[] {0d, 0d, 3d, 3d, 10d, 10d}),
                                  new Vector<double>(0.5d));

            Assert.True(grad.Count == 6);
            Assert.True(grad.Skip(2).All(x => Math.Abs(x) < 1e-14));
            Assert.True(Math.Abs(grad[0] - 0.5) < 1e-14);
            Assert.True(Math.Abs(grad[1] - 1) < 1e-14);

            grad = f.Gradient(new Vector<double>(new[] {0d, 0d, 3d, 3d, 10d, 10d}),
                              new Vector<double>(1.5d));

            Assert.True(grad.Count == 6);
            Assert.True(grad.Take(2).All(x => Math.Abs(x) < 1e-14));
            Assert.True(grad.TakeLast(2).All(x => Math.Abs(x) < 1e-14));
            Assert.True(Math.Abs(grad[2] - 1.5) < 1e-14);
            Assert.True(Math.Abs(grad[3] - 1) < 1e-14);

            grad = f.Gradient(new Vector<double>(new[] {0d, 0d, 3d, 3d, 10d, 10d}),
                              new Vector<double>(3d));

            Assert.True(grad.Count == 6);
            Assert.True(grad.Take(4).All(x => Math.Abs(x) < 1e-14));
            Assert.True(Math.Abs(grad[4] - 3) < 1e-14);
            Assert.True(Math.Abs(grad[5] - 1) < 1e-14);

            grad = f.Gradient(new Vector<double>(new[] {0d, 0d, 3d, 3d, 10d, 10d}),
                              new Vector<double>(2d));

            Assert.True(grad.Count == 6);
            Assert.True(grad.Count == 6);
            Assert.True(grad.Take(4).All(x => Math.Abs(x) < 1e-14));
            Assert.True(Math.Abs(grad[4] - 2) < 1e-14);
            Assert.True(Math.Abs(grad[5] - 1) < 1e-14);

            Assert.True(1e-14 > 6 - f.Value(new Vector<double>(new[] {0d, 0d, 3d, 3d, 10d, 10d}),
                                            new Vector<double>(new[] {1d})));

            grad = f.Gradient(new Vector<double>(new[] {0d, 0d, 3d, 3d, 10d, 10d}),
                              new Vector<double>(1.5d));

            Assert.True(grad.Count == 6);
            Assert.True(grad.Take(2).All(x => Math.Abs(x) < 1e-14));
            Assert.True(grad.TakeLast(2).All(x => Math.Abs(x) < 1e-14));
            Assert.True(Math.Abs(grad[2] - 1.5) < 1e-14);
            Assert.True(Math.Abs(grad[3] - 1) < 1e-14);
        }
    }
}