using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Interfaces
{
    public interface IFunction<T>
    {
        T Value(IVector<T> point);
    }
}