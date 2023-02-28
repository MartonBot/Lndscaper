using System;

namespace Lndscaper
{
    unsafe struct LndHeader
    {
        public Int32 NumBlocks;
        public fixed byte BlockIndex[32 * 32];
        public Int32 NumMaterials;
        public Int32 NumCountries;
        public Int32 BlockSize;    // sizeof(TLndBlock)    =   2520
        public Int32 MaterialSize; // sizeof(TLndMaterial) = 131074
        public Int32 CountrySize;  // sizeof(TLndCountry)  =   3076
        public UInt32 NumLoResTextures; // why unsigned?
    }
}
