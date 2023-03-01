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
            Console.WriteLine($"Lo-res textures: {header.NumLoResTextures} x ({sizeof(LoResTextureHeader)} + N) bytes");

            int offset = sizeof(LndHeader);

            Console.WriteLine("");

            Console.WriteLine("Lo-res textures:");

            LoResTextureHeader loResTextureHeader;
            for (var i = 0; i < header.NumLoResTextures; i++)
            {
                loResTextureHeader = Binary.FileToStructure<LoResTextureHeader>(lndFileName, offset);
                offset += sizeof(LoResTextureHeader) + (loResTextureHeader.Size - 4); // after the lo-res header come (loResTextureHeader.Size - 4) of DirectDraw stuff
                Console.WriteLine($"Lo-res texture[{i}]: ID = {loResTextureHeader.ID}, size: {(loResTextureHeader.Size - 4)} bytes, {loResTextureHeader.NumSubTextures} sub-textures");
            }

            Console.WriteLine($"Current offset = {offset}");

            // at this stage offset is at the end of the lo-res textures data

            Console.WriteLine("");

            Console.WriteLine("Blocks:");

            Block[] blocks;
            var numberOfBlocks = header.NumBlocks - 1;
            using (var stream = File.Open(lndFileName, FileMode.Open))
            {
                using (var reader = new LndReader(stream, Encoding.UTF8, false))
                {
                    blocks = reader.ReadBlocks(numberOfBlocks, offset);
                }
            }

            for (var i = 0; i < numberOfBlocks; i++)
            {
                Console.WriteLine($"Block[{i}]: Index = {blocks[i].BlockData.Index}, coords = {blocks[i].BlockData.BlockX}, {blocks[i].BlockData.BlockY}");
            }

            ShowMap(blocks);

        }

        private static void ShowMap(Block[] blocks)
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
                        Console.Write($"{sortedBlocks[i, j].Value.BlockData.Index:d3} ");
                    }
                }
                Console.WriteLine("");
            }
        }

    }
}
