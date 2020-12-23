using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functionals.Interfaces
{
    public interface IFunctional<T> where T : unmanaged
    {
        T Value(IFunction<T> f);
    }
}