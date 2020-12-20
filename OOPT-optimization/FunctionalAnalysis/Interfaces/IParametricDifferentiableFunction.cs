using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functions
{
  public interface IParametricDifferentiableFunction<T> : IParametricFunction<T>
  {
    IVector<T> Gradient(IVector<T> parameters, IVector<T> point);
  }
}