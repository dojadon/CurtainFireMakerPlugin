using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VecMath;

namespace CsMmdDataIO.Mvd
{
    public static class BinaryExtension
    {
        public static BinaryReader CreateSizedBufferReader(this BinaryReader br)
        {
            return new BinaryReader(new MemoryStream(br.ReadSizedBuffer()));
        }

        public static byte[] ReadSizedBuffer(this BinaryReader br)
        {
            return br.ReadBytes(br.ReadInt32());
        }

        public static void WriteSizedBuffer(this BinaryWriter bw, byte[] buffer)
        {
            bw.Write(buffer.Length);
            bw.Write(buffer);
        }

        public static T ParseEnum<T>(string value)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            return (T)Enum.Parse(typeof(T), value, false);
        }

        public static long GetRemainingLength(this BinaryReader self)
        {
            return self.BaseStream.Length - self.BaseStream.Position;
        }

        //public static IEnumerable<T> StartWith<T>(this IEnumerable<T> self, params T[] items)
        //{
        //	return items.Concat(self);
        //}

        //public static IEnumerable<T[]> Buffer<T>(this IEnumerable<T> self, int size)
        //{
        //	var list = new List<T>();

        //	foreach (var i in self)
        //	{
        //		list.Add(i);

        //		if (list.Count >= size)
        //		{
        //			yield return list.ToArray();

        //			list.Clear();
        //		}
        //	}

        //	if (list.Any())
        //		yield return list.ToArray();
        //}

        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (var i in self)
                action(i);
        }

        public static void ForEach<T>(this IEnumerable<T> self, Action<T, int> action)
        {
            var idx = 0;

            foreach (var i in self)
                action(i, idx++);
        }

        public static IEnumerable<T> Repeat<T>(T element)
        {
            while (true)
                yield return element;
        }

        public static IEnumerable<T> Defer<T>(Func<IEnumerable<T>> func)
        {
            return func();
        }

        public static void Write(this BinaryWriter bw, Vector2 vec)
        {
            bw.Write(vec.x);
            bw.Write(vec.y);
        }

        public static void Write(this BinaryWriter bw, Vector3 vec)
        {
            bw.Write(vec.x);
            bw.Write(vec.y);
            bw.Write(vec.z);
        }

        public static void Write(this BinaryWriter bw, Vector4 vec)
        {
            bw.Write(vec.x);
            bw.Write(vec.y);
            bw.Write(vec.z);
            bw.Write(vec.w);
        }

        public static void Write(this BinaryWriter bw, Quaternion vec)
        {
            bw.Write(vec.x);
            bw.Write(vec.y);
            bw.Write(vec.z);
            bw.Write(vec.w);
        }

        public static Vector2 ReadVector2(this BinaryReader br) => new Vector2(br.ReadSingle(), br.ReadSingle());

        public static Vector3 ReadVector3(this BinaryReader br) => new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

        public static Vector4 ReadVector4(this BinaryReader br) => new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

        public static Quaternion ReadQuaternion(this BinaryReader br) => new Quaternion(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
    }
}
