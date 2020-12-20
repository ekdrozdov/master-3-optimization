using System;
using System.Linq;
using OOPT.Optimization.Algebra;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;
using OOPT.Optimization.FunctionalAnalysis.Functions;

namespace OOPT.Optimization.MathAnalysis.Integrates
{
  public static class GaussLegendreRule<T>
  {
    private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

    public static T Integrate(IFunction<T> function, IVector<T> from, IVector<T> to, int order)
    {
      if (from.Count != to.Count)
      {
        throw new ArgumentOutOfRangeException();
      }

      var point = GaussLegendrePointFactory.GetGaussPoint(from[0], to[0], order);
      var sum = LinearAlgebra.Value.GetZeroValue();
      for (var i = 0; i < point.Abscissas.Length; i++)
      {
        LinearAlgebra.Value.Add(ref sum, IntegrateInternal(function, new Vector<T>(new[] { point.Abscissas[i] }), point.Weights[i], 0, from, to, order));
      }

      return sum;
    }
    private static T IntegrateInternal(IFunction<T> function, IVector<T> currentParameters, T currentWeight, int currentIndex, IVector<T> from, IVector<T> to, int order)
    {
      if (currentIndex == from.Count - 1)
      {
        return LinearAlgebra.Value.Mult(function.Value(currentParameters), currentWeight);
      }

      currentIndex++;
      var point = GaussLegendrePointFactory.GetGaussPoint(from[currentIndex], to[currentIndex], order);

      var sum = LinearAlgebra.Value.GetZeroValue();
      for (var i = 0; i < point.Abscissas.Length; i++)
      {
        LinearAlgebra.Value.Add(ref sum, IntegrateInternal(function, new Vector<T>(currentParameters.Union(new[] { point.Abscissas[i] })), LinearAlgebra.Value.Mult(point.Weights[i], currentWeight), currentIndex, from, to, order));
      }

      return sum;
    }
  }
}
