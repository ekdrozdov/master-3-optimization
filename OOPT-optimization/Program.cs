using System;
using OOPT.Optimization.Algebra;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.FunctionalAnalysis;
using OOPT.Optimization.FunctionalAnalysis.Functionals;
using OOPT.Optimization.FunctionalAnalysis.Interfaces;

namespace OOPT.Optimization
{
    class Program
    {
        static void Main(string[] args)
        {
            var point1 = new Vector<double>(new[] { 1d });
            var point2 = new Vector<double>(new[] { 2d });
            var functional = new IntegralFunctional<double>(new[] { point1, point2 }, 4);
            var f = new LinearFunction<double>().Bind(new Vector<double>(new[] { 10d, 0d }));
            var r = functional.Value(f);
            //    var r1 = functional.Gradient((IDifferentiableFunction<double>)f);
            Console.WriteLine(r);
            /*
                        foreach (var d in r1)
                        {
                            Console.WriteLine(d);
                        }*/
        }
    }
}