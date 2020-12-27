using System;
using System.Linq;
using OOPT.Optimization.Algebra.Extensions;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;
using OOPT.Optimization.FunctionalAnalysis.Functionals.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;
using OOPT.Optimization.OptimizationMethods.Interfaces;

namespace OOPT.Optimization.OptimizationMethods
{
    public class GaussNewton<T> : IOptimizer<T> where T : unmanaged
    {
        private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

        private T Eps { get; set; }

        private int MaxIteration { get; set; }

        public GaussNewton(int maxIteration, T eps)
        {
            MaxIteration = maxIteration;
            Eps = eps;
        }

        private IVector<T> MinimizeInternal(ILeastSquaresFunctional<T> objective, IParametricFunction<T> function, IVector<T> initialParameters, IVector<T> minimumParameters = default,
            IVector<T> maximumParameters = default)
        {
            var la = LinearAlgebra.Value;
            var x = initialParameters.Clone() as IVector<T>;

            var bindF = function.Bind((x));
            var iter = 0;
            var residual = objective.Residual(bindF);
            var error = la.Sqrt(la.Dot(residual.ToArray(), residual.ToArray()));
            var oldError = la.Cast(1000);

            while (la.Compare(la.Sub(error, oldError), Eps) == 1 || iter++ < MaxIteration)
            {
                var jacobi = objective.Jacobian(bindF);
                var jacobiT = jacobi.Transpose(); //JT
                var jTj = jacobiT.Mult(jacobi); //jTj
                var jTj_1 = jTj.Inverse();
                var jTj_1jT = jTj_1.Mult(jacobiT);
                var temp = jTj_1jT.Mult(residual);
                var gamma = GoldenRatioMethod<T>.FindMin(objective, function, x, temp, Eps);
                x.Sub(temp.Mult(gamma));
                this.ApplyMinimumAndMaximumValues(minimumParameters, maximumParameters, x, la);
                oldError = error;
                bindF = function.Bind(x);
                residual = objective.Residual(bindF);
                error = la.Sqrt(la.Dot(residual.ToArray(), residual.ToArray()));
            }

            return x;
        }

        public IVector<T> Minimize(IFunctional<T> objective, IParametricFunction<T> function, IVector<T> initialParameters, IVector<T> minimumParameters = default,
            IVector<T> maximumParameters = default)
        {
            if (!(objective is ILeastSquaresFunctional<T> o1))
            {
                throw new ArgumentException("This optimizer accept only IDifferentiableFunctional", nameof(objective));
            }

            return MinimizeInternal(o1, function, initialParameters, minimumParameters, maximumParameters);
        }
    }
}