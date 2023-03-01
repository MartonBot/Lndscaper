using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Lndscaper
{
    class Binary
    {

        public static T FileToStructure<T>(string fileName, int offset = 0)
        {
            Console.WriteLine($"Extracting data from {fileName}, offset {offset}.");

            Byte[] buffer = new Byte[Marshal.SizeOf(typeof(T))];

            using var stream = File.Open(fileName, FileMode.Open);
            using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
            {
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);
                reader.Read(buffer, 0, buffer.Length);
            }

            return ArrayToStructure<T>(buffer);
        }

        public static T ArrayToStructure<T>(byte[] bytes)
        {
            GCHandle iHandle = default;
            T rTarget;
            try
            {
                iHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                rTarget = (T)Marshal.PtrToStructure(iHandle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                if (iHandle.IsAllocated)
                {
                    iHandle.Free();
                }
            }
            return rTarget;
        }

    }
}
