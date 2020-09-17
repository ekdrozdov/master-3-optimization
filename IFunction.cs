namespace Optimization
{
  interface IFunction<T>
  {
    double Value(IVector<T> parameters, IVector<T> point);
  }
}