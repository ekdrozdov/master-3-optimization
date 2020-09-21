namespace OOPT.Optimization.FunctionalAnalysis.Interfaces
{
    public interface IFunctional<T>
    {
        T Value(IFunction<T> f);
    }
}