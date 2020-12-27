using System;
using System.Collections.Generic;
using System.Linq;
using OOPT.Optimization.Algebra;
using OOPT.Optimization.Algebra.Extensions;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;

namespace OOPT.Optimization.FunctionalAnalysis.Functions
{
    public class PiecewiseLinearFunction<T> : IParametricFunction<T> where T : unmanaged
    {
        private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

        private static readonly LinearFunction<T> Function = new LinearFunction<T>();

        private IVector<IVector<T>> Points { get; set; }

        //parameters.Size = (Points.Count +1) * LinearFunction.Size;
        public PiecewiseLinearFunction(params IVector<T>[] points) : this(points.ToList()) { }

        public PiecewiseLinearFunction(IEnumerable<IVector<T>> points)
        {
            if (!points.Any())
            {
                throw new Exception("Must be at least one points");
            }

            var firstCount = points.First().Count;

            if (points.Any(x => x.Count != firstCount))
            {
                throw new Exception("Not all piece has same dimension");
            }

            var sortedList = points.ToList();

            sortedList.Sort((x, y) =>
            {
                if (x.LessThan(y))
                {
                    return -1;
                }

                if (x.MoreThan(y))
                {
                    return 1;
                }

                return 0;
            });

            Points = new Vector<IVector<T>>(sortedList);
        }

        public IVector<T> Gradient(IVector<T> parameters, IVector<T> point)
        {
            if (((point.Count + 1) * (Points.Count + 1)) != parameters.Count)
            {
                throw new ArgumentException(nameof(point));
            }

            var intersection = SearchIntersection(point);
            var gradient = new Vector<T>(parameters.Count, LinearAlgebra.Value.GetZeroValue());

            if (intersection == -1)
            {
                var grad = Function.Gradient(new Vector<T>(parameters.Take(point.Count + 1)), point);

                for (int i = 0; i < point.Count + 1; i++)
                {
                    gradient[i] = grad[i] is ICloneable t ? (T) t.Clone() : grad[i];
                }

                return gradient;
            }

            if (intersection == Points.Count)
            {
                var grad = Function.Gradient(new Vector<T>(parameters.TakeLast(point.Count + 1)), point);

                for (int i = parameters.Count - (point.Count + 1), j = 0; j < point.Count + 1; i++, j++)
                {
                    gradient[i] = grad[j] is ICloneable t ? (T) t.Clone() : grad[j];
                }

                return gradient;
            }

            var grad1 = Function.Gradient(new Vector<T>(parameters.Skip((point.Count + 1) * (intersection + 1)).Take(point.Count + 1)), point);

            for (int i = (point.Count + 1) * (intersection + 1), j = 0; j < point.Count + 1; i++, j++)
            {
                gradient[i] = grad1[j] is ICloneable t ? (T) t.Clone() : grad1[j];
            }

            return gradient;
        }

        public T Value(IVector<T> parameters, IVector<T> point)
        {
            if (((point.Count + 1) * (Points.Count + 1)) != parameters.Count)
            {
                throw new ArgumentException(nameof(point));
            }

            var intersection = SearchIntersection(point);

            if (intersection == -1)
            {
                return Function.Value(new Vector<T>(parameters.Take(point.Count + 1)), point);
            }

            if (intersection == Points.Count)
            {
                return Function.Value(new Vector<T>(parameters.TakeLast(point.Count + 1)), point);
            }

            return Function.Value(new Vector<T>(parameters.Skip((point.Count + 1) * (intersection + 1)).Take(point.Count + 1)), point);
        }

        public IFunction<T> Bind(IVector<T> parameters) =>
            new BaseDifferentiableFunction<T>(point => Value(parameters, point),
                                              point => Gradient(parameters, point));

        private int SearchIntersection(IVector<T> point)
        {
            if (point.Count != (Points.FirstOrDefault()?.Count ?? 0))
            {
                throw new Exception("Point has different dimension than pieces");
            }

            if (point.LessThan(Points[0]))
            {
                return -1;
            }

            if (point.MoreOrEqualThan(Points[^1]))
            {
                return Points.Count;
            }

            for (int i = 0; i < Points.Count - 1; i++)
            {
                if (point.LeftIntersect(Points[i], Points[i + 1]))
                {
                    return i;
                }
            }

            //never
            throw new Exception("Intersection not found. Error in Points");
        }
    }
}