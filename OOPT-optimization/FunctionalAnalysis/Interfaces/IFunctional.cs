using OOPT.Optimization.FunctionalAnalysis.Functions;

namespace OOPT.Optimization.FunctionalAnalysis.Functionals
{
  public interface IFunctional<T>
  {
    T Value(IFunction<T> f);
  }
}