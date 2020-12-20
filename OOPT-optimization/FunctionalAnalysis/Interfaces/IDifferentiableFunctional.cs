using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functions;

namespace OOPT.Optimization.FunctionalAnalysis.Functionals
{
  interface IDifferentiableFunctional<T> : IFunctional<T>
  {
    IVector<T> Gradient(IDifferentiableFunction<T> f);
  }
}