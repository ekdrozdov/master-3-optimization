using System;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functions
{
    public class BaseFunction<T> : IFunction<T> where T : unmanaged
    {
        protected readonly Func<IVector<T>, T> F;

        public BaseFunction(Func<IVector<T>, T> f)
        {
            F = f;
        }

        public T Value(IVector<T> point) => F(point);
    }
}