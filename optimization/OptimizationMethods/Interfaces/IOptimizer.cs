namespace Optimization
{

  interface IOptimizer<T>
  {
    IVector<T> Minimize(IFunctional<T> objective,
                     IFunction<T> function,
                     IVector<T> initialParameters,
                     IVector<T> minimumParameters = default,
                     IVector<T> maximumParameters = default);
  }
}