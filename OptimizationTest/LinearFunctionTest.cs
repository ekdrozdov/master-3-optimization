using System;
using System.Linq;
using OOPT.Optimization.Algebra;
using OOPT.Optimization.FunctionalAnalysis.Functions;
using Xunit;

namespace OptimizationTest
{
    public class LinearFunctionTest
    {
        [Fact]
        public void Value()
        {
            var f = new LinearFunction<double>();

            Assert.True(1e-14 > Math.Abs(2 - f.Value(new Vector<double>(new[] {2d}),
                                                     new Vector<double>(new double[0]))));

            Assert.True(1e-14 > Math.Abs(3 - f.Value(new Vector<double>(new[] {3d}),
                                                     new Vector<double>(new double[0]))));

            Assert.True(1e-14 > Math.Abs(7 - f.Value(new Vector<double>(new[] {3d, 1d}),
                                                     new Vector<double>(new[] {2d}))));

            Assert.True(1e-14 > Math.Abs(10 - f.Value(new Vector<double>(new[] {3d, 1d}),
                                                      new Vector<double>(new[] {3d}))));
        }

        [Fact]
        public void Gradient()
        {
            var f = new LinearFunction<double>();

            var grad = f.Gradient(new Vector<double>(new[] {2d}),
                                  new Vector<double>(new double[0])).ToArray();

            Assert.Single(grad);

            Assert.True(1e-14 > Math.Abs(1 - grad[0]));

            grad = f.Gradient(new Vector<double>(new[] {2d, 1d}),
                              new Vector<double>(new[] {2d})).ToArray();

            Assert.True(grad.Length == 2);
            Assert.True(1e-14 > Math.Abs(2 - grad[0]));
            Assert.True(1e-14 > Math.Abs(1 - grad[1]));

            grad = f.Gradient(new Vector<double>(new[] {2d, 3d, 1d}),
                              new Vector<double>(new[] {2d, 3d})).ToArray();

            Assert.True(grad.Length == 3);
            Assert.True(1e-14 > Math.Abs(2 - grad[0]));
            Assert.True(1e-14 > Math.Abs(3 - grad[1]));
            Assert.True(1e-14 > Math.Abs(1 - grad[2]));
        }
    }
}