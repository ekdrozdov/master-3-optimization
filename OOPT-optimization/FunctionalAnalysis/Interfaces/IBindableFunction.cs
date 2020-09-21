using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Interfaces
{
    public interface IBindableFunction<T> : IParametricFunction<T>
    {
        IFunction<T> Bind(IVector<T> parameters);
    }
}