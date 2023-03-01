using System;

namespace Lndscaper
{
    // all the data in a Block, except for the array of cells
    unsafe struct BlockData
    {
        public Int32 Index;
        public float MapY; // scripting X coord = BlockX * 160
        public float MapX; // scripting Y coord = BlockY * 160
        public Int32 BlockY; // X coord in Header.BlockIndex
        public Int32 BlockX; // Y coord in Header.BlockIndex
        public Int32 Clipped; // boolean
        public UInt32 FrameVisibility; // longword
        public UInt32 Unused; // longword
        public Int32 UseSmallBump; // boolean
        public Int32 ForceLoResTex; // boolean
        public UInt32 MeshLOD; // enum
        public UInt32 MeshBlending; // enum
        public UInt32 TextureBlend; // enum
        public UInt32 MeshLODType; // ? not defined
        public UInt32 Fog; // ? not defined
        public Int32 Texture; // pointer
        public Int32 Material; // pointer
        public Int32 DrawSomething; // ???
        public Int32 SpecialMaterialBefore; // pointer
        public Int32 SpecialMaterialAfter; // pointer
        public fixed float TransformUVBefore[12]; // ?
        public fixed float TransformUVAfter[12]; // ?
        public Int32 NextSorting; // pointer
        public float ValueSorting;
        public UInt32 LoResTexture; // longword
        public float fu_lrs; // = (iu_lrs / 256)
        public float fv_lrs; // = (iv_lrs / 256)
        public Int32 iu_lrs; // X coord of LowResTex sub-image
        public Int32 iv_lrs; // Y coord of LowResTex sub-image
        public Int32 SmallTexUpdated; // boolean
    }
}
