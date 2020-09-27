using System;
using System.Collections.Generic;
using System.Linq;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;
using OOPT.Optimization.FunctionalAnalysis.Interfaces;
using OOPT.Optimization.MathAnalysis.Integrates;

namespace OOPT.Optimization.FunctionalAnalysis.Functionals
{
    public class IntegralFunctional<T> : IFunctional<T>
    {
        private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

        private readonly IVector<T>[] _elements;

        private readonly int _order;

        private readonly bool _isCircle;

        public IntegralFunctional(IEnumerable<IVector<T>> points, int order, bool isCircle = false)
        {
            _order = order;
            _isCircle = isCircle;
            var incoming = points.ToArray();

            if (incoming.Length < 2)
            {
                throw new ArgumentException("Need at least two item");
            }

            _elements = new IVector<T>[incoming.LongLength];
            incoming.ToArray().CopyTo(_elements, 0);
        }

        public T Value(IFunction<T> f)
        {
            var sum = LinearAlgebra.Value.GetZeroValue();

            for (var i = 0; i < _elements.Length - 1; i++)
            {
                LinearAlgebra.Value.Add(ref sum, GaussLegendreRule<T>.Integrate(f, _elements[i], _elements[i + 1], _order));
            }

            if (_isCircle)
            {
                LinearAlgebra.Value.Add(ref sum, GaussLegendreRule<T>.Integrate(f, _elements[^1], _elements[0], _order));
            }

            return sum;
        }
    }
}