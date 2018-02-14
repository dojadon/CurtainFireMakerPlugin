using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin
{
    public static class CollectionUtil
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var t in collection) action(t);
        }
    }

    public class ArrayEqualityComparer<T> : IEqualityComparer<T[]>
    {
        public bool Equals(T[] x, T[] y)
        {
            return x.Length == y.Length && Enumerable.Range(0, x.Length).All(i => x[i].Equals(y[i]));
        }

        public int GetHashCode(T[] obj)
        {
            int result = 17;
            foreach (var o in obj)
            {
                result = result * 23 + o.GetHashCode();
            }
            return result;
        }
    }
}
