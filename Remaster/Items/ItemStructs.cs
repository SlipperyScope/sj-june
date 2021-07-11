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
        public String Text;
    }

    public struct ItemAnimationData
    {
        public String TexturePath;
        public Texture Texture => GD.Load<Texture>(TexturePath);

        public String SoundEffectPath;
        public Int32 AnimationRow;
        public (Int32 start, Int32 end) AnimationFrames;
    }
}
