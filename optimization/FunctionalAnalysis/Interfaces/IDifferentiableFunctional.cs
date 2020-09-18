namespace Optimization
{
  interface IDifferentiableFunctional<T> : IFunctional<T>
  {
    IVector<T> Gradient(IFunction<T> function);
  }
}