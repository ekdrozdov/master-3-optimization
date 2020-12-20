using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functionals;
using OOPT.Optimization.FunctionalAnalysis.Functions;

namespace OOPT.Optimization.OptimizationMethods.Interfaces
{
  interface IOptimizer<T>
  {
    IVector<T> Minimize(IFunctional<T> objective,
        IFunction<T> function,
        IVector<T> initialParameters,
        IVector<T> minimumParameters = default,
        IVector<T> maximumParameters = default);
  }
}