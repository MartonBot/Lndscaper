using Lndscaper.Structures;
using System;

namespace Lndscaper
{
    struct Country
    {
        public UInt32 TerrainType; // longword is 4 bytes
        public MapMaterial[] MapMaterials; // 0..255
    }
}
