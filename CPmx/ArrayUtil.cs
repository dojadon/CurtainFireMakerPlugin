using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsPmx
{
    public class ArrayUtil
    {
        public static T[] Set<T>(T[] array, Func<int, T> func)
        {
            Enumerable.Range(0, array.Length).ToList().ForEach(i => array[i] = func.Invoke(i));
            return array;
        }
    }
}
