using Lndscaper.Structures;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Lndscaper
{
    class LndReader : BinaryReader
    {

        const int CellsPerBlock = 17 * 17;
        const int MapMaterialsPerCountry = 256;
        const int ImagesPerMaterial = 256 * 256;

        public LndReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        private T[] ReadStructArray<T>(int numItems)
        {
            T[] array = new T[numItems];
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            for (var i = 0; i < numItems; i++)
            {
                Read(buffer, 0, buffer.Length);
                array[i] = Binary.ArrayToStructure<T>(buffer);
            }
            return array;
        }

        public Block[] ReadBlocks(int numBlocks)
        {
            var blocks = new Block[numBlocks];
            byte[] buffer = new byte[Marshal.SizeOf(typeof(BlockData))];

            for (var i = 0; i < numBlocks; i++)
            {
                blocks[i].Cells = ReadStructArray<Cell>(CellsPerBlock);
                Read(buffer, 0, buffer.Length);
                blocks[i].BlockData = Binary.ArrayToStructure<BlockData>(buffer);
            }

            return blocks;
        }

        public Country[] ReadCountries(int numCountries)
        {
            var countries = new Country[numCountries];

            for (var i = 0; i < numCountries; i++)
            {
                //countries[i].TerrainType = ReadUInt32();
                countries[i].MapMaterials = ReadStructArray<MapMaterial>(MapMaterialsPerCountry);
            }

            return countries;
        }

        public Material[] ReadMaterials(int numMaterials)
        {
            var materials = new Material[numMaterials];

            for (var i = 0; i < numMaterials; i++)
            {
                materials[i].TerrainType = ReadUInt16();
                materials[i].Images = ReadStructArray<UInt16>(ImagesPerMaterial);
            }

            return materials;
        }
    }
}
