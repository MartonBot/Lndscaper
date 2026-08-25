namespace Lndscaper.Structures
{
    struct Material
    {
        public ushort TerrainType; // 0..65535
        public ushort[] Images = new ushort[256 * 256]; // 256 x 256 texels

        public Material()
        {
        }

        public void Read(BinaryReader reader)
        {
            TerrainType = reader.ReadUInt16();
            for (int i = 0; i < Images.Length; i++)
            {
                Images[i] = reader.ReadUInt16();
            }
        }
    }
}
