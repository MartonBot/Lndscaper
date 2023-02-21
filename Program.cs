using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Lndscaper
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = "./Test files";
            var lndFilter = "*.lnd";
            Byte[] buffer = new Byte[Marshal.SizeOf(typeof(LndHeader))];

            var lndFiles = Directory.EnumerateFiles(dir, lndFilter);

            foreach (var lndFileName in lndFiles)
            {
                Console.WriteLine($"Processing {lndFileName}.");

                using var stream = File.Open(lndFileName, FileMode.Open);
                using var reader = new BinaryReader(stream, Encoding.UTF8, false);
                reader.Read(buffer, 0, buffer.Length);
                var header = ArrayToStructure<LndHeader>(buffer);
                Console.WriteLine($"There are {header.NumBlocks} blocks in the '{lndFileName}' header.");

            }
        }

        private unsafe struct LndHeader
        {
            public Int32 NumBlocks;
            public fixed byte BlockIndex[32 * 32];
            public Int32 NumMaterials;
            public Int32 NumCountries;
            public Int32 BlockSize;    // sizeof(TLndBlock)    =   2520
            public Int32 MaterialSize; // sizeof(TLndMaterial) = 131074
            public Int32 CountrySize;  // sizeof(TLndCountry)  =   3076
            public UInt32 NumLoResTextures; // why unsigned?
        }

        private static T ArrayToStructure<T>(byte[] abSource)
        {
            GCHandle iHandle = default;
            T rTarget;
            try
            {
                iHandle = GCHandle.Alloc(abSource, GCHandleType.Pinned);
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
