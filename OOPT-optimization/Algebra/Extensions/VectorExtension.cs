using System;
using System.Linq;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;

namespace OOPT.Optimization.Algebra.Extensions
{
    public static class VectorExtension
    {
        public static (T minimum, long index) MinWithIndex<T>(this IVector<T> self) where T : unmanaged
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

        public static IVector<T> Add<T>(this IVector<T> a, IVector<T> b) where T : unmanaged
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

        public static IVector<T> Sub<T>(this IVector<T> a, IVector<T> b) where T : unmanaged
        {
            if (a.Count != b.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            for (var i = 0; i < a.Count; i++)
            {
                a[i] = la.Sub(a[i], b[i]);
            }

            return a;
        }

        public static IVector<T> AddWithCloning<T>(this IVector<T> a, IVector<T> b) where T : unmanaged
        {
            return (a.Clone() as IVector<T>).Add(b);
        }

        public static IVector<T> SubWithCloning<T>(this IVector<T> a, IVector<T> b) where T : unmanaged
        {
            return (a.Clone() as IVector<T>).Sub(b);
        }

        public static IVector<T> Mult<T>(this IVector<T> a, T b) where T : unmanaged
        {
            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            for (var i = 0; i < a.Count; i++)
            {
                a[i] = la.Mult(a[i], b);
            }

            return a;
        }

        public static IVector<T> MultWithCloning<T>(this IVector<T> a, T b) where T : unmanaged
        {
            return (a.Clone() as IVector<T>).Mult(b);
        }

        public static IVector<T> Div<T>(this IVector<T> a, T b) where T : unmanaged
        {
            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            for (var i = 0; i < a.Count; i++)
            {
                a[i] = la.Div(a[i], b);
            }

            return a;
        }

        public static IVector<T> SetValue<T>(this IVector<T> a, T b) where T : unmanaged
        {
            for (var i = 0; i < a.Count; i++)
            {
                a[i] = b;
            }

            return a;
        }
        public static bool MoreOrEqualThan<T>(this IVector<T> source, IVector<T> a) where T : unmanaged
        {
            if (source.Count != a.Count)
            {
                throw new Exception("IVectors with different dimension");
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            if (source.Where((t, i) => la.Compare(t, a[i]) == -1).Any())
            {
                return false;
            }

            return true;
        }
        public static bool LessOrEqualThan<T>(this IVector<T> source, IVector<T> a) where T : unmanaged
        {
            if (source.Count != a.Count)
            {
                throw new Exception("IVectors with different dimension");
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            if (source.Where((t, i) => la.Compare(t, a[i]) == 1).Any())
            {
                return false;
            }

            return true;
        }
        public static bool LessThan<T>(this IVector<T> source, IVector<T> a) where T : unmanaged
        {
            if (source.Count != a.Count)
            {
                throw new Exception("IVectors with different dimension");
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            if (source.Where((t, i) => la.Compare(t, a[i]) != -1).Any())
            {
                return false;
            }

            return true;
        }
        public static bool MoreThan<T>(this IVector<T> source, IVector<T> a) where T : unmanaged
        {
            if (source.Count != a.Count)
            {
                throw new Exception("IVectors with different dimension");
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            if (source.Where((t, i) => la.Compare(t, a[i]) != 1).Any())
            {
                return false;
            }

            return true;
        }
        public static bool LeftIntersect<T>(this IVector<T> source, IVector<T> left, IVector<T> right) where T : unmanaged
        {
            if (source.Count != left.Count || left.Count != right.Count || source.Count != right.Count)
            {
                throw new Exception("IVectors with different dimension");
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            if (source.Where((t, i) => la.Compare(t, left[i]) == -1).Any())
            {
                return false;
            }

            if (source.Where((t, i) => la.Compare(t, right[i]) != -1).Any())
            {
                return false;
            }

            return true;
        }

        public static bool RightIntersect<T>(this IVector<T> source, IVector<T> left, IVector<T> right) where T : unmanaged
        {
            if (source.Count != left.Count || left.Count != right.Count || source.Count != right.Count)
            {
                throw new Exception("IVectors with different dimension");
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            if (source.Where((t, i) => la.Compare(t, left[i]) != 1).Any())
            {
                return false;
            }

            if (source.Where((t, i) => la.Compare(t, right[i]) == 1).Any())
            {
                return false;
            }

            return true;
        }

        public static bool Intersect<T>(this IVector<T> source, IVector<T> left, IVector<T> right) where T : unmanaged
        {
            if (source.Count != left.Count || left.Count != right.Count || source.Count != right.Count)
            {
                throw new Exception("IVectors with different dimension");
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            if (source.Where((t, i) => la.Compare(t, left[i]) == -1).Any())
            {
                return false;
            }

            if (source.Where((t, i) => la.Compare(t, right[i]) == 1).Any())
            {
                return false;
            }

            return true;
        }
    }
}