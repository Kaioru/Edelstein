using System;

namespace Edelstein.Common.Util
{
    public class Rotational<T>
    {
        public int Index = 0;
        public T[] Array { get; }

        public Rotational(T[] array)
        {
            Array = array;
        }

        public T Next() => Array[Index++ % Array.Length];
        public int Skip(int count = 1) => Index += count;
    }
}
