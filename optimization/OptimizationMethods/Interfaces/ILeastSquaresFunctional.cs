namespace Optimization
{
  interface ILeastSquaresFunctional<T> : IFunctional<T>
  {
    IVector<T> Residual(IFunction<T> function, IVector<T> parameters);
    IMatrix<T> Jacobian(IFunction<T> function, IVector<T> parameters);
  }
}