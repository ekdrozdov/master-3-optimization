namespace Optimization
{
  public interface IBindableFunction<T> : IParametricFunction<T>
  {
    IFunction<T> Bind(IVector<T> parameters);

  }
}