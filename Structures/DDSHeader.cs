using System;
using System.IO;

namespace Lndscaper
{
    struct DDSHeader
    {
        public static int SIZE = 124;

        public static int DDSD_CAPS = 0x1;
        public static int DDSD_HEIGHT = 0x2;
        public static int DDSD_WIDTH = 0x4;
        public static int DDSD_PITCH = 0x8;
        public static int DDSD_PIXELFORMAT = 0x1000;
        public static int DDSD_MIPMAPCOUNT = 0x20000;
        public static int DDSD_LINEARSIZE = 0x80000;
        public static int DDSD_DEPTH = 0x800000;
        public static int DDSCAPS_COMPLEX = 0x8;
        public static int DDSCAPS_MIPMAP = 0x400000;
        public static int DDSCAPS_TEXTURE = 0x1000;
        public static int DDSCAPS2_CUBEMAP = 0x200;
        public static int DDSCAPS2_CUBEMAP_POSITIVEX = 0x400;
        public static int DDSCAPS2_CUBEMAP_NEGATIVEX = 0x800;
        public static int DDSCAPS2_CUBEMAP_POSITIVEY = 0x1000;
        public static int DDSCAPS2_CUBEMAP_NEGATIVEY = 0x2000;
        public static int DDSCAPS2_CUBEMAP_POSITIVEZ = 0x4000;
        public static int DDSCAPS2_CUBEMAP_NEGATIVEZ = 0x8000;
        public static int DDSCAPS2_VOLUME = 0x2000000;

        public int size = SIZE;
        public int flags = DDSD_CAPS | DDSD_HEIGHT | DDSD_WIDTH | DDSD_PIXELFORMAT;
        public int height;
        public int width;
        public int pitchOrLinearSize;
        public int depth;
        public int mipMapCount;
        public int[] reserved1 = new int[11];
        public DDSPixelFormat ddspf;
        public int caps = DDSCAPS_TEXTURE;
        public int caps2;
        public int caps3;
        public int caps4;
        public int reserved2;

        public DDSHeader()
        {
        }

        public void Read(BinaryReader reader)
        {
            size = reader.ReadInt32();
            if (size != SIZE) throw new Exception($"Invalid DDSHeader size: {size}");
            flags = reader.ReadInt32();
            height = reader.ReadInt32();
            if (height % 4 != 0) throw new Exception($"Height must be a multiple of 4: {height}");
            width = reader.ReadInt32();
            if (width % 4 != 0) throw new Exception($"Width must be a multiple of 4: {width}");
            pitchOrLinearSize = reader.ReadInt32();
            depth = reader.ReadInt32();
            mipMapCount = reader.ReadInt32();
            if (mipMapCount != 0) throw new Exception("MipMaps are not supported");
            for (int i = 0; i < reserved1.Length; i++)
            {
                reserved1[i] = reader.ReadInt32();
            }
            ddspf.Read(reader);
            caps = reader.ReadInt32();
            caps2 = reader.ReadInt32();
            caps3 = reader.ReadInt32();
            caps4 = reader.ReadInt32();
            reserved2 = reader.ReadInt32();
        }

        public readonly bool HasHeader10()
        {
            // See https://learn.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide#dds-file-layout
            return (ddspf.flags & DDSPixelFormat.DDPF_FOURCC) != 0 && "DX10".Equals(ddspf.fourCC);
        }
    }
}