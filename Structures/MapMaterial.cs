using System.IO;

namespace Lndscaper.Structures
{
    struct MapMaterial
    {
        public uint Index1;
        public uint Index2;
        public uint Coef1;

        public void Read(BinaryReader reader)
        {
            Index1 = reader.ReadUInt32();
            Index2 = reader.ReadUInt32();
            Coef1 = reader.ReadUInt32();
        }
    }
}
