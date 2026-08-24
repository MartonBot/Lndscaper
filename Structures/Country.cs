using System.IO;

namespace Lndscaper.Structures
{
    struct Country
    {
        public int TerrainType; // longword is 4 bytes
        public MapMaterial[] MapMaterials = new MapMaterial[256]; // 0..255

        public Country()
        {
        }

        public void Read(BinaryReader reader)
        {
            TerrainType = reader.ReadInt32();
            for (int i = 0; i < MapMaterials.Length; i++)
            {
                MapMaterials[i] = new MapMaterial();
                MapMaterials[i].Read(reader);
            }
        }
    }
}
