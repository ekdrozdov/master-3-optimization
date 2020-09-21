using System;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis
{
    public class BaseFunction<T> : IFunction<T>
    {
        protected readonly Func<IVector<T>, T> F;

        public BaseFunction(Func<IVector<T>, T> f)
        {
            F = f;
        }

        public T Value(IVector<T> point) => F(point);
    }
}