using System;

namespace Optimization
{
  class Program
  {
    static void Main(string[] args)
    {
      var point1 = new Vector<double>(new[] { 1d, 1d, 1d });
      var point2 = new Vector<double>(new[] { 2d, 2d, 2d });
      var functional = new L1FunctionalReal(new[] { ((IVector<double>)point1, 20d), ((IVector<double>)point2, 60d) });
      var f = new LinearFunctionReal().Bind(new Vector<double>(new[] { 10d, 10d, 10d, 0d }));
      var r = functional.Value(f);
      Console.WriteLine(r);
    }
  }
}
