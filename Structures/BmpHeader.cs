using System;

namespace Lndscaper
{
    struct BmpHeader // 14 bytes long
    {
        public Int16 Type;
        public Int32 Size;
        public Int16 Reserved1, Reserved2;
        public Int32 Offset;
    }
}
