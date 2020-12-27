using System;
using OOPT.Optimization.Algebra;
using OOPT.Optimization.FunctionalAnalysis.Functions;
using Xunit;

namespace OptimizationTest
{
    public class LagrangeSplineTest
    {
        [Fact]
        public void Value()
        {
            var f = new LagrangeSpline<double>(2, (new Vector<double>(2d), 4), (new Vector<double>(3d), 9), (new Vector<double>(4d), 16));
            Assert.True(1e-14 > Math.Abs(4 - f.Value(new Vector<double>(2d))));

            f = new LagrangeSpline<double>(2, (new Vector<double>(2d), 6), (new Vector<double>(3d), 11), (new Vector<double>(4d), 18));
            Assert.True(1e-14 > Math.Abs(6 - f.Value(new Vector<double>(2d))));

            f = new LagrangeSpline<double>(2, (new Vector<double>(2d), 6), (new Vector<double>(3d), 11), (new Vector<double>(4d), 18), (new Vector<double>(1.2d), Math.Pow(1.2, 2) + 2),
                                           (new Vector<double>(1.5d), Math.Pow(1.5, 2) + 2));

            Assert.True(1e-14 > Math.Abs(Math.Pow(2d, 2) + 2 - f.Value(new Vector<double>(2d))));
            Assert.True(1e-14 > Math.Abs(Math.Pow(1.2d, 2) + 2 - f.Value(new Vector<double>(1.2d))));
            Assert.True(1e-14 > Math.Abs(Math.Pow(1.3d, 2) + 2 - f.Value(new Vector<double>(1.3d))));
            Assert.True(1e-14 > Math.Abs(Math.Pow(1.1d, 2) + 2 - f.Value(new Vector<double>(1.1d))));
            Assert.True(1e-14 > Math.Abs(Math.Pow(5d, 2) + 2 - f.Value(new Vector<double>(5d))));
        }
    }
}