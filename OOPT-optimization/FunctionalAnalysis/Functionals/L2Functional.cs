using System;
using System.Collections.Generic;
using System.Linq;
using OOPT.Optimization.Algebra;
using OOPT.Optimization.Algebra.Extensions;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;
using OOPT.Optimization.FunctionalAnalysis.Functions;

namespace OOPT.Optimization.FunctionalAnalysis.Functionals
{
  public class L2Functional<T> : IDifferentiableFunctional<T>, ILeastSquaresFunctional<T>
  {
    private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

    private readonly (IVector<T> point, T target)[] _elements;

    public L2Functional(IEnumerable<(IVector<T>, T)> points)
    {
      var incoming = points.ToArray();

      if (!incoming.Any())
      {
        throw new ArgumentException("Need at least one item");
      }

      _elements = new (IVector<T>, T)[incoming.LongLength];
      incoming.ToArray().CopyTo(_elements, 0);
    }

    //sum ((f(xi)-realValue(xi))*f')/(sqrt of sum (f(xi)-realValue(xi))^2)
    public IVector<T> Gradient(IDifferentiableFunction<T> f)
    {
      var la = LinearAlgebra.Value;

      return _elements.Aggregate((IVector<T>)new Vector<T>(_elements[0].point.Count),
                                 (prev, curr) => prev.Add(
                                                          f.Gradient(curr.point)
                                                              .Mult(la.Sub(f.Value(curr.point), curr.target))))
          .Div((Value(f)));
    }

    //sqrt of sum (f(xi)-realValue(xi))^2
    public T Value(IFunction<T> f)
    {
      var la = LinearAlgebra.Value;

      return la.Pow(la.Sum(_elements,
                           x => la
                               .Abs(la.Pow(la.Sub(f.Value(x.point),
                                                  x.target),
                                           2))),
                    la.Cast(0.5d));
    }

    //Really residual this sh*t or i am idiot?
    public IVector<T> Residual(IDifferentiableFunction<T> function) =>
        new Vector<T>(_elements.Select(x => LinearAlgebra.Value.Sub(function.Value(x.point), x.target)));

    //Really Jacobian this sh*t or i am idiot?
    public IMatrix<T> Jacobian(IDifferentiableFunction<T> function) =>
        new Matrix<T>(_elements.Select(x => function.Gradient(x.point)));
  }
}