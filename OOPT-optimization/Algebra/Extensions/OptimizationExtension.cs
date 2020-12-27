using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.OptimizationMethods.Interfaces;

namespace OOPT.Optimization.Algebra.Extensions {
    public static class OptimizationExtension
    {
        public static void ApplyMinimumAndMaximumValues<T>(this IOptimizer<T> o, IVector<T> minimumParameters, IVector<T> maximumParameters, IVector<T> xNew, ILinearAlgebra<T> la) where T : unmanaged
        {
            if (!(maximumParameters is null))
            {
                for (int i = 0; i < xNew.Count; i++)
                {
                    if (la.Compare(xNew[i], maximumParameters[i]) == -1)
                    {
                        xNew[i] = maximumParameters[i];
                    }
                }
            }

            if (!(minimumParameters is null))
            {
                for (int i = 0; i < xNew.Count; i++)
                {
                    if (la.Compare(xNew[i], minimumParameters[i]) == 1)
                    {
                        xNew[i] = minimumParameters[i];
                    }
                }
            }
        }
    }
}