using System;
using System.Linq;
using System.Collections.Generic;

namespace Optimization
{
  public class L1FunctionalReal : IFunctional<double>
  {
    (IVector<double> point, double target)[] elements;
    public L1FunctionalReal(IEnumerable<(IVector<double>, double)> points)
    {
      this.elements = new (IVector<double>, double)[points.Count()];
      points.ToArray().CopyTo(this.elements, 0);
    }
    public double Value(IFunction<double> f)
    {
      return elements.Sum(x => Math.Abs(f.Value(x.point) - x.target));
    }
  }
}