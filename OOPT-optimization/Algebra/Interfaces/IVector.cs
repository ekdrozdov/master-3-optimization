using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace OOPT.Optimization.Algebra.Interfaces
{
    public interface IVector<T> : IEnumerable<T>
    {
        int Count { get; }

        T this[int index] { get; set; }

        IVector<T> Clone();
    }
}