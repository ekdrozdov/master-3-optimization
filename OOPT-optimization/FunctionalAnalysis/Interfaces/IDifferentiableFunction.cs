using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functions
{
  public interface IDifferentiableFunction<T> : IFunction<T>
  {
    IVector<T> Gradient(IVector<T> point);
  }
}