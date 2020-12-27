using System;
using System.Linq;
using OOPT.Optimization.Algebra;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functions
{
    /// <summary>
    /// Function realize ax^p+bx^(p-1)+cx^(p-2)....+d
    /// </summary>
    public class PolynomialFunction<T> : IParametricFunction<T> where T : unmanaged
    {
        private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

        public T Value(IVector<T> parameters, IVector<T> point)
        {
            if (point.Count != 1)
            {
                throw new ArgumentException("Point dimension must be 1", nameof(point));
            }

            var la = LinearAlgebra.Value;
            var sum = la.GetZeroValue();

            for (var i = 0; i < parameters.Count; i++)
            {
                la.Add(ref sum, la.Mult(parameters[i], la.Pow(point[0], parameters.Count - i - 1)));
            }

            return sum;
        }

        public IVector<T> Gradient(IVector<T> parameters, IVector<T> point)
        {
            if (point.Count != 1)
            {
                throw new ArgumentException("Point dimension must be 1", nameof(point));
            }

            var la = LinearAlgebra.Value;

            return new Vector<T>(parameters.Select((x, i) => la.Pow(point[0], parameters.Count - 1 - i)));
        }

        public IFunction<T> Bind(IVector<T> parameters)
            => new BaseDifferentiableFunction<T>(point => Value(parameters, point), point => Gradient(parameters, point));
    }
}