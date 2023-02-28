using System;
using System.IO;
using System.Text;

namespace Lndscaper
{
    class Bitmap
    {

        public static void BmpToRaw(string bmpFileName)
        {
            var infoHeader = Binary.FileToStructure<BmpInfoHeader>(bmpFileName, 14);
            var nColours = infoHeader.NColours;

            var palette = new byte[4 * nColours];
            var rawSize = infoHeader.Width * infoHeader.Height;
            var rawImage = new byte[rawSize];

            using (var stream = File.Open(bmpFileName, FileMode.Open))
            {
                using var reader = new BinaryReader(stream, Encoding.UTF8, false);

                var paletteOffset = 54;

                // first fill the palette
                reader.BaseStream.Seek(paletteOffset, SeekOrigin.Begin);
                reader.Read(palette, 0, palette.Length);

                // then resolve the raw image
                for (var i = 0; i < rawSize; i++)
                {
                    rawImage[i] = palette[reader.ReadByte() * 4];
                }
            }

            // write the raw image
            var rawFileName = Path.GetFileNameWithoutExtension(bmpFileName) + ".raw";
            using (var stream = File.Open(rawFileName, FileMode.Create))
            {
                using var writer = new BinaryWriter(stream, Encoding.UTF8, false);
                writer.Write(rawImage);
                Console.WriteLine($"Written {rawFileName}");
            }

        }

        public static void ShowBmpFileInfo(string bmpFileName)
        {
            var infoHeader = Binary.FileToStructure<BmpInfoHeader>(bmpFileName, 14);
            Console.WriteLine($"Number of colours in palette: {infoHeader.NColours}");
        }

        public static void BmpMinAlt2(string bmpFileName)
        {
            var paletteOffset = 54;
            var infoHeader = Binary.FileToStructure<BmpInfoHeader>(bmpFileName, 14);
            var nColours = infoHeader.NColours;
            var imageSize = infoHeader.Width * infoHeader.Height;

            var bmpHeaders = new byte[paletteOffset];
            var palette = new byte[4 * nColours];

            var image = new byte[imageSize];

            using (var stream = File.Open(bmpFileName, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    // store the 2 headers in an array
                    reader.Read(bmpHeaders, 0, paletteOffset);

                    // store the palette
                    reader.Read(palette, 0, palette.Length);

                    // store the image
                    reader.Read(image, 0, image.Length);
                }
            }

            // find the indexes in the palette that correspond to 0, 1, 2
            byte index0 = 0b0, index1 = 0b0, index2 = 0b0;
            for (var i = 0; i < nColours; i++)
            {
                var isGrayscale = (palette[4 * i] == palette[4 * i + 1]) && (palette[4 * i + 1] == palette[4 * i + 2]);
                if (!isGrayscale) continue;
                var level = palette[4 * i];

                if (level == 0b0)
                {
                    index0 = (byte)i;
                }
                if (level == 0b1)
                {
                    index1 = (byte)i;
                }
                if (level == 0b10)
                {
                    index2 = (byte)i;
                }
            }

            // go through the image and update relevant pixels
            for (var i = 0; i < image.Length; i++)
            {
                if (image[i] == index0 || image[i] == index1)
                {
                    image[i] = index2;
                }
            }

            // write the updated bitmap
            var updatedFileName = Path.GetFileNameWithoutExtension(bmpFileName) + "_minalt2.bmp";
            using (var stream = File.Open(updatedFileName, FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    writer.Write(bmpHeaders);
                    writer.Write(palette);
                    writer.Write(image);
                    Console.WriteLine($"Written {updatedFileName}");
                }

            }
        }

    }
}
