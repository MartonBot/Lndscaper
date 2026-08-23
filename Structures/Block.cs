using System;
using System.Text.Json.Serialization;

namespace Lndscaper.Structures
{
    struct Block
    {
        public Cell[] Cells;
        public BlockData BlockData; // everything but the array of cells
        
    }
}
