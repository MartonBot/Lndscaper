using System.Drawing;
using System.Drawing.Imaging;

namespace Lndscaper.Structures
{
    struct Material
    {
        public ushort TerrainType; // 0..65535
        public ushort[] Images = new ushort[256 * 256]; // 256 x 256 texels

        public Material()
        {
        }

        public void Read(BinaryReader reader)
        {
            TerrainType = reader.ReadUInt16();
            for (int i = 0; i < Images.Length; i++)
            {
                Images[i] = reader.ReadUInt16();
            }
        }

        public System.Drawing.Bitmap GetImage()
        {
            var width = 256;
            var height = 256;
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(width, height, PixelFormat.Format32bppArgb);

            for (int i = 0; i < Images.Length; i++)
            {
                int texel = (ushort)Images[i];

                int red =
                    ((texel & 0b0111110000000000) >> 7) |
                    ((texel & 0b0000011100000000) >> 12);

                int green =
                    ((texel & 0b0000001111100000) >> 2) |
                    ((texel & 0b0000000001110000) >> 7);

                int blue =
                    ((texel & 0b0000000000011111) << 3) |
                    ((texel & 0b0000000000011100) >> 2);

                int alpha = (texel & 0b1000000000000000) == 0
                    ? 255
                    : 0;

                int x = i % width;
                int y = i / width;

                image.SetPixel(x, y, Color.FromArgb(alpha, red, green, blue));
            }

            return image;
        }
    }
}
