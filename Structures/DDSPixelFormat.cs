using System;
using System.Collections.Generic;
using System.IO;

namespace Lndscaper.Structures
{
    struct DDSPixelFormat
    {
        public static int SIZE = 32;

        public static int DDPF_ALPHAPIXELS = 0x1;
        public static int DDPF_ALPHA = 0x2;
        public static int DDPF_FOURCC = 0x4;
        public static int DDPF_RGB = 0x40;
        public static int DDPF_YUV = 0x200;
        public static int DDPF_LUMINANCE = 0x20000;

        private static ISet<string> CompressedFormats = new HashSet<string>();
        private static ISet<string> SupportedCompressions = new HashSet<string>();

        public int size = SIZE;
        public int flags = DDPF_FOURCC;
        public string fourCC = "DXT3";
        public int rgbBitCount;
        public int rBitMask;
        public int gBitMask;
        public int bBitMask;
        public int aBitMask;

        public DDSPixelFormat()
        {
        }

        static DDSPixelFormat()
        {
            CompressedFormats.Add("DXT1");
            CompressedFormats.Add("DXT2");
            CompressedFormats.Add("DXT3");
            CompressedFormats.Add("DXT4");
            CompressedFormats.Add("DXT5");
            SupportedCompressions.Add("DXT3");
        }

        public void Read(BinaryReader reader)
        {
            size = reader.ReadInt32();
            if (size != SIZE) throw new Exception($"Invalid DDSPixelFormat size: {size}");
            flags = reader.ReadInt32();
            if (flags != DDPF_FOURCC)
            {
                throw new Exception($"Unsupported flags: {flags}");
            }
            fourCC = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(4));
            if (!SupportedCompressions.Contains(fourCC))
            {
                throw new Exception("Unsupported compression type: " + fourCC);
            }
            rgbBitCount = reader.ReadInt32();
            rBitMask = reader.ReadInt32();
            gBitMask = reader.ReadInt32();
            bBitMask = reader.ReadInt32();
            aBitMask = reader.ReadInt32();
        }
    }
}