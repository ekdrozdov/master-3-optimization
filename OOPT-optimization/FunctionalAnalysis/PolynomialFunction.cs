using System;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;

namespace OOPT.Optimization.FunctionalAnalysis.Functions
{
  public class PolynomialFunction<T> : IParametricFunction<T>
  {
    private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

    public T Value(IVector<T> parameters, IVector<T> point)
    {
      if (point.Count != 1)
      {
        throw new ArgumentException("Point dimension must be 1", nameof(point));
      }

      var la = LinearAlgebra.Value;
      var sum = la.GetZeroValue();

      for (var i = 0; i < parameters.Count; i++)
      {
        la.Add(ref sum, la.Mult(parameters[i], la.Pow(point[0], parameters.Count - i - 1)));
      }

      return sum;
    }

    public IFunction<T> Bind(IVector<T> parameters)
        => new BaseFunction<T>(point => Value(parameters, point));
  }
}