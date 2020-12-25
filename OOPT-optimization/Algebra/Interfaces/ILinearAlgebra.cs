using System;
using System.Numerics;

namespace OOPT.Optimization.Algebra.Interfaces
{
    public interface ILinearAlgebra<T> where T : unmanaged
    {
        T GetZeroValue();

        T Cast(double a);

        T Cast(Complex a);

        void Add(T[] a, T alpha, ref T[] b);

        void Add(ref T[] a, T alpha);

        void Mult(ref T[] a, T alpha);

        T Abs(T a);

        T Sqrt(T a);
        T Dot(T[] a, T[] b);

        void Sub(T[] a, T alpha, ref T[] b);

        void Sub(ref T[] a, T alpha);

        void Div(ref T[] a, T alpha);

        void Add(ref T a, T b);

        void Sub(ref T a, T b);

        T Sub(T a, T b);

        T Sum(T a, T b);

        T[] Sum(T[] a, T[] b);

        T Sum<TIn>(TIn[] a, Func<TIn, T> f);

        T Div(T a, T b);

        double Exp(T a);
        T Mult(T a, T b);

        T Pow(T a, T power);

        T Pow(T a, int power);

        T Max<TIn>(TIn[] a, Func<TIn, T> f);

        T Sign(T a);

        double DotReal(T[] a, T[] b);

        int Compare(T a, T b);
    }
}