using System.IO;

namespace Lndscaper.Structures
{
    // all the data in a Block, except for the array of cells
    struct BlockData
    {
        public int Index;
        public float MapY; // scripting X coord = BlockX * 160
        public float MapX; // scripting Y coord = BlockY * 160
        public int BlockY; // X coord in Header.BlockIndex
        public int BlockX; // Y coord in Header.BlockIndex
        public int Clipped; // boolean
        public uint FrameVisibility; // longword
        public uint HighestAltitude; // longword
        public int UseSmallBump; // boolean
        public int ForceLoResTex; // boolean
        public uint MeshLOD; // enum
        public uint MeshBlending; // enum
        public uint TextureBlend; // enum
        public uint MeshLODType; // ? not defined
        public uint Fog; // ? not defined
        public int TexturePtr; // pointer
        public int MaterialPtr; // pointer
        public int DrawSomething; // ???
        public int SpecialMaterialBefore; // pointer
        public int SpecialMaterialAfter; // pointer
        public float[] TransformUVBefore = new float[12]; // ?
        public float[] TransformUVAfter = new float[12]; // ?
        public int NextSorting; // pointer
        public float ValueSorting;
        public uint LoResTexture; // longword
        public float fu_lrs; // = (iu_lrs / 256)
        public float fv_lrs; // = (iv_lrs / 256)
        public int iu_lrs; // X coord of LowResTex sub-image
        public int iv_lrs; // Y coord of LowResTex sub-image
        public int SmallTexUpdated; // boolean

        public BlockData()
        {
        }

        public void Read(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            MapY = reader.ReadSingle();
            MapX = reader.ReadSingle();
            BlockY = reader.ReadInt32();
            BlockX = reader.ReadInt32();
            Clipped = reader.ReadInt32();
            FrameVisibility = reader.ReadUInt32();
            HighestAltitude = reader.ReadUInt32();
            UseSmallBump = reader.ReadInt32();
            ForceLoResTex = reader.ReadInt32();
            MeshLOD = reader.ReadUInt32();
            MeshBlending = reader.ReadUInt32();
            TextureBlend = reader.ReadUInt32();
            MeshLODType = reader.ReadUInt32();
            Fog = reader.ReadUInt32();
            TexturePtr = reader.ReadInt32();
            MaterialPtr = reader.ReadInt32();
            DrawSomething = reader.ReadInt32();
            SpecialMaterialBefore = reader.ReadInt32();
            SpecialMaterialAfter = reader.ReadInt32();

            for (int i = 0; i < 12; i++)
                TransformUVBefore[i] = reader.ReadSingle();

            for (int i = 0; i < 12; i++)
                TransformUVAfter[i] = reader.ReadSingle();

            NextSorting = reader.ReadInt32();
            ValueSorting = reader.ReadSingle();
            LoResTexture = reader.ReadUInt32();
            fu_lrs = reader.ReadSingle();
            fv_lrs = reader.ReadSingle();
            iu_lrs = reader.ReadInt32();
            iv_lrs = reader.ReadInt32();
            SmallTexUpdated = reader.ReadInt32();
        }
    }
}
