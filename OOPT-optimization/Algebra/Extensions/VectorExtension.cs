using System;
using OOPT.Optimization.Algebra.LinearAlgebra;

namespace OOPT.Optimization.Algebra.Interfaces {
    public static class VectorExtension
    {
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
    }
}