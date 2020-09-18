namespace Optimization
{
  public class BaseDifferentiableFunction<T> : BaseFunction<T>, IDifferentiableFunction<T>
  {
    protected System.Func<IVector<T>, IVector<T>> df;

    public BaseDifferentiableFunction(System.Func<IVector<T>, T> f, System.Func<IVector<T>, IVector<T>> df) : base(f)
    {
      this.df = df;
    }
    public IVector<T> Gradient(IVector<T> point)
    {
      return df(point);
    }
  }
}