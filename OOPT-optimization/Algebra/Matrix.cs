using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.Algebra
{
    public class Matrix<T> : IMatrix<T>
    {
        private readonly IVector<T>[] components;

        public Matrix(IEnumerable<IVector<T>> components)
        {
            var incoming = components.ToArray();

            this.components = new IVector<T>[incoming.LongCount()];

            for (var i = 0; i < this.components.LongLength; i++)
            {
                this.components[i] = incoming.ElementAt(i).Clone();
            }

            RowCount = this.components.Length;
            ColumnsCount = new Vector<int>(incoming.Select(x => x.Count()).ToArray());
        }

        public int RowCount { get; }

        public IVector<int> ColumnsCount { get; }

        public T this[int row, int column]
        {
            get => RowCount < row && ColumnsCount[column] < column
                ? components[row][column]
                : throw new ArgumentOutOfRangeException(nameof(row) + " or " + nameof(column));
            set
            {
                if (RowCount <= row || ColumnsCount[column] <= column)
                {
                    throw new ArgumentOutOfRangeException(nameof(row) + " or " + nameof(column));
                }

                components[row][column] = value;
            }
        }

        public IVector<T> this[int row]
        {
            get => RowCount < row ? components[row] : throw new ArgumentOutOfRangeException(nameof(row));
            set
            {
                if (RowCount <= row)
                {
                    throw new ArgumentOutOfRangeException(nameof(row));
                }

                components[row] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var vector in components)
            {
                foreach (var element in vector)
                {
                    yield return element;
                }
            }
        }

        IEnumerator<IVector<T>> IEnumerable<IVector<T>>.GetEnumerator()
        {
            return ((IEnumerable<IVector<T>>) components).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return components.GetEnumerator();
        }
    }
}