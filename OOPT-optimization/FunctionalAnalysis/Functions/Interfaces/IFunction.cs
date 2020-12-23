using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces
{
    public interface IFunction<T> where T : unmanaged
    {
        T Value(IVector<T> point);
    }
}