using System.IO;

namespace Lndscaper.Structures
{
    struct Block
    {
        public Cell[] Cells = new Cell[17 * 17]; // 17x17 array of cells
        public BlockData BlockData = new(); // everything but the array of cells

        public Block()
        {
        }
        
        public void Read(BinaryReader reader)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = new();
                Cells[i].Read(reader);
            }
            
            BlockData.Read(reader);
        }
    }
}
