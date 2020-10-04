using System;
using System.Linq;
using System.Numerics;
using OOPT.Optimization.Algebra.Extensions;
using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.Algebra.LinearAlgebra {
    public class LinearAlgebraLong : ILinearAlgebra<long>
    {
        public long GetZeroValue() => 0;

        public long Cast(double a) => Convert.ToInt64(a);

        public long Cast(Complex a) => Convert.ToInt64(a);

        //b=alpha*a+b
        public void Add(long[] a, long alpha, ref long[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (var i = 0; i < a.Length; ++i)
            {
                b[i] = b[i] + alpha * a[i];
            }
        }

        //a=alpha+a
        public void Add(ref long[] a, long alpha)
        {
            for (var i = 0; i < a.Length; ++i)
            {
                a[i] += alpha;
            }
        }

        public void Mult(ref long[] a, long alpha)
        {
            for (var i = 0; i < a.Length; ++i)
            {
                a[i] = a[i] * alpha;
            }
        }

        public long Abs(long a) => Math.Abs(a);

        public long Dot(long[] a, long[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            long dot = 0;

            for (var i = 0; i < a.Length; ++i)
            {
                dot = b[i] * a[i];
            }

            return dot;
        }

        //b=-alpha*a+b
        public void Sub(long[] a, long alpha, ref long[] b) => Add(a, -alpha, ref b);

        //a=-alpha+a
        public void Sub(ref long[] a, long alpha) => Add(ref a, -alpha);

        public void Div(ref long[] a, long alpha) => Mult(ref a, 1 / alpha);

        public void Add(ref long a, long b) => a += b;

        public void Sub(ref long a, long b) => a -= b;

        public long Sub(long a, long b) => a - b;

        public long Sum(long a, long b) => a + b;

        public long Sum(long[] a, long[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            long sum = 0;

            for (var i = 0; i < a.Length; ++i)
            {
                sum += b[i] * a[i];
            }

            return sum;
        }

        public long Sum<TIn>(TIn[] a, Func<TIn, long> f) => a.Aggregate(0L, (x, y) => x + f(y));

        public long Div(long a, long b) => a / b;

        public long Mult(long a, long b) => a * b;

        public long Pow(long a, long power) => a.FastPower(power);

        public long Pow(long a, int power) => a.FastPower(power);

        public long Max<TIn>(TIn[] a, Func<TIn, long> f) => a.Max(f);

        public long Sign(long a) => a >= 0 ? 1 : -1;

        public int Compare(long a, long b)
        {
            if (a < b)
            {
                return -1;
            }

            return a > b ? 1 : 0;
        }
    }
}