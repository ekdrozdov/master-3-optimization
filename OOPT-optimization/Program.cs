using System;
using OOPT.Optimization.Algebra;
using OOPT.Optimization.Algebra.DiophantineEquations;
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
            var de = DiophantineEquation.ReadFromConsole();
            de.Solve();
            de.WriteSolutionIntoConsoleMachineReadable();
        }
    }
}