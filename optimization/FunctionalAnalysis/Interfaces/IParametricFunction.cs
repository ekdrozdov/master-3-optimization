namespace Optimization
{
  public interface IParametricFunction<T>
  {
    T Value(IVector<T> parameters, IVector<T> point);
  }
}