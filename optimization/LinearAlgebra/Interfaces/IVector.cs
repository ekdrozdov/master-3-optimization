using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Optimization
{
  public interface IVector<T> : IEnumerable<T>
  {
    int Count
    {
      get;
    }
    T this[int index] { get; set; }
  }

  public class Vector<T> : IVector<T>
  {
    private T[] components;
    public Vector(IEnumerable<T> components)
    {
      this.components = new T[components.Count()];
      components.ToArray().CopyTo(this.components, 0);
      this.Count = this.components.Count();
    }
    public int Count { get; private set; }

    T IVector<T>.this[int index]
    {
      get => Count > index ? this.components[index] : throw new ArgumentOutOfRangeException(nameof(index)); set
      {
        if (Count >= index)
        {
          throw new ArgumentOutOfRangeException(nameof(index));
        }
        this.components[index] = value;
        return;
      }
    }

    public T this[int index] => Count > index ? this.components[index] : throw new ArgumentOutOfRangeException(nameof(index));

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.components.GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
      foreach (var component in this.components)
      { yield return component; }
      yield break;
    }
  }
}