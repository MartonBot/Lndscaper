using Lndscaper.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Lndscaper
{
    class Lnd
    {
        private LndHeader header;
        private readonly List<LoResTexture> lowresTextures = [];
        private readonly List<Block> blocks = [];

        public void Read(string filename)
        {
            using FileStream fs = new(filename, FileMode.Open, FileAccess.Read);
            using BinaryReader reader = new(fs);
            header.Read(reader);
            for (int i = 0; i < header.NumLoResTextures; i++)
            {
                Console.WriteLine($"Reading texture {i}...");
                LoResTexture texture = new();
                texture.Read(reader);
                lowresTextures.Add(texture);
            }
            for (int i = 1; i <= header.NumBlocks - 1; i++)
            {
                Console.WriteLine($"Reading block {i}...");
                Block block = new();
                block.Read(reader);
                blocks.Add(block);
            }

        }

        public unsafe static void ShowLndFileInfo(string lndFileName)
        {

            var fileSize = new FileInfo(lndFileName).Length;
            Console.WriteLine($"File size: {fileSize} bytes");

            var header = Binary.FileToStructure<LndHeader>(lndFileName);
            DumpHeaderInfo(header);

            Console.WriteLine("");

            Console.WriteLine($"LND header size: {sizeof(LndHeader)} bytes");
            Console.WriteLine($"Lo-res textures: {header.NumLoResTextures} x ({sizeof(LoResTexture)} + (lo-res texture data size)) bytes");

            int offset = sizeof(LndHeader);

            Console.WriteLine("");

            Console.WriteLine("Lo-res textures:");

            LoResTexture loResTextureHeader;
            for (var i = 0; i < header.NumLoResTextures; i++)
            {
                loResTextureHeader = Binary.FileToStructure<LoResTexture>(lndFileName, offset);
                offset += sizeof(LoResTexture) + (loResTextureHeader.Size - 4); // after the lo-res header come (loResTextureHeader.Size - 4) of DirectDraw stuff
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

                    // dump block data to JSON

                    var block2 = blocks[1];
                    File.WriteAllText("Test files/cells.json", JsonSerializer.Serialize<Block>(block2, new JsonSerializerOptions
                    {
                        IncludeFields = true,
                        WriteIndented = true
                    }));



                    /*
                    for (var i = 0; i < numberOfBlocks; i++)
                    {
                        Console.WriteLine($"Block[{i}]: Index = {blocks[i].BlockData.Index}, coords = {blocks[i].BlockData.BlockX}, {blocks[i].BlockData.BlockY}");
                    }
                    */

                    Console.WriteLine("Blocks: (counting from 1)");
                    // ShowMap(blocks, b => b.Cells[0].LandProperties.ToString("X"), 5);
                    // ShowMap(blocks, b => $"{b.BlockData.BlockX}:{b.BlockData.BlockY}", 7);
                    ShowMap(blocks);

                    // Func<Cell, object> cellProperty = cell => cell.HasWater ? 1 : 0;
                    Func<Cell, object> cellProperty = cell => cell.Split ? "/" : (cell.Coastline ? "o" : (cell.FullWater ? "-" : (cell.HasWater ? " " : "*")));

                    Console.WriteLine($"Block 2");
                    ShowBlock(blocks[1], cellProperty);

                    Console.WriteLine($"Block 5:");
                    ShowBlock(blocks[4], cellProperty);

                    /*
                    Func<Cell, object> altitudeProperty = cell => cell.Altitude;

                    Console.WriteLine($"Block 2");
                    ShowBlock(blocks[1], altitudeProperty);

                    Console.WriteLine($"Block 5:");
                    ShowBlock(blocks[4], altitudeProperty);
                    */

                    Func<Cell, object> countryProperty = cell => cell.Country;
                    Func<Cell, object> landProperty = cell => cell.LandProperties;

                    Console.WriteLine($"Block 2");
                    ShowBlock(blocks[1], countryProperty);

                    Console.WriteLine($"Block 5:");
                    ShowBlock(blocks[4], countryProperty);

                    Console.WriteLine("");
                    Console.WriteLine($"Current offset = {reader.BaseStream.Position}");
                    Console.WriteLine("");

                    Console.WriteLine("Countries:");

                    Country[] countries;
                    var numberOfCountries = header.NumCountries;

                    countries = reader.ReadCountries(numberOfCountries);

                    for (var i = 0; i < numberOfCountries; i++)
                    {
                        var country = countries[i];
                        Console.WriteLine($"Country[{i}]: Materials = ({country.MapMaterials[130].Index1}, {country.MapMaterials[130].Index2})");
                    }

                    Console.WriteLine("");
                    Console.WriteLine($"Current offset = {reader.BaseStream.Position:X}");
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
                    Console.WriteLine($"Current offset = {reader.BaseStream.Position:X}");
                    Console.WriteLine("");

                    Console.WriteLine("Noise map:");

                    byte[] noiseMap = new byte[256 * 256];
                    reader.Read(noiseMap, 0, noiseMap.Length);

                    Console.WriteLine("");
                    Console.WriteLine($"Current offset = {reader.BaseStream.Position:X}");
                    Console.WriteLine("");

                    Console.WriteLine("Bump map:");

                    byte[] bumpMap = new byte[256 * 256];
                    reader.Read(bumpMap, 0, bumpMap.Length);

                    Console.WriteLine("");
                    Console.WriteLine($"Current offset = {reader.BaseStream.Position:X}");

                    var leftToRead = fileSize - reader.BaseStream.Position;
                    Console.WriteLine($"Left to read: {leftToRead:X}");
                }
            }

        }

        private static unsafe void DumpHeaderInfo(LndHeader header)
        {
            Console.WriteLine($"NumBlocks: {header.NumBlocks}");
            Console.WriteLine($"NumMaterials: {header.NumMaterials}");
            Console.WriteLine($"NumCountries: {header.NumCountries}");
            Console.WriteLine($"BlockSize: {header.BlockSize}");
            Console.WriteLine($"MaterialSize: {header.MaterialSize}");
            Console.WriteLine($"CountrySize: {header.CountrySize}");
            Console.WriteLine($"NumLoResTextures: {header.NumLoResTextures}");
        }

        private static void ShowBlock(Block block, Func<Cell, object> cellProperty = null)
        {
            for (var i = 0; i < 17; i++)
            {
                for (var j = 0; j < 17; j++)
                {
                    if (cellProperty != null)
                    {
                        Console.Write(cellProperty(block.Cells[i * 17 + j]).ToString().PadRight(5));
                    }
                    else
                    {
                        Console.Write(block.Cells[i * 17 + j].Country.ToString().PadRight(5));
                    }

                }
                Console.WriteLine();
            }
        }

        private static void ShowMap(Block[] blocks, Func<Block, object> blockProperty = null, int width = 3)
        {
            string emptyBlock = "".PadRight(width);

            Block?[,] sortedBlocks = new Block?[32, 32];

            for (var i = 0; i < blocks.Length; i++)
            {
                sortedBlocks[blocks[i].BlockData.BlockX, blocks[i].BlockData.BlockY] = blocks[i];
            }

            Console.WriteLine("X:> Y:V");

            for (var i = 0; i < 32; i++) // Y axis
            {
                for (var j = 0; j < 32; j++) // X axis
                {
                    if (sortedBlocks[j, i] == null)
                    {
                        Console.Write(emptyBlock);
                    }
                    else
                    {
                        var block = sortedBlocks[j, i].Value;
                        if (blockProperty != null)
                        {

                            Console.Write(blockProperty(block).ToString().PadRight(width));
                        }
                        else
                        {
                            Console.Write(block.BlockData.Index.ToString().PadRight(width));
                        }
                    }
                }
                Console.WriteLine();
            }
        }


    }
}
