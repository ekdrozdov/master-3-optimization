using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.Algebra
{
    public class Vector<T> : IVector<T>
    {
        private readonly T[] _components;

        public int Count { get; }

        public Vector(params T[] components) : this(components.ToList()) { }

        public Vector(IEnumerable<T> components)
        {
            var incoming = components.Select(t => t is ICloneable t1 ? (T) t1.Clone() : t).ToArray();
            _components = new T[incoming.Length];
            incoming.ToArray().CopyTo(_components, 0);
            Count = _components.Length;
        }

        public Vector(int size, T initializeValue = default)
        {
            _components = new T[size];
            Count = size;

            if (Equals(initializeValue, default)) return;

            for (var i = 0L; i < _components.LongLength; i++)
            {
                _components[i] = initializeValue is ICloneable t1 ? (T) t1.Clone() : initializeValue;
            }
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

        public object Clone()
        {
            return new Vector<T>(_components);
        }
    }
}