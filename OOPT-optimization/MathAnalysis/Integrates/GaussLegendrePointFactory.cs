using System;
using OOPT.Optimization.Algebra.LinearAlgebra;

namespace OOPT.Optimization.MathAnalysis.Integrates
{
    /* Some copy paste from MathNet.Numerics.Integration.*/
    public static class GaussLegendrePointFactory
    {
        [ThreadStatic] private static GaussPoint _gaussLegendrePoint;

        /// <summary>Getter for the GaussPoint.</summary>
        /// <param name="order">Defines an Nth order Gauss-Legendre rule. Precomputed Gauss-Legendre abscissas/weights for orders 2-20, 32, 64, 96, 100, 128, 256, 512, 1024 are used, otherwise they're calculated on the fly.</param>
        /// <returns>Object containing the non-negative abscissas/weights, order, and intervalBegin/intervalEnd. The non-negative abscissas/weights are generated over the interval [-1,1] for the given order.</returns>
        public static GaussPoint GetGaussPoint(int order)
        {
            if ((_gaussLegendrePoint == null ? 0 : _gaussLegendrePoint.Order == order ? 1 : 0) != 0)
            {
                return _gaussLegendrePoint;
            }

            if (!GaussLegendrePoints.PreComputed.TryGetValue(order, out _gaussLegendrePoint))
            {
                _gaussLegendrePoint = GaussLegendrePoints.Generate(order, 1E-10);
            }

            return _gaussLegendrePoint;
        }

        /// <summary>Getter for the GaussPoint.</summary>
        /// <param name="intervalBegin">Where the interval starts, inclusive and finite.</param>
        /// <param name="intervalEnd">Where the interval stops, inclusive and finite.</param>
        /// <param name="order">Defines an Nth order Gauss-Legendre rule. Precomputed Gauss-Legendre abscissas/weights for orders 2-20, 32, 64, 96, 100, 128, 256, 512, 1024 are used, otherwise they're calculated on the fly.</param>
        /// <returns>Object containing the abscissas/weights, order, and intervalBegin/intervalEnd.</returns>
        public static GaussPoint<T> GetGaussPoint<T>(
            T intervalBegin,
            T intervalEnd,
            int order) where T : unmanaged
        {
            return Map(intervalBegin, intervalEnd, GetGaussPoint(order));
        }

        /// <summary>
        /// Maps the non-negative abscissas/weights from the interval [-1, 1] to the interval [intervalBegin, intervalEnd].
        /// </summary>
        /// <param name="intervalBegin">Where the interval starts, inclusive and finite.</param>
        /// <param name="intervalEnd">Where the interval stops, inclusive and finite.</param>
        /// <param name="gaussPoint">Object containing the non-negative abscissas/weights, order, and intervalBegin/intervalEnd. The non-negative abscissas/weights are generated over the interval [-1,1] for the given order.</param>
        /// <returns>Object containing the abscissas/weights, order, and intervalBegin/intervalEnd.</returns>
        private static GaussPoint<T> Map<T>(
            T intervalBegin,
            T intervalEnd,
            GaussPoint gaussPoint) where T : unmanaged
        {
            //TODO: not sure wat this work for Complex
            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();
            var abscissas = new T[gaussPoint.Order];
            var weights = new T[gaussPoint.Order];
            var num1 = la.Mult(la.Cast(0.5), la.Sub(intervalEnd, intervalBegin));
            var num2 = la.Mult(la.Cast(0.5), la.Sum(intervalEnd, intervalBegin));
            var num3 = gaussPoint.Order + 1 >> 1;

            for (var index1 = 1; index1 <= num3; ++index1)
            {
                var index2 = gaussPoint.Order - index1;
                var index3 = index1 - 1;
                var index4 = num3 - index1;
                abscissas[index2] = la.Sum(la.Mult(la.Cast(gaussPoint.Abscissas[index4]), num1), num2);

                abscissas[index3] = la.Sum(la.Mult(la.Cast(gaussPoint.Abscissas[index4]), la.Mult(num1, la.Cast(-1))), num2);
                weights[index2] = weights[index3] = la.Mult(la.Cast(gaussPoint.Weights[index4]), num1);
            }

            return new GaussPoint<T>(gaussPoint.Order, abscissas, weights, intervalBegin, intervalEnd);
        }
    }
}