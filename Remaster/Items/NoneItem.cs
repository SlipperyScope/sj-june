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

        public override ItemAnimationData Animation(String key) => Animations[key];

        public override ItemDescription Description(String querier) => querier switch
        {
            _ => new ItemDescription("There is no item.")
        };

        #region Animations
        private Dictionary<String, ItemAnimationData> Animations = new Dictionary<string, ItemAnimationData>
        {
            {
                "HudWindowIdle", new ItemAnimationData
                {
                    TexturePath = "res://Remaster/Assets/Items/None_SubWindow_InOut.png",
                    SoundEffectPath = String.Empty,
                    AnimationRow = 0,
                    AnimationFrames = (0, 0),
                    Repeat = false,
                    Time = 0.1f,
                    GridSize = (1, 5)
                }
            },
            {
                "HudWindowIn", new ItemAnimationData
                {
                    TexturePath = "res://Remaster/Assets/Items/None_SubWindow_InOut.png",
                    SoundEffectPath = String.Empty,
                    AnimationRow = 0,
                    AnimationFrames = (4, 0),
                    Repeat = false,
                    Time = 0.1f,
                    GridSize = (1, 5)
                }
            },
            {
                "HudWindowOut", new ItemAnimationData
                {
                    TexturePath = "res://Remaster/Assets/Items/None_SubWindow_InOut.png",
                    SoundEffectPath = String.Empty,
                    AnimationRow = 0,
                    AnimationFrames = (0, 4),
                    Repeat = false,
                    Time = 0.1f,
                    GridSize = (1, 5)
                }
            }
        };
        #endregion
    }
}
