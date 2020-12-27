using System;
using OOPT.Optimization.Algebra.Extensions;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;
using OOPT.Optimization.FunctionalAnalysis.Functionals;
using OOPT.Optimization.FunctionalAnalysis.Functionals.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functions;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;

namespace OOPT.Optimization.OptimizationMethods
{
    public static class GoldenRatioMethod<T> where T : unmanaged
    {
        private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

        public static T FindMin(IFunctional<T> functional, IParametricFunction<T> function, IVector<T> s, IVector<T> p, T eps)
        {
            var la = LinearAlgebra.Value;

            var a = la.Cast(0);
            var b = la.Cast(1e2);
            var x = la.Sum(a, la.Mult(la.Mult(la.Cast(0.5), la.Sub(la.Cast(3), la.Sqrt(la.Cast(5.0)))), (la.Sub(b, a))));
            var y = la.Sum(la.Sub(b, x), a);

            var fx = function.Bind(s.AddWithCloning(p.MultWithCloning(la.Mult(x, la.Cast(-1)))));
            var fy = function.Bind(s.AddWithCloning(p.MultWithCloning(la.Mult(y, la.Cast(-1)))));
            var valueX = functional.Value(fx);

            var valueY = functional.Value(fy);
            while (la.Compare(la.Abs(la.Sub(b, a)), la.Cast(1e-5)) == 1)
            {
                if (la.Compare(valueX, valueY) == -1)
                {
                    b = y;
                    y = x;
                    fy = fx;
                    valueY = valueX;
                    x = la.Sub(la.Sum(b, a), y);
                    fx = function.Bind(s.AddWithCloning(p.MultWithCloning(la.Mult(x, la.Cast(-1)))));
                    valueX = functional.Value(fx);
                }
                else
                {
                    a = x;
                    x = y;
                    fx = fy;
                    valueX = valueY;
                    y = la.Sub(la.Sum(b, a), x);
                    fy = function.Bind(s.AddWithCloning(p.MultWithCloning(la.Mult(y, la.Cast(-1)))));
                    valueY = functional.Value(fy);
                }
            }
            return (la.Div(la.Sum(a, b), la.Cast(2)));
        }
    }
}