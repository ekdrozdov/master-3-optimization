using System;
using System.Collections.Generic;

namespace OOPT.Optimization.Algebra.Interfaces
{
    public interface IVector<T> : IEnumerable<T>, ICloneable
    {
        int Count { get; }

        T this[int index] { get; set; }
    }
}