using Remaster.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster.Items
{
    /// <summary>
    /// The none item, for when there isn't one
    /// </summary>
    public class NoneItem : Item
    {
        public override ItemID ID => ItemID.None;

        public override string Name => "None";

        public override ItemAnimationData Animation(String key) => key switch
        {
            HudWindowIdle => new ItemAnimationData
            {
                TexturePath = "res://Remaster/Assets/Items/None_SubWindow_InOut.png",
                SoundEffectPath = String.Empty,
                AnimationRow = 0,
                AnimationFrames = (0, 0),
                Repeat = true,
                Time = 0.1f,
                GridSize = (1, 6)
            },
            HudWindowIn => new ItemAnimationData
            {
                TexturePath = "res://Remaster/Assets/Items/None_SubWindow_InOut.png",
                SoundEffectPath = String.Empty,
                AnimationRow = 0,
                AnimationFrames = (5, 0),
                Repeat = false,
                Time = 0.1f,
                GridSize = (1, 6)
            },
            HudWindowOut => new ItemAnimationData
            {
                TexturePath = "res://Remaster/Assets/Items/None_SubWindow_InOut.png",
                SoundEffectPath = String.Empty,
                AnimationRow = 0,
                AnimationFrames = (0, 5),
                Repeat = false,
                Time = 0.1f,
                GridSize = (1, 6)
            },
            _ => new ItemAnimationData
            {
                TexturePath = "res://Remaster/Assets/Items/None_SubWindow_InOut.png",
                SoundEffectPath = String.Empty,
                AnimationRow = 0,
                AnimationFrames = (0, 0),
                Repeat = true,
                Time = 0.1f,
                GridSize = (1, 6)
            },
        };

        public override ItemDescription Description(String querier) => querier switch
        {
            _ => new ItemDescription("There is no item.")
        };
    }
}
