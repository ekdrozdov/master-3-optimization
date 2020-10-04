using System;
using System.Linq;
using System.Numerics;
using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.Algebra.LinearAlgebra
{
    public class LinearAlgebraComplex : ILinearAlgebra<Complex>
    {
        public Complex GetZeroValue() => Complex.Zero;

        public Complex Cast(double a) => a;

        public Complex Cast(Complex a) => a;

        //b=alpha*a+b
        public void Add(Complex[] a, Complex alpha, ref Complex[] b)
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
        public void Add(ref Complex[] a, Complex alpha)
        {
            for (var i = 0; i < a.Length; ++i)
            {
                a[i] += alpha;
            }
        }

        public void Mult(ref Complex[] a, Complex alpha)
        {
            for (var i = 0; i < a.Length; ++i)
            {
                a[i] = a[i] * alpha;
            }
        }

        public Complex Abs(Complex a) => Complex.Abs(a);

        public Complex Dot(Complex[] a, Complex[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            Complex dot = 0;

            for (var i = 0; i < a.Length; ++i)
            {
                dot = b[i] * a[i];
            }

            return dot;
        }

        //b=-alpha*a+b
        public void Sub(Complex[] a, Complex alpha, ref Complex[] b) => Add(a, -alpha, ref b);

        //a=-alpha+a
        public void Sub(ref Complex[] a, Complex alpha) => Add(ref a, -alpha);

        public void Div(ref Complex[] a, Complex alpha) => Mult(ref a, 1 / alpha);

        public void Add(ref Complex a, Complex b) => a += b;

        public void Sub(ref Complex a, Complex b) => a -= b;

        public Complex Sub(Complex a, Complex b) => a - b;

        public Complex Sum(Complex a, Complex b) => a + b;

        public Complex Sum(Complex[] a, Complex[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            Complex sum = 0;

            for (var i = 0; i < a.Length; ++i)
            {
                sum += b[i] * a[i];
            }

            return sum;
        }

        public Complex Sum<TIn>(TIn[] a, Func<TIn, Complex> f) => a.Aggregate(Complex.Zero, (x, y) => x + f(y));

        public Complex Div(Complex a, Complex b) => a / b;

        public Complex Mult(Complex a, Complex b) => a * b;

        public Complex Pow(Complex a, Complex power) => Complex.Pow(a, power);

        public Complex Pow(Complex a, int power) => Complex.Pow(a, power);

        public Complex Max<TIn>(TIn[] a, Func<TIn, Complex> f) => a.Max(f);

        public Complex Sign(Complex a) => a.Real >= 0 ? 1 : -1;

        public int Compare(Complex a, Complex b)
        {
            if (a.Magnitude < b.Magnitude)
            {
                return -1;
            }

            return a.Magnitude > b.Magnitude ? 1 : 0;
        }
    }
}