using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functionals.Interfaces
{
    public interface IDifferentiableFunctional<T> : IFunctional<T> where T : unmanaged
    {
        IVector<T> Gradient(IFunction<T> f);
    }
}