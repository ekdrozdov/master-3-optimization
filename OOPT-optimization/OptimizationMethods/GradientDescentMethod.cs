using System;
using System.Linq;
using OOPT.Optimization.Algebra.Extensions;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;
using OOPT.Optimization.FunctionalAnalysis.Functionals;
using OOPT.Optimization.FunctionalAnalysis.Functionals.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functions;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;
using OOPT.Optimization.OptimizationMethods.Interfaces;

namespace OOPT.Optimization.OptimizationMethods
{
    public class GradientDescentMethod<T> : IOptimizer<T> where T : unmanaged
    {
        private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

        private T Eps { get; set; }

        private int MaxIteration { get; set; }

        public GradientDescentMethod(int maxIteration, T eps)
        {
            MaxIteration = maxIteration;
            Eps = eps;
        }

        private IVector<T> MinimizeInternal(IDifferentiableFunctional<T> objective, IParametricFunction<T> function, IVector<T> initialParameters, IVector<T> minimumParameters = default,
            IVector<T> maximumParameters = default)
        {
            var la = LinearAlgebra.Value;

            var xOld = initialParameters.Clone() as IVector<T>;

            //Calculate initial gradient
            var bindF = function.Bind(xOld);
            var fiNew = objective.Gradient(bindF);
            var xNew = initialParameters.Clone() as IVector<T>;
            var k = 0;
            var normOld = la.Sqrt(la.Dot(fiNew.ToArray(), fiNew.ToArray()));
            var normNew = la.Cast(1000);
            while (k++ < MaxIteration && (la.Compare(la.Sqrt(la.Dot(fiNew.ToArray(), fiNew.ToArray())), Eps) == 1) && (la.Compare(la.Sub(normNew, normOld), Eps) == 1)) 
            {
                bindF = function.Bind(xNew);
                fiNew = objective.Gradient(bindF);
                normOld = normNew;
                normNew = la.Sqrt(la.Dot(fiNew.ToArray(), fiNew.ToArray()));

                var gamma = GoldenRatioMethod<T>.FindMin(objective, function, xOld, fiNew, Eps);

                xOld = xNew.Clone() as IVector<T>;
                xNew = xOld.Sub(fiNew.MultWithCloning(gamma));
                this.ApplyMinimumAndMaximumValues(minimumParameters, maximumParameters, xNew, la);
            }

            return xNew;
        }

        public IVector<T> Minimize(IFunctional<T> objective, IParametricFunction<T> function, IVector<T> initialParameters, IVector<T> minimumParameters = default,
            IVector<T> maximumParameters = default)
        {
            if (!(objective is IDifferentiableFunctional<T> o1))
            {
                throw new ArgumentException("This optimizer accept only IDifferentiableFunctional", nameof(objective));
            }

            return MinimizeInternal(o1, function, initialParameters, minimumParameters, maximumParameters);
        }
    }
}