namespace Optimization
{
  public interface IDifferentiableFunction<T> : IFunction<T>
  {
    IVector<T> Gradient(IVector<T> point);
  }
}