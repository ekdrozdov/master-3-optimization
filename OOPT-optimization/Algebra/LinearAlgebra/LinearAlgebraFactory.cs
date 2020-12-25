using System;
using System.Collections.Concurrent;
using System.Linq;
using OOPT.Optimization.Algebra.Interfaces;

namespace OOPT.Optimization.Algebra.LinearAlgebra
{
    public static class LinearAlgebraFactory
    {
        private static readonly ConcurrentDictionary<Type, object> LinearAlgebraCache = new ConcurrentDictionary<Type, object>();

        public static ILinearAlgebra<T> GetLinearAlgebra<T>() where T: unmanaged
        {
            var type = typeof(T);

            if (LinearAlgebraCache.TryGetValue(type, out var lA))
            {
                return (ILinearAlgebra<T>) lA;
            }

            var typeOfRealization = typeof(Program).Assembly
                .GetTypes().SingleOrDefault(x => !x.IsAbstract &&
                                                 !x.IsGenericType &&
                                                 !x.IsInterface &&
                                                 typeof(ILinearAlgebra<T>).IsAssignableFrom(x));

            if (typeOfRealization == null)
            {
                throw new NotImplementedException($"No one implementation of ILinearAlgebra<{type}> was found");
            }

            return (ILinearAlgebra<T>) LinearAlgebraCache.GetOrAdd(typeOfRealization,
                                                                   (ILinearAlgebra<T>) Activator.CreateInstance(typeOfRealization));
        }
    }
}