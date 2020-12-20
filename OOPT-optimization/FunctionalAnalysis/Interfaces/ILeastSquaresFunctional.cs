using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functions;

namespace OOPT.Optimization.FunctionalAnalysis.Functionals
{
  interface ILeastSquaresFunctional<T> : IFunctional<T>
  {
    IVector<T> Residual(IDifferentiableFunction<T> function);

    IMatrix<T> Jacobian(IDifferentiableFunction<T> function);
  }
}