using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.Algebra
{
    public class Vector<T> : IVector<T>
    {
        private readonly T[] _components;

        public int Count { get; }

        public Vector(IEnumerable<T> components)
        {
            var incoming = components.ToArray();
            _components = new T[incoming.Length];
            incoming.ToArray().CopyTo(_components, 0);
            Count = _components.Length;
        }

        public Vector(int size)
        {
            _components = new T[size];
        }

        public T this[int index]
        {
            get => Count > index ? _components[index] : throw new ArgumentOutOfRangeException(nameof(index));
            set
            {
                if (Count <= index)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                _components[index] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) _components).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IVector<T> Clone()
        {
            return new Vector<T>(_components);
        }
    }
}