using System.Text.Json.Serialization;

namespace Lndscaper.Structures
{
    unsafe struct Cell

    {
        [JsonIgnore]
        public fixed byte Color[4];
        public byte Altitude;
        public byte SaveColor;
        [JsonIgnore]
        public byte LandProperties; // 8 bytes for: Country (4), HasWater (1), Coastline (1), FullWater (1), Split(1)
        [JsonIgnore]
        public byte SoundProperties; // Sound properties: coastal sound, land sound, sea sound, freshwater sound

        public int Country => LandProperties >> 4;
        public bool HasWater => (LandProperties & (1 << 4)) != 0;
        public bool Coastline => (LandProperties & (1 << 3)) != 0;
    }
}
