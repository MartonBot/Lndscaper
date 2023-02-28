using System;

namespace Lndscaper
{
    class Lnd
    {

        public unsafe static void ShowLndFileInfo(string lndFileName)
        {
            var header = Binary.FileToStructure<LndHeader>(lndFileName);
            Console.WriteLine($"NumBlocks: {header.NumBlocks}");
            Console.WriteLine($"NumMaterials: {header.NumMaterials}");
            Console.WriteLine($"NumCountries: {header.NumCountries}");
            Console.WriteLine($"BlockSize: {header.BlockSize}");
            Console.WriteLine($"MaterialSize: {header.MaterialSize}");
            Console.WriteLine($"CountrySize: {header.CountrySize}");
            Console.WriteLine($"NumLoResTextures: {header.NumLoResTextures}");

            Console.WriteLine("");

            Console.WriteLine($"LND header size: {sizeof(LndHeader)} bytes");
            Console.WriteLine($"Lo-res textures: {header.NumLoResTextures} x ({sizeof(LoResTextureHeader)} + N) bytes");

            Console.WriteLine("");

            Console.WriteLine("Lo-res textures:");

            LoResTextureHeader loResTextureHeader;
            int offset = sizeof(LndHeader);
            for (var i = 0; i < header.NumLoResTextures; i++)
            {
                loResTextureHeader = Binary.FileToStructure<LoResTextureHeader>(lndFileName, offset);
                offset += sizeof(LoResTextureHeader) + (loResTextureHeader.Size - 4); // after the lo-res header come (loResTextureHeader.Size - 4) of DirectDraw stuff
                Console.WriteLine($"Lo-res texture[{i}] size: {(loResTextureHeader.Size - 4)} bytes");
                Console.WriteLine($"Lo-res texture[{i}]: {loResTextureHeader.NumSubTextures} sub-textures");
            }

            // at this stage offset is at the end of the lo-res textures data

            Console.WriteLine("");

            Console.WriteLine("Blocks:");

        }

    }
}
