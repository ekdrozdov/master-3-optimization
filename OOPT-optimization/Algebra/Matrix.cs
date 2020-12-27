using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.Algebra
{
    public class Matrix<T> : IMatrix<T> where T : unmanaged
    {
        private readonly IVector<T>[] _components;

        public Matrix(int rows, int columns, params T[] defaultValuesForRows) : this(rows, columns, defaultValuesForRows.ToList())
        { }

        public Matrix(int rows, int columns, IEnumerable<T> defaultValuesForRows)
        {
            if (rows < 0)
            {
                throw new ArgumentException("Negative rows count", nameof(rows));
            }

            if (columns < 0)
            {
                throw new ArgumentException("Negative columns count", nameof(columns));
            }

            var valuesForRows = defaultValuesForRows?.ToArray();

            if (valuesForRows != null && valuesForRows.Length != rows)
            {
                throw new ArgumentException("Not enough elements", nameof(defaultValuesForRows));
            }

            _components = new IVector<T>[rows];

            if (valuesForRows == null)
            {
                for (var i = 0L; i < rows; i++)
                {
                    _components[i] = new Vector<T>(columns);
                }
            }
            else
            {
                for (var i = 0; i < rows; i++)
                {
                    _components[i] = new Vector<T>(columns, valuesForRows[i]);
                }
            }

            RowCount = _components.Length;
            ColumnsCount = new Vector<int>(_components.Select(x => x.Count).ToArray());
        }

        public Matrix(params IVector<T>[] components) : this(components.ToList())
        { }

        public Matrix(IEnumerable<IVector<T>> components)
        {
            var incoming = components.ToArray();

            this._components = new IVector<T>[incoming.LongCount()];

            for (var i = 0; i < this._components.LongLength; i++)
            {
                this._components[i] = incoming.ElementAt(i).Clone() as IVector<T>;
            }

            RowCount = this._components.Length;
            ColumnsCount = new Vector<int>(incoming.Select(x => x.Count()).ToArray());
        }

        public int RowCount { get; }

        public IVector<int> ColumnsCount { get; }

        public T this[int row, int column]
        {
            get => RowCount > row && ColumnsCount[row] > column
                ? _components[row][column]
                : throw new ArgumentOutOfRangeException(nameof(row) + " or " + nameof(column));
            set
            {
                if (RowCount <= row || ColumnsCount[row] <= column)
                {
                    throw new ArgumentOutOfRangeException(nameof(row) + " or " + nameof(column));
                }

                _components[row][column] = value;
            }
        }

        public object Clone()
        {
            return new Matrix<T>(_components);
        }

        public IVector<T> this[int row]
        {
            get =>
                RowCount > row ? _components[row] : throw new ArgumentOutOfRangeException(nameof(row));
            set
            {
                if (RowCount <= row)
                {
                    throw new ArgumentOutOfRangeException(nameof(row));
                }

                _components[row] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var vector in _components)
            {
                foreach (var element in vector)
                {
                    yield return element;
                }
            }
        }

        IEnumerator<IVector<T>> IEnumerable<IVector<T>>.GetEnumerator()
        {
            return ((IEnumerable<IVector<T>>)_components).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _components.GetEnumerator();
        }
    }
}