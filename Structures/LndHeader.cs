using System.Text.Json.Serialization;

namespace Lndscaper.Structures
{
    struct LndHeader
    {
        [JsonInclude]
        public int NumBlocks; // number of blocks = NumBlocks - 1 (the number of blocks is incremented by 1 when crafting a LND file for some reason)
        [JsonIgnore]
        public byte[] BlockIndex;
        [JsonInclude]
        public int NumMaterials;
        [JsonInclude]
        public int NumCountries;
        [JsonInclude]
        public int BlockSize;    // sizeof(TLndBlock)    =   2520
        [JsonInclude]
        public int MaterialSize; // sizeof(TLndMaterial) = 131074
        [JsonInclude]
        public int CountrySize;  // sizeof(TLndCountry)  =   3076
        [JsonInclude]
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