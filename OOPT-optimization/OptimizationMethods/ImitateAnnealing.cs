using System;
using MathNet.Numerics.Distributions;
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
    public class ImitateAnnealing<T> : IOptimizer<T> where T : unmanaged
    {
        private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

        public ImitateAnnealing(int? maxIter, T eps)
        {
            MaxIter = maxIter;
            Eps = eps;
        }

        private T Eps { get; set; }

        private int? MaxIter { get; set; }

        private double Mean { get; set; } = 0;

        private double StdDev { get; set; } = 1;

        public IVector<T> Minimize(IFunctional<T> objective, IParametricFunction<T> function, IVector<T> initialParameters, IVector<T> minimumParameters = default,
            IVector<T> maximumParameters = default)
        {
            var k = 0;
            var la = LinearAlgebra.Value;
            IVector<T> xPrev = initialParameters.Clone() as IVector<T>;
            IVector<T> xNew = initialParameters.Clone() as IVector<T>;

            var normalDist = new Normal(Mean, StdDev);
            T prevValue = objective.Value(function.Bind(xPrev));

            do
            {
                var t = 20d / Math.Log(k, Math.E);

                for (int i = 0; i < xPrev.Count; i++)
                {
                    var nR = normalDist.Sample() * t;
                    xNew[i] = la.Sum(xPrev[i], la.Cast(nR));
                }

                this.ApplyMinimumAndMaximumValues(minimumParameters, maximumParameters, xNew, la);

                var newValue = objective.Value(function.Bind(xNew));

                var sub = la.Sub(newValue, prevValue);

                if (la.Compare(sub, la.GetZeroValue()) == -1) // || la.Exp(la.Mult(la.Cast(-1/t), sub)) >= rand.NextDouble())
                {
                    prevValue = newValue;
                    xPrev = xNew.Clone() as IVector<T>;
                }
            } while ((MaxIter.HasValue && MaxIter > k++ && la.Compare(prevValue, Eps) == 1) || (!MaxIter.HasValue && la.Compare(prevValue, Eps) == 1));

            return xPrev;
        }
    }
}