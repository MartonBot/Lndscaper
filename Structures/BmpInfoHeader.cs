using System;

namespace Lndscaper
{
    struct BmpInfoHeader // offset 14, 40 bytes long
    {
        public UInt32 Size; // Header size in bytes
        public Int32 Width, Height; // Width and height of image
        public UInt16 Planes; // Number of colour planes
        public UInt16 Bits; // Bits per pixel
        public UInt32 Compression; // Compression type - 0, 1, 2 or 3 - hope for 0!
        public UInt32 ImageSize; // Image size in bytes
        public Int32 XResolution, YResolution; // Pixels per meter
        public UInt32 NColours; // Number of colours
        public UInt32 ImportantColours; // Important colours
    }
}
