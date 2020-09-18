namespace Optimization
{
  public interface IFunctional<T>
  {
    double Value(IFunction<T> f);
  }
}