using Remaster.HUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster.Items
{
    public class Seaweed : Item
    {
        public const String HUDWINDOW = "HUDWINDOW";
        public override ItemID ID => ItemID.Seaweed;
        public override string Name => "Seaweed";

        public override ItemDescription Description(String key) => key switch
        {
            nameof(SubConsole) => SubConsole_Description,
            _ => Default_Description
        };

        public override ItemAnimationData Animation(String key) => key switch
        {
            HUDWINDOW => Animations[key],
            _ => throw new KeyNotFoundException($"No animation specified for {key}")
        };

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
                HUDWINDOW, new ItemAnimationData
                {
                    TexturePath = "res://Remaster/Assets/Items/Seaweed.png",
                    SoundEffectPath = String.Empty,
                    AnimationRow = 0,
                    AnimationFrames = (0 , 7),
                    Repeat = true
                }
            }
        };
        #endregion
    }
}
