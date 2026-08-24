using System;
using System.IO;

namespace Lndscaper.Structures
{
    struct DDSTexture
    {
        public DDSHeader header = new();
        public byte[] bdata;

        // DDS files begin with a 4-byte magic identifier, typically "DDS "
        // before the 124-byte DDS header and the pixel data itself.
        // See https://learn.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide#dds-file-layout
        public readonly int Size => 4 + DDSHeader.SIZE + bdata.Length;

        public DDSTexture()
        {
        }

        public void Read(BinaryReader reader)
        {
            header.Read(reader);
            if (header.HasHeader10())
            {
                throw new Exception("DX10 is not suported");
                //Read DDS_HEADER_DXT10
            }
            bdata = new byte[header.width * header.height];
            reader.Read(bdata, 0, bdata.Length);
        }
    }

}