using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functionals.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;

namespace OOPT.Optimization.OptimizationMethods.Interfaces
{
    public interface IOptimizer<T> where T : unmanaged
    {
        IVector<T> Minimize(IFunctional<T> objective,
            IParametricFunction<T> function,
            IVector<T> initialParameters,
            IVector<T> minimumParameters = default,
            IVector<T> maximumParameters = default);
    }
}