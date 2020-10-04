using System;
using System.Linq;
using System.Numerics;
using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.Algebra.LinearAlgebra
{
    public class LinearAlgebraDouble : ILinearAlgebra<double>
    {
        public double GetZeroValue() => 0d;

        public double Cast(double a) => a;

        public double Cast(Complex a) => throw new InvalidCastException();

        //b=alpha*a+b
        public void Add(double[] a, double alpha, ref double[] b)
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
        public void Add(ref double[] a, double alpha)
        {
            for (var i = 0; i < a.Length; ++i)
            {
                a[i] += alpha;
            }
        }

        public void Mult(ref double[] a, double alpha)
        {
            for (var i = 0; i < a.Length; ++i)
            {
                a[i] = a[i] * alpha;
            }
        }

        public double Abs(double a) => Math.Abs(a);

        public double Sum(double[] a, double[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            double sum = 0;

            for (var i = 0; i < a.Length; ++i)
            {
                sum = b[i] * a[i];
            }

            return sum;
        }

        public double Sum<TIn>(TIn[] a, Func<TIn, double> f) => a.Aggregate(0d, (x, y) => x + f(y));

        public double Dot(double[] a, double[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            return a.Select((t, i) => b[i] * t).Sum();
        }

        //b=-alpha*a+b
        public void Sub(double[] a, double alpha, ref double[] b) => Add(a, -alpha, ref b);

        //a=-alpha+a
        public void Sub(ref double[] a, double alpha) => Add(ref a, -alpha);

        public void Div(ref double[] a, double alpha) => Mult(ref a, 1 / alpha);

        public void Add(ref double a, double b) => a += b;

        public void Sub(ref double a, double b) => a -= b;

        public double Sub(double a, double b) => a - b;

        public double Sum(double a, double b) => a + b;

        public double Div(double a, double b) => a / b;

        public double Mult(double a, double b) => a * b;

        public double Pow(double a, double power) => Math.Pow(a, power);

        public double Pow(double a, int power) => Math.Pow(a, power);

        public double Max<TIn>(TIn[] a, Func<TIn, double> f) => a.Max(f);

        public double Sign(double a) => a >= 0 ? 1 : -1;
        public int Compare(double a, double b)
        {
            if (a < b)
            {
                return -1;
            }

            return a > b ? 1 : 0;
        }
    }
}