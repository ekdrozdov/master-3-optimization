using System;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            var point1 = new Vector<double>(0d);
            var point2 = new Vector<double>(2d);
            var point3 = new Vector<double>(3d);
            var point4 = new Vector<double>(1d);

            var functional = new L2Functional<double>((point1, 1d), (point2, 9d), (point3, 16d), (point4, 4d));
            var optimizer = new GaussNewton<double>(1000, 1e-14);
            var value = optimizer.Minimize(functional, new PolynomialFunction<double>(), new Vector<double>(new[] { 1d, 0d, 1d }));
            Console.WriteLine(JsonSerializer.Serialize(value));
        }
    }
}