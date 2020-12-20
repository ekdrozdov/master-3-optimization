using System;
using System.Collections.Generic;
using System.Linq;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;
using OOPT.Optimization.FunctionalAnalysis.Functions;

namespace OOPT.Optimization.FunctionalAnalysis.Functionals
{
  public class LInfFunctional<T> : IFunctional<T>
  {
    private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

    private readonly (IVector<T> point, T target)[] _elements;

    public LInfFunctional(IEnumerable<(IVector<T>, T)> points)
    {
      var incoming = points.ToArray();

      if (!incoming.Any())
      {
        throw new ArgumentException("Need at least one item");
      }

      _elements = new (IVector<T>, T)[incoming.LongLength];
      incoming.ToArray().CopyTo(_elements, 0);
    }

    public T Value(IFunction<T> f)
    {
      var la = LinearAlgebra.Value;

      return la.Max(_elements, (x) => la.Abs(la.Sub(f.Value(x.point), x.target)));
    }
  }
}