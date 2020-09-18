namespace Optimization
{
  public class BaseFunction<T> : IFunction<T>
  {
    protected System.Func<IVector<T>, T> f;
    public BaseFunction(System.Func<IVector<T>, T> f)
    {
      this.f = f;
    }

    public T Value(IVector<T> point)
    {
      return f(point);
    }
  }
}