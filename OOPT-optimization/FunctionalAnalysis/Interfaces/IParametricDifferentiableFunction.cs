using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Interfaces
{
    public interface IParametricDifferentiableFunction<T> : IParametricFunction<T>
    {
        IVector<T> Gradient(IVector<T> parameters, IVector<T> point);
    }
}