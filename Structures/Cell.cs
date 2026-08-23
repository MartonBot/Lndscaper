using System.Text.Json.Serialization;

namespace Lndscaper.Structures
{
    unsafe struct Cell
    {
        [JsonIgnore]
        public fixed byte Color[4];
        [JsonIgnore]
        public byte Altitude;
        [JsonIgnore]
        public byte SaveColor;
        [JsonIgnore]
        public byte LandProperties; // 8 bits for: Country (4), HasWater (1), Coastline (1), FullWater (1), Split(1) (but in which order?)
        [JsonIgnore]
        public byte SoundProperties; // Sound properties: coastal sound, land sound, sea sound, freshwater sound

        //public int Country => ((LandProperties & 128) >> 7) | ((LandProperties & 64) >> 5) | ((LandProperties & 32) >> 3) | ((LandProperties & 16)) >> 1;
        public int Country => LandProperties & 0x0F;
        public bool HasWater => (LandProperties & (1 << 4)) != 0;
        public bool Coastline => (LandProperties & (1 << 5)) != 0;
        public bool FullWater => (LandProperties & (1 << 6)) != 0;
        public bool Split => (LandProperties & (1 << 7)) != 0;
    }
}
