using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Interfaces
{
    interface IDifferentiableFunctional<T> : IFunctional<T>
    {
        IVector<T> Gradient(IDifferentiableFunction<T> f);
    }
}