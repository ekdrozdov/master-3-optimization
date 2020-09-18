using System.Collections.Generic;

namespace Optimization
{
  interface ITensor<T> : IEnumerable<T>, IEnumerable<IMatrix<T>>
  {
    IVector<int> Dimensions
    {
      get;
    }
  }
}