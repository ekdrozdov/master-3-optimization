using System;
using System.Collections.Generic;
using System.Linq;
using OOPT.Optimization.Algebra.Extensions;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;
using OOPT.Optimization.FunctionalAnalysis.Functionals.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functions;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functionals
{
    public class L1Functional<T> : IDifferentiableFunctional<T> where T : unmanaged
    {
        private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

        private readonly (IVector<T> point, T target)[] _elements;

        public L1Functional(params (IVector<T>, T)[] points)
        {
            var incoming = points.ToArray();

            if (!incoming.Any())
            {
                throw new ArgumentException("Need at least one item");
            }

            _elements = new (IVector<T>, T)[incoming.LongLength];
            incoming.ToArray().CopyTo(_elements, 0);
        }

        //sum of sign(f(xi)-real(xi))*f'
        public IVector<T> Gradient(IFunction<T> f1)
        {
            if (!(f1 is IDifferentiableFunction<T> f))
            {
                throw new ArgumentException("Function must be IDifferentiableFunction");
            }

            var sub = LinearAlgebra.Value
                .Sub(f.Value(_elements[0].point),
                     _elements[0].target);

            var first = f.Gradient(_elements[0].point)
                .Mult(LinearAlgebra.Value.Sign(sub));

            return _elements.Skip(1)
                .Aggregate(first,
                           (prev, curr) => prev.Add(
                                                    f.Gradient(curr.point)
                                                        .Mult(LinearAlgebra.Value.Sign(LinearAlgebra.Value
                                                                                          .Sub(f.Value(curr.point),
                                                                                               curr.target)))));
        }

        //sum of abs(f(xi)-real(xi))
        public T Value(IFunction<T> f)
        {
            return LinearAlgebra.Value.Sum(_elements,
                                           x => LinearAlgebra.Value
                                               .Abs(LinearAlgebra.Value.Sub(f.Value(x.point), x.target)));
        }
    }
}