using System;

namespace Lndscaper.Structures
{
    struct Material
    {
        public UInt16 TerrainType; // 0..65535
        public UInt16[] Images; // 256 x 256
    }
}
