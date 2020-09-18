using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Optimization
{
  public interface IMatrix<T> : IEnumerable<T>, IEnumerable<IVector<T>>
  {
    int RowCount
    {
      get;
    }
    IVector<int> ColumnsCount
    {
      get;
    }
    IVector<T> this[int index] { get; set; }
    T this[int row, int column] { get; set; }

  }

  public class Matrix<T> : IMatrix<T>
  {
    private IVector<T>[] components;
    public Matrix(IEnumerable<IVector<T>> components)
    {
      this.components = new IVector<T>[components.Count()];
      for (int i = 0; i < this.components.Count(); i++)
      {
        this.components[i] = components.ElementAt(i);

        //TODO: add ivector function copyTo  
      }
      this.RowCount = this.components.Count();
      this.ColumnsCount = new Vector<int>(components.Select(x => x.Count).ToArray());
    }
    public int RowCount { get; private set; }

    public IVector<int> ColumnsCount { get; private set; }

    T IMatrix<T>.this[int row, int column]
    {
      get => RowCount < row && ColumnsCount[column] < column
? this.components[row][column] : throw new ArgumentOutOfRangeException(nameof(row) + " or " + nameof(column)); set
      {
        if (RowCount >= row || ColumnsCount[column] >= column)
        {
          throw new ArgumentOutOfRangeException(nameof(row) + " or " + nameof(column));
        }
        this.components[row][column] = value;
        return;
      }
    }
    IVector<T> IMatrix<T>.this[int row]
    {
      get => RowCount < row ? this.components[row] : throw new ArgumentOutOfRangeException(nameof(row)); set
      {
        if (RowCount >= row)
        {
          throw new ArgumentOutOfRangeException(nameof(row));
        }
        this.components[row] = value;
        return;
      }
    }
    public IEnumerator<T> GetEnumerator()
    {
      foreach (var vector in this.components)
        foreach (var element in vector)
        { yield return element; }
      yield break;
    }

    IEnumerator<IVector<T>> IEnumerable<IVector<T>>.GetEnumerator()
    {
      foreach (var component in this.components)
      { yield return component; }
      yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.components.GetEnumerator();
    }
  }
}