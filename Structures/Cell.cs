namespace Lndscaper.Structures
{
    unsafe struct Cell
    {
        public fixed byte Color[4];
        public byte Altitude;
        public byte SaveColor;
        public fixed byte Flags[2];
    }
}
