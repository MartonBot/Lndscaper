using System;
using System.IO;

namespace Lndscaper.Structures
{
    struct LoResTexture
    {
        private const int MAX_TEXTURE_SIZE = 8 * 1024 * 1024;
        public int Texture;
        public int Material;
        public int NumSubTextures;
        public int ID;
        public int Size;
        public DDSTexture ddsTexture = new();

        public LoResTexture()
        {
        }

        public void Read(BinaryReader reader)
        {
            Texture = reader.ReadInt32();
            Material = reader.ReadInt32();
            NumSubTextures = reader.ReadInt32();
            ID = reader.ReadInt32();
            Size = reader.ReadInt32();
            if (Size < 124 || Size > MAX_TEXTURE_SIZE) throw new Exception($"Invalid texture size: {Size}");
            ddsTexture.Read(reader);
            if (ddsTexture.Size != Size) {
				throw new Exception($"Wrong texture size: {ddsTexture.Size}, should be {Size}");
			}
        }
    }
}
