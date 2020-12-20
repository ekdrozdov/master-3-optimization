using System;
using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functions
{
  public class BaseDifferentiableFunction<T> : BaseFunction<T>, IDifferentiableFunction<T>
  {
    protected readonly Func<IVector<T>, IVector<T>> df;

    public BaseDifferentiableFunction(Func<IVector<T>, T> f, Func<IVector<T>, IVector<T>> df) : base(f)
    {
      this.df = df;
    }

    public IVector<T> Gradient(IVector<T> point) => df(point);
  }
}