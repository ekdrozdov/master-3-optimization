using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functions
{
  public interface IFunction<T>
  {
    T Value(IVector<T> point);
  }
}