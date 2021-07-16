using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Remaster.Items
{
    /// <summary>
    /// Container for item description information
    /// TODO: Audio? 
    /// </summary>
    public struct ItemDescription
    {
        [Obsolete]
        public String Text;
        public List<PrintBlock> Blocks;

        /// <summary>
        /// Creates a description from a printblock list
        /// </summary>
        public ItemDescription(List<PrintBlock> blocks)
        {
            Text = String.Empty;
            Blocks = blocks ?? new List<PrintBlock>();
        }

        /// <summary>
        /// Creates a description from printblocks
        /// </summary>
        public ItemDescription(PrintBlock block, params PrintBlock[] args)
        {
            Text = String.Empty;
            Blocks = new List<PrintBlock>();
            Blocks.Add(block);
            foreach (var printblock in args)
            {
                Blocks.Add(printblock);
            }
        }
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
    }

    /// <summary>
    /// Block of printable data
    /// </summary>
    public struct PrintBlock
    {
        public PrintToken Token;
        public String Text;

        /// <summary>
        /// Creates a block of printable data
        /// </summary>
        /// <param name="text">
        /// <para>Text: String of text
        /// <br />Pause: Length of pause
        /// <br />Image: Path to texture resource
        /// <br />Clear: N/A
        /// </para></param>
        /// <param name="token">Block type</param>
        public PrintBlock(String text, PrintToken token = PrintToken.Text)
        {
            Token = token;
            Text = text;
        }

        public PrintBlock(PrintToken token, String text = "")
        {
            Token = token;
            Text = text;
        }
    }
}
