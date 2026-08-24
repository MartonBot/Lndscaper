using System;
using System.IO;

namespace Lndscaper
{
    struct LndHeader
    {
        public Int32 NumBlocks; // number of blocks = NumBlocks - 1 (the number of blocks is incremented by 1 when crafting a LND file for some reason)
        public byte[] BlockIndex;
        public int NumMaterials;
        public int NumCountries;
        public int BlockSize;    // sizeof(TLndBlock)    =   2520
        public int MaterialSize; // sizeof(TLndMaterial) = 131074
        public int CountrySize;  // sizeof(TLndCountry)  =   3076
        public uint NumLoResTextures; // why unsigned?

        public void Read(BinaryReader reader)
        {
            NumBlocks = reader.ReadInt32();
            BlockIndex = reader.ReadBytes(32 * 32);
            NumMaterials = reader.ReadInt32();
            NumCountries = reader.ReadInt32();
            BlockSize = reader.ReadInt32();
            MaterialSize = reader.ReadInt32();
            CountrySize = reader.ReadInt32();
            NumLoResTextures = reader.ReadUInt32();
        }
    }
}