using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Remaster.Utilities;

namespace Remaster.Items
{
    /// <summary>
    /// Container for item description information
    /// TODO: Audio? 
    /// </summary>
    public struct ItemDescription : IPrintable
    {
        public List<PrintBlock> Blocks;

        /// <summary>
        /// Creates a description from a printblock list
        /// </summary>
        public ItemDescription(List<PrintBlock> blocks)
        {
            Blocks = blocks ?? new List<PrintBlock>();
        }

        /// <summary>
        /// Creates a description from printblocks
        /// </summary>
        public ItemDescription(PrintBlock block, params PrintBlock[] args)
        {
            Blocks = new List<PrintBlock>();
            Blocks.Add(block);
            foreach (var printblock in args)
            {
                Blocks.Add(printblock);
            }
        }

        public List<PrintBlock> PrintBlocks => Blocks;
    }

    public struct ItemAnimationData
    {
        public String TexturePath;
        public (Int32 Rows, Int32 Columns) GridSize;
        public String SoundEffectPath;
        public Int32 AnimationRow;
        public (Int32 start, Int32 end) AnimationFrames;
        public Boolean Repeat;
        public Texture Texture => GD.Load<Texture>(TexturePath);
        public AudioStreamMP3 SoundEffect => GD.Load<AudioStreamMP3>(SoundEffectPath);
        public Single Time;

        public override String ToString() => $"{TexturePath}\nRow {AnimationRow} frames {AnimationFrames} from sheetsize [{GridSize}] in {Time} seconds\nAudio: {SoundEffectPath}"; 
    }
}
