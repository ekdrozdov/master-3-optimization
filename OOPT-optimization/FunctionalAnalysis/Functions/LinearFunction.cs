using OOPT.Optimization.Algebra;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;
using System;
using System.Drawing;
using System.Linq;

namespace OOPT.Optimization.FunctionalAnalysis.Functions
{
    /// <summary>
    /// Function realize ax+by+cz....+d
    /// </summary>
    public class LinearFunction<T> : IParametricFunction<T> where T : unmanaged
    {
        private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

        public IVector<T> Gradient(IVector<T> parameters, IVector<T> point)
        {
            if (point.Count != parameters.Count - 1)
            {
                throw new ArgumentException(nameof(point));
            }

            return new Vector<T>(parameters.Select((x, i) => i == parameters.Count - 1 ? LinearAlgebra.Value.Cast(1) : point[i]));
        }

        public IFunction<T> Bind(IVector<T> parameters) =>
            new BaseDifferentiableFunction<T>(point => Value(parameters, point),
                                              point => Gradient(parameters, point));

        public T Value(IVector<T> parameters, IVector<T> point)
        {
            if (point.Count != parameters.Count - 1)
            {
                throw new ArgumentException(nameof(point));
            }

            return LinearAlgebra.Value.Sum(parameters[^1],
                                           LinearAlgebra.Value.Dot(point.ToArray(), parameters.Take(parameters.Count - 1).ToArray()));
        }
    }
}