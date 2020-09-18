namespace Optimization
{
  public interface IFunction<T>
  {
    T Value(IVector<T> point);
  }
}