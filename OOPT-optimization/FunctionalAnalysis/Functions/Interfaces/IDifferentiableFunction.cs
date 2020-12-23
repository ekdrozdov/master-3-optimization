using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces
{
    public interface IDifferentiableFunction<T> : IFunction<T> where T : unmanaged
    {
        IVector<T> Gradient(IVector<T> point);
    }
}