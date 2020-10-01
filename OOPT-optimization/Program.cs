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