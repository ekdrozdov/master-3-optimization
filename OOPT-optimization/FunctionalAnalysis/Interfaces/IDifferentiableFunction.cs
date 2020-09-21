using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Interfaces
{
    public interface IDifferentiableFunction<T> : IFunction<T>
    {
        IVector<T> Gradient(IVector<T> point);
    }
}