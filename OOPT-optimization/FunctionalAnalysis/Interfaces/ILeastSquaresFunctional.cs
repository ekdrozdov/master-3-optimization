using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Interfaces
{
    interface ILeastSquaresFunctional<T> : IFunctional<T>
    {
        IVector<T> Residual(IDifferentiableFunction<T> function);

        IMatrix<T> Jacobian(IDifferentiableFunction<T> function);
    }
}