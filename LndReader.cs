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

        public LndReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public Cell[] ReadCells(int numCells)
        {
            Cell[] cells = new Cell[numCells];
            byte[] buffer = new byte[Marshal.SizeOf(typeof(Cell))];
            for (var i = 0; i < numCells; i++)
            {
                // Console.WriteLine($"Reading Cell of size {Marshal.SizeOf(typeof(Cell))} at offset {BaseStream.Position}");
                base.Read(buffer, 0, buffer.Length);
                cells[i] = Binary.ArrayToStructure<Cell>(buffer);
            }
            return cells;
        }

        public Block[] ReadBlocks(int numBlocks, int offset = 0)
        {
            var blocks = new Block[numBlocks];
            byte[] buffer = new byte[Marshal.SizeOf(typeof(BlockData))];

            base.BaseStream.Seek(offset, SeekOrigin.Begin);

            for (var i = 0; i < numBlocks; i++)
            {
                blocks[i].Cells = ReadCells(CellsPerBlock);
                // Console.WriteLine($"Reading BlockData of size {Marshal.SizeOf(typeof(BlockData))} at offset {BaseStream.Position}");
                base.Read(buffer, 0, buffer.Length);
                blocks[i].BlockData = Binary.ArrayToStructure<BlockData>(buffer);
            }

            return blocks;
        }
    }
}
