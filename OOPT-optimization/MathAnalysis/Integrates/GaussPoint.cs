namespace OOPT.Optimization.MathAnalysis.Integrates {
    public class GaussPoint
    {
        public double[] Abscissas { get; private set; }

        public double[] Weights { get; private set; }

        public int Order { get; private set; }

        public GaussPoint(int order, double[] abscissas, double[] weights)
        {
            Order = order;
            Abscissas = abscissas;
            Weights = weights;
        }
    }
    public class GaussPoint<T>
    {
        public T[] Abscissas { get; private set; }

        public T[] Weights { get; private set; }

        public int Order { get; private set; }
        public T IntervalBegin { get; private set; }

        public T IntervalEnd { get; private set; }
        public GaussPoint(int order, T[] abscissas, T[] weights, T intervalBegin, T intervalEnd)
        {
            Order = order;
            Abscissas = abscissas;
            Weights = weights;
            IntervalBegin = intervalBegin;
            IntervalEnd = intervalEnd;
        }
    }
}