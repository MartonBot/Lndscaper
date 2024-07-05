using Lndscaper.Structures;
using System;
using System.IO;
using System.Text;

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
            Console.WriteLine($"Lo-res textures: {header.NumLoResTextures} x ({sizeof(LoResTextureHeader)} + (lo-res texture data size)) bytes");

            int offset = sizeof(LndHeader);

            Console.WriteLine("");

            Console.WriteLine("Lo-res textures:");

            LoResTextureHeader loResTextureHeader;
            for (var i = 0; i < header.NumLoResTextures; i++)
            {
                loResTextureHeader = Binary.FileToStructure<LoResTextureHeader>(lndFileName, offset);
                offset += sizeof(LoResTextureHeader) + (loResTextureHeader.Size - 4); // after the lo-res header come (loResTextureHeader.Size - 4) of DirectDraw stuff
                // somehow, the value that is read for the size of the lo-res texture data is not accurate, you have to remove 4 bytes from it. I guess just accept that
                Console.WriteLine($"Lo-res texture[{i}]:");
                // Console.WriteLine($"\ttexture pointer == {loResTextureHeader.Texture}");
                // Console.WriteLine($"\tmaterial pointer == {loResTextureHeader.Material}");
                Console.WriteLine($"\tnumber of sub-textures == {loResTextureHeader.NumSubTextures}");
                Console.WriteLine($"\tlo-res texture ID == {loResTextureHeader.ID}");
                Console.WriteLine($"\tlo-res texture size == {(loResTextureHeader.Size - 4)} bytes (subtracted 4 bytes from read value to obtain the size of the lo-res texture data)");
                Console.WriteLine("");
            }

            Console.WriteLine($"Current offset = {offset}");

            // at this stage offset is at the end of the lo-res textures data

            using (var stream = File.Open(lndFileName, FileMode.Open))
            {
                using (var reader = new LndReader(stream, Encoding.UTF8, false))
                {

                    reader.BaseStream.Seek(offset, SeekOrigin.Begin);

                    Console.WriteLine("");

                    Console.WriteLine("Blocks:");

                    Block[] blocks;
                    var numberOfBlocks = header.NumBlocks - 1; // there is one less block than advertised

                    blocks = reader.ReadBlocks(numberOfBlocks);


                    /*
                    for (var i = 0; i < numberOfBlocks; i++)
                    {
                        Console.WriteLine($"Block[{i}]: Index = {blocks[i].BlockData.Index}, coords = {blocks[i].BlockData.BlockX}, {blocks[i].BlockData.BlockY}");
                    }
                    */

                    // ShowMap(blocks);
                    ShowMap(blocks, b => b.BlockData.Clipped);

                    Console.WriteLine("");
                    Console.WriteLine($"Current offset = {reader.BaseStream.Position}");
                    Console.WriteLine("");

                    Console.WriteLine("Countries:");

                    Country[] countries;
                    var numberOfCountries = header.NumCountries;

                    countries = reader.ReadCountries(numberOfCountries);

                    for (var i = 0; i < numberOfCountries; i++)
                    {
                        Console.WriteLine($"Country[{i}]: TerrainType = {countries[i].TerrainType}");
                    }

                    Console.WriteLine("");
                    Console.WriteLine($"Current offset = {reader.BaseStream.Position}");
                    Console.WriteLine("");

                    Console.WriteLine("Materials:");

                    Material[] materials;
                    var numberOfMaterials = header.NumMaterials;

                    materials = reader.ReadMaterials(numberOfMaterials);

                    for (var i = 0; i < numberOfMaterials; i++)
                    {
                        Console.WriteLine($"Material[{i}]: TerrainType = {materials[i].TerrainType}");
                    }

                    Console.WriteLine("");
                    Console.WriteLine($"Current offset = {reader.BaseStream.Position}");
                    Console.WriteLine("");

                    Console.WriteLine("Noise map:");

                    byte[] noiseMap = new byte[256 * 256];
                    reader.Read(noiseMap, 0, noiseMap.Length);

                    Console.WriteLine("");
                    Console.WriteLine($"Current offset = {reader.BaseStream.Position}");
                    Console.WriteLine("");

                    Console.WriteLine("Bump map:");

                    byte[] bumpMap = new byte[256 * 256];
                    reader.Read(bumpMap, 0, bumpMap.Length);

                    Console.WriteLine("");
                    Console.WriteLine($"Current offset = {reader.BaseStream.Position}");

                }
            }

        }

        private static void ShowMap(Block[] blocks, Func<Block, object> blockProperty = null)
        {
            Block?[,] sortedBlocks = new Block?[32, 32];

            for (var i = 0; i < blocks.Length; i++)
            {
                sortedBlocks[blocks[i].BlockData.BlockX, blocks[i].BlockData.BlockY] = blocks[i];
            }

            for (var i = 0; i < 32; i++)
            {
                for (var j = 0; j < 32; j++)
                {
                    if (sortedBlocks[i, j] == null)
                    {
                        Console.Write("    ");
                    }
                    else
                    {
                        if (blockProperty != null)
                        {
                            Console.Write($"{blockProperty(sortedBlocks[i, j].Value):d3} ");
                        }
                        else
                        {
                            Console.Write($"{sortedBlocks[i, j].Value.BlockData.Index:d3} ");
                        }
                    }
                }
                Console.WriteLine();
            }
        }


    }
}
