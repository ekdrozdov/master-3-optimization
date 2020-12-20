using System;
using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functions
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