using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functionals.Interfaces
{
    interface ILeastSquaresFunctional<T> : IFunctional<T> where T : unmanaged
    {
        IVector<T> Residual(IFunction<T> function);

        IMatrix<T> Jacobian(IFunction<T> function);
    }
}