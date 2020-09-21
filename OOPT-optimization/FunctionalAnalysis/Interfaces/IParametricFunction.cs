using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Interfaces
{
    public interface IParametricFunction<T>
    {
        T Value(IVector<T> parameters, IVector<T> point);
    }
}