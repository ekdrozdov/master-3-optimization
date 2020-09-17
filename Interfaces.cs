namespace Optimization
{

  interface IDifferentiableFunction<T> : IFunction<T>
  {
    IVector<T> Gradient(IVector<T> parameters);
  }

  interface IFunctional<T>
  {
    double Value(IFunction<T> function, IVector<T> parameters);
  }

  interface IDifferentiableFunctional<T> : IFunctional<T>
  {
    IVector<T> Gradient(IFunction<T> function, IVector<T> parameters);
  }

  interface ILeastSquaresFunctional<T> : IFunctional<T>
  {
    IVector<T> Residual(IFunction<T> function, IVector<T> parameters);
    IMatrix<T> Jacobian(IFunction<T> function, IVector<T> parameters);
  }

  interface IOptimizator<T>
  {
    IVector<T> Minimize(IFunctional<T> objective,
                     IFunction<T> function,
                     IVector<T> initialParameters,
                     IVector<T> minimumParameters = default,
                     IVector<T> maximumParameters = default);
  }
}