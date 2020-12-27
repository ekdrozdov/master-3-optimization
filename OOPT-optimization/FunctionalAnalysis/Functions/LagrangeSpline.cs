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
    public class LagrangeSpline<T> : IFunction<T> where T : unmanaged
    {
        private readonly int _order;

        private static readonly Lazy<ILinearAlgebra<T>> LinearAlgebra = new Lazy<ILinearAlgebra<T>>(LinearAlgebraFactory.GetLinearAlgebra<T>);

        private static readonly PolynomialFunction<T> Function = new PolynomialFunction<T>();

        private IVector<IVector<T>> Points { get; set; }

        private IVector<T> Parameters { get; set; }

        private IVector<T> Values { get; set; }

        public LagrangeSpline(int order, params (IVector<T> point, T value)[] entity)
            : this(order, entity.Select(x => x.Item1), entity.Select(x => x.Item2)) { }

        public LagrangeSpline(int order, IEnumerable<IVector<T>> points, IEnumerable<T> values)
        {
            _order = order;

            if (_order != 2)
            {
                throw new NotImplementedException("Only quadratic lagrange spline available now");
            }

            if (points.Count() < _order + 1)
            {
                throw new Exception($"Must be at least {_order} points");
            }

            if (points.Count() != values.Count())
            {
                throw new Exception("Points and values has different dimension");
            }

            var firstCount = points.First().Count;

            if (points.Any(x => x.Count != firstCount))
            {
                throw new Exception("Not all piece has same dimension");
            }

            if (points.First().Count != 1)
            {
                throw new NotImplementedException("Only zero dimension lagrange spline available now");
            }

            var la = LinearAlgebra.Value;

            var sortedList = points.Zip(values).ToList();

            sortedList.Sort((x, y) =>
            {
                if (x.First.LessThan(y.First))
                {
                    return -1;
                }

                if (x.First.MoreThan(y.First))
                {
                    return 1;
                }

                return 0;
            });

            Points = new Vector<IVector<T>>(sortedList.Select(x => x.First));
            Values = new Vector<T>(sortedList.Select(x => x.Second));
            Parameters = new Vector<T>((Points.Count - _order) * (_order + 1));

            for (int i = 0, j = 0; i < (Points.Count - _order) * (_order + 1); i += (_order + 1), j++)
            {
                Parameters[i] = la.Sub(la.Div(la.Sub(Values[j + 2], Values[j]), la.Mult(la.Sub(Points[j + 2][0], Points[j][0]), la.Sub(Points[j + 2][0], Points[j + 1][0]))),
                                       la.Div(la.Sub(Values[j + 1], Values[j]), la.Mult(la.Sub(Points[j + 1][0], Points[j][0]), la.Sub(Points[j + 2][0], Points[j + 1][0]))));

                Parameters[i + 1] = la.Sub(la.Div(la.Sub(Values[j + 1], Values[j]), la.Sub(Points[j + 1][0], Points[j][0])),
                                           la.Mult(Parameters[i], la.Sum(Points[j + 1][0], Points[j][0])));

                Parameters[i + 2] = la.Sub(Values[j + 2], la.Sum(la.Mult(Parameters[i + 1], Points[j + 2][0]), la.Mult(Parameters[i], la.Pow(Points[j + 2][0], 2))));
            }
        }

        public T Value(IVector<T> point)
        {
            if (point.Count != Points.First().Count)
            {
                throw new ArgumentException(nameof(point));
            }

            var intersection = SearchIntersection(point);

            if (intersection == -1)
            {
                return Function.Value(new Vector<T>(Parameters.Take(_order + 1)), point);
            }

            if (intersection == Points.Count)
            {
                return Function.Value(new Vector<T>(Parameters.TakeLast(_order + 1)), point);
            }

            return Function.Value(new Vector<T>(Parameters.Skip(intersection * (_order + 1)).Take(_order + 1)), point);
        }

        private int SearchIntersection(IVector<T> point)
        {
            if (point.Count != (Points.FirstOrDefault()?.Count ?? 0))
            {
                throw new Exception("Point has different dimension than pieces");
            }

            if (point.LessOrEqualThan(Points[0]))
            {
                return -1;
            }

            if (point.MoreOrEqualThan(Points[^1]))
            {
                return Points.Count;
            }

            for (int i = 0; i < Points.Count - 2; i++)
            {
                if (point.Intersect(Points[i], Points[i + 2]))
                {
                    return i + 1;
                }
            }

            //never
            throw new Exception("Intersection not found. Error in Points");
        }
    }
}