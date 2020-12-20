using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functions
{
  public interface IParametricFunction<T>
  {
    IFunction<T> Bind(IVector<T> parameters);
  }
}