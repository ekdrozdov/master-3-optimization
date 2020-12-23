using System;
using System.Collections.Generic;
using System.Linq;
using OOPT.Optimization.Algebra;
using OOPT.Optimization.Algebra.Extensions;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;
using OOPT.Optimization.FunctionalAnalysis.Functionals.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functions;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functionals
{
    public class L2Functional<T> : IDifferentiableFunctional<T>, ILeastSquaresFunctional<T> where T : unmanaged
    {
        private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

        private readonly (IVector<T> point, T target)[] _elements;

        public L2Functional(params (IVector<T>, T)[] points)
        {
            var incoming = points.ToArray();

            if (!incoming.Any())
            {
                throw new ArgumentException("Need at least one item");
            }

            _elements = new (IVector<T>, T)[incoming.LongLength];
            incoming.ToArray().CopyTo(_elements, 0);
        }

        //sum ((f(xi)-realValue(xi))*f')/(sqrt of sum (f(xi)-realValue(xi))^2)
        public IVector<T> Gradient(IFunction<T> f1)
        {
            if (!(f1 is IDifferentiableFunction<T> f))
            {
                throw new ArgumentException("Function must be IDifferentiableFunction");
            }

            var la = LinearAlgebra.Value;

            return _elements.Skip(1).Aggregate(f.Gradient(_elements[0].point)
                                                   .Mult(la.Sub(f.Value(_elements[0].point), _elements[0].target)),
                                               (prev, curr) => prev.Add(
                                                                        f.Gradient(curr.point)
                                                                            .Mult(la.Sub(f.Value(curr.point), curr.target))))
                .Div((Value(f)));
        }

        //sqrt of sum (f(xi)-realValue(xi))^2
        public T Value(IFunction<T> f)
        {
            var la = LinearAlgebra.Value;

            return la.Pow(la.Sum(_elements,
                                 x => la.Pow(la.Sub(f.Value(x.point),
                                                    x.target),
                                             2)),
                          la.Cast(0.5d));
        }

        public IVector<T> Residual(IFunction<T> f1)
        {
            if (!(f1 is IDifferentiableFunction<T> f))
            {
                throw new ArgumentException("Function must be IDifferentiableFunction");
            }

            return new Vector<T>(_elements.Select(x => LinearAlgebra.Value.Sub(f.Value(x.point), x.target)));
        }

        public IMatrix<T> Jacobian(IFunction<T> f1)
        {
            if (!(f1 is IDifferentiableFunction<T> f))
            {
                throw new ArgumentException("Function must be IDifferentiableFunction");
            }

            return new Matrix<T>(_elements.Select(x => f.Gradient(x.point)));
        }
    }
}