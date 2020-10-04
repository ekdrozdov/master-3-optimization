using System;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;

namespace OOPT.Optimization.Algebra.Extensions
{
    public static class VectorExtension
    {
        public static (T minimum, long index) MinWithIndex<T>(this IVector<T> self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            if (self.Count == 0)
            {
                throw new ArgumentException("List is empty.", nameof(self));
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            var min = self[0];
            var minIndex = 0;

            for (var i = 1; i < self.Count; ++i)
            {
                if (la.Compare(self[i], min) >= 0)
                {
                    continue;
                }

                min = self[i];
                minIndex = i;
            }

            return (min, minIndex);
        }

        public static IVector<T> Add<T>(this IVector<T> a, IVector<T> b)
        {
            if (a.Count != b.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            for (var i = 0; i < a.Count; i++)
            {
                a[i] = la.Sum(a[i], b[i]);
            }

            return a;
        }

        public static IVector<T> Mult<T>(this IVector<T> a, T b)
        {
            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            for (var i = 0; i < a.Count; i++)
            {
                a[i] = la.Mult(a[i], b);
            }

            return a;
        }

        public static IVector<T> Div<T>(this IVector<T> a, T b)
        {
            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            for (var i = 0; i < a.Count; i++)
            {
                a[i] = la.Div(a[i], b);
            }

            return a;
        }

        public static IVector<T> SetValue<T>(this IVector<T> a, T b)
        {
            for (var i = 0; i < a.Count; i++)
            {
                a[i] = b;
            }

            return a;
        }
    }
}