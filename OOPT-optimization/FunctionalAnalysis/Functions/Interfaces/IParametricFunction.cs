using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces
{
    public interface IParametricFunction<T> where T : unmanaged
    {
        IFunction<T> Bind(IVector<T> parameters);
    }
}