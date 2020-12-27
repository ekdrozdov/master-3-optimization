using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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
    public class NonLinearConjugateGradientMethod<T> : IOptimizer<T> where T : unmanaged
    {
        private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

        private T Eps { get; set; }

        private int MaxIteration { get; set; }

        private int SpaceSize { get; set; }

        public NonLinearConjugateGradientMethod(int maxIteration, T eps, int spaceSize)
        {
            MaxIteration = maxIteration;
            Eps = eps;
            SpaceSize = spaceSize;
        }

        private IVector<T> MinimizeInternal(IDifferentiableFunctional<T> objective, IParametricFunction<T> function, IVector<T> initialParameters, IVector<T> minimumParameters = default,
            IVector<T> maximumParameters = default)
        {
            var la = LinearAlgebra.Value;


            var xNew = initialParameters.Clone() as IVector<T>;
            var bindF = function.Bind(xNew);
            var curVal = objective.Value(bindF);
            var prevVal = curVal;
            var gradient = objective.Gradient(bindF);
            var p = objective.Gradient(bindF).Clone() as IVector<T>;
            gradient.Mult(la.Cast(-1));
            var gradSquare = la.Dot(p.ToArray(), p.ToArray());

            int numIter = 0;

            do
            {
                T alpha, beta, newGradSquare;
                IVector<T> newGrad;

                //Ищем минимум F(x + alpha * p) с помощью метода одномерной оптимизации
                alpha = GoldenRatioMethod<T>.FindMin(objective, function, xNew, p, Eps);
                xNew = xNew.Add(p.MultWithCloning(la.Mult(la.Cast(-1), alpha)));

                this.ApplyMinimumAndMaximumValues(minimumParameters, maximumParameters, xNew, la);
                bindF = function.Bind(xNew);
                newGrad = objective.Gradient(bindF).Mult(la.Cast(-1));
                newGradSquare = la.Dot(newGrad.ToArray(), newGrad.ToArray());

                beta = numIter % (5 * SpaceSize) == 0
                    ? la.GetZeroValue()
                    : la.Div(la.Mult(la.Cast(-1), la.Sub(newGradSquare, la.Dot(newGrad.ToArray(), gradient.ToArray()))), gradSquare);

                p.Mult(beta).Add(newGrad);

                prevVal = curVal;
                curVal = objective.Value(bindF);

                gradient = newGrad;
                gradSquare = newGradSquare;
            } while (la.Compare(gradSquare, Eps) == 1 && MaxIteration > numIter++);

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