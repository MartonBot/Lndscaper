using System;

namespace Lndscaper
{
    unsafe struct LoResTextureHeader // this is only the header part of the lo-res texture, after it come (loRexTextureHeader.Size - 4) bytes of DirectDraw "crap"
    {
        public Int32 Texture;
        public Int32 Material;
        public Int32 NumSubTextures;
        public Int32 ID;
        public Int32 Size;
    }
}
