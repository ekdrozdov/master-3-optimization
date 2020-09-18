using System;
using System.Linq;

namespace Optimization
{
  public class LinearFunctionReal : IBindableFunction<double>, IParametricDifferentiableFunction<double>
  {

    public IVector<double> Gradient(IVector<double> parameters, IVector<double> point)
    {
      if (point.Count != parameters.Count - 1)
      {
        throw new System.ArgumentException(nameof(point));
      }

      return new Vector<double>(parameters.Take(parameters.Count - 1));
    }

    public IFunction<double> Bind(IVector<double> parameters)
    {
      Func<IVector<double>, double> f = (IVector<double> point) => this.Value(parameters, point);
      Func<IVector<double>, IVector<double>> df = (IVector<double> point) => this.Gradient(parameters, point);
      return new BaseDifferentiableFunction<double>(f, df);
    }

    public double Value(IVector<double> parameters, IVector<double> point)
    {
      if (point.Count != parameters.Count - 1)
      {
        throw new System.ArgumentException(nameof(point));
      }
      double result = parameters[parameters.Count - 1];
      for (int i = 0; i < point.Count; ++i)
      {
        result += point[i] * parameters[i];
      }
      return result;
    }
  }
}