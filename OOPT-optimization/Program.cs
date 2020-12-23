using System;
using OOPT.Optimization.Algebra;
using OOPT.Optimization.Algebra.DiophantineEquations;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.FunctionalAnalysis.Functionals;
using OOPT.Optimization.FunctionalAnalysis.Functions;
using OOPT.Optimization.FunctionalAnalysis.Functions.Interfaces;
using OOPT.Optimization.OptimizationMethods;

namespace OOPT.Optimization
{
    class Program
    {
        static void Main(string[] args)
        {
            var point1 = new Vector<double>(new[] { 0d });
            var point2 = new Vector<double>(new[] { 2d });
            var point3 = new Vector<double>(new[] { 3d });
            var point4 = new Vector<double>(new[] { 1d });

            var functional = new L2Functional<double>(new[] { ((IVector<double>)point1, 1d), ((IVector<double>)point2, 9d), ((IVector<double>)point3, 16d), ((IVector<double>)point4, 4d) });
            var optimizer = new GaussNewton<double>(1000, 1e-14);
           // var optimizer = new ImitateAnnealing<double>(10000000, 1e-4); 121

            var value = optimizer.Minimize(functional, new PolynomialFunction<double>(), new Vector<double>(new[] { 1d, 0d, 1d }));
            var f = new PolynomialFunction<double>().Bind(new Vector<double>(new[] { 1d, 2d, 1d }));
            var r = ((IDifferentiableFunction<double>) f).Gradient(point2);
            var c=functional.Value(f);
            var grad = functional.Gradient((IDifferentiableFunction<double>)f);
            //var r = functional.Value(new PolynomialFunction<double>().Bind(value));
            //Console.WriteLine(r);
            var result = false;
            var de = DiophantineEquation.ReadFromConsole();

            try
            {
                result = de.Solve();
            }
            catch (Exception e)
            {
                Console.WriteLine("NO");
                Console.WriteLine(e.Message);

                return;
            }

            if (result)
            {
                de.WriteSolutionIntoConsoleMachineReadable();
            }
            else
            {
                Console.WriteLine("NO");
            }
        }
    }
}