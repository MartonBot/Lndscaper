using System.IO;

namespace Lndscaper
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lnd.ShowLndFileInfo("./Test files/Land3.lnd");
            Lnd.ShowLndFileInfo("./Test files/Land1.lnd");
            // Lnd.ShowLndFileInfo("./Test files/02test-1blockor2.lnd");
            // Bitmap.BmpToRaw("./Test files/02-test_1blockor2.bmp");
            // Bitmap.BmpMinAlt2("./Test files/01-test_1blockor2.bmp"); // will change all the grayscale pixels of 0 or 1 to 2
        }

        private static void AllBmp()
        {
            var dir = "./Test files";
            var bmpFilter = "*.bmp";

            var bmpFiles = Directory.EnumerateFiles(dir, bmpFilter);

            foreach (var bmpFileName in bmpFiles)
            {
                Bitmap.BmpToRaw(bmpFileName);
            }
        }

    }
}
