namespace Optimization
{
  public interface IParametricDifferentiableFunction<T> : IParametricFunction<T>
  {
    IVector<T> Gradient(IVector<T> parameters, IVector<T> point);
  }
}