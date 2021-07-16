using Remaster.HUD;
using Remaster.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster.Items
{
    public class Seaweed : Item
    {
        public const String HudWindowIdle = "HudWindowIdle";
        public const String HudWindowIn = "HudWindowIn";
        public const String HudWindowOut = "HudWindowOut";

        public override ItemID ID => ItemID.Seaweed;
        public override string Name => "Seaweed";

        public override ItemDescription Description(String key) => key switch
        {
            nameof(SubConsole) => SubConsole_Description,
            _ => Default_Description
        };

        public override ItemAnimationData Animation(String key) => Animations[key];

        #region Descriptions
        private ItemDescription SubConsole_Description => new ItemDescription
        (
            new PrintBlock(PrintToken.Clear),
            new PrintBlock("Seaweed:\n"),
            new PrintBlock(PrintToken.Pause, "1"),
            new PrintBlock("It's like lettuce, but salty.")
        );
        #endregion

        #region Animations
        private Dictionary<String, ItemAnimationData> Animations = new Dictionary<String, ItemAnimationData>
        {
            {
                HudWindowIdle, new ItemAnimationData
                {
                    TexturePath = "res://Remaster/Assets/Items/SeaweedItem_SubWindow_Idle.png",
                    SoundEffectPath = String.Empty,
                    AnimationRow = 0,
                    AnimationFrames = (0 , 7),
                    Repeat = true,
                    Time = 1f,
                    GridSize = (1, 8)
                }
            },
            {
                HudWindowIn, new ItemAnimationData
                {
                    TexturePath = "res://Remaster/Assets/Items/SeaweedItem_SubWindow_InOut.png",
                    SoundEffectPath = String.Empty,
                    AnimationRow = 0,
                    AnimationFrames = (0 , 4),
                    Repeat = false,
                    Time = 0.2f,
                    GridSize = (1, 5)
                }
            },
            {
                HudWindowOut, new ItemAnimationData
                {
                    TexturePath = "res://Remaster/Assets/Items/SeaweedItem_SubWindow_InOut.png",
                    SoundEffectPath = String.Empty,
                    AnimationRow = 0,
                    AnimationFrames = (4 , 0),
                    Repeat = false,
                    Time = 0.2f,
                    GridSize = (1, 5)
                }
            }
        };
        #endregion
    }
}
