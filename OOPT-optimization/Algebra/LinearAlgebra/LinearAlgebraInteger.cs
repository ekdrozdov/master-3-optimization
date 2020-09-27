using System;
using System.Linq;
using System.Numerics;
using OOPT.Optimization.Algebra.Extensions;
using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.Algebra.LinearAlgebra
{
    public class LinearAlgebraInteger : ILinearAlgebra<int>
    {
        public int GetZeroValue() => 0;

        public int Cast(double a) => Convert.ToInt32(a);

        public int Cast(Complex a) => Convert.ToInt32(a);

        //b=alpha*a+b
        public void Add(int[] a, int alpha, ref int[] b)
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
        public void Add(ref int[] a, int alpha)
        {
            for (var i = 0; i < a.Length; ++i)
            {
                a[i] += alpha;
            }
        }

        public void Mult(ref int[] a, int alpha)
        {
            for (var i = 0; i < a.Length; ++i)
            {
                a[i] = a[i] * alpha;
            }
        }

        public int Abs(int a) => Math.Abs(a);

        public int Dot(int[] a, int[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            int dot = 0;

            for (var i = 0; i < a.Length; ++i)
            {
                dot = b[i] * a[i];
            }

            return dot;
        }

        //b=-alpha*a+b
        public void Sub(int[] a, int alpha, ref int[] b) => Add(a, -alpha, ref b);

        //a=-alpha+a
        public void Sub(ref int[] a, int alpha) => Add(ref a, -alpha);

        public void Div(ref int[] a, int alpha) => Mult(ref a, 1 / alpha);

        public void Add(ref int a, int b) => a += b;

        public void Sub(ref int a, int b) => a -= b;

        public int Sub(int a, int b) => a - b;

        public int Sum(int a, int b) => a + b;

        public int Sum(int[] a, int[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            int sum = 0;

            for (var i = 0; i < a.Length; ++i)
            {
                sum += b[i] * a[i];
            }

            return sum;
        }

        public int Sum<TIn>(TIn[] a, Func<TIn, int> f) => a.Aggregate(0, (x, y) => x + f(y));

        public int Div(int a, int b) => a / b;

        public int Mult(int a, int b) => a * b;

        public int Pow(int a, int power) => a.FastPower(power);

        public int Max<TIn>(TIn[] a, Func<TIn, int> f) => a.Max(f);

        public int Sign(int a) => a >= 0 ? 1 : -1;

        public int Compare(int a, int b)
        {
            if (a < b)
            {
                return -1;
            }

            return a > b ? 1 : 0;
        }
    }
}