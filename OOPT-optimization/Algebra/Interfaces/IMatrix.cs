using System.Collections.Generic;

namespace OOPT.Optimization.Algebra.Interfaces
{
    public interface IMatrix<T> : IEnumerable<T>, IEnumerable<IVector<T>>
    {
        int RowCount { get; }

        IVector<int> ColumnsCount { get; }

        IVector<T> this[int index] { get; set; }

        T this[int row, int column] { get; set; }

    }
}