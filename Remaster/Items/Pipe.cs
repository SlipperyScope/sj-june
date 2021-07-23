using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Remaster.HUD;
using Remaster.Utilities;

namespace Remaster.Items
{
    public class Pipe : Item
    {
        public override ItemID ID => ItemID.Pipe;
        public override string Name => "Pipe";
        public override ItemDescription Description(String querier) => querier switch
        {
            nameof(SubConsole) => SubConsole_Description,
            _                  => Default_Description
        };

        public override ItemAnimationData Animation(string querier) => querier switch
        {
            HudWindow_Idle => new ItemAnimationData
            {
                TexturePath = "res://Remaster/Assets/Items/Pipe.png",
                SoundEffectPath = String.Empty,
                AnimationRow = 0,
                AnimationFrames = (0, 0),
                Repeat = true,
                Time = .1f,
                GridSize = (1, 1)
            },
            _ => new ItemAnimationData
            {
                TexturePath = "res://Remaster/Assets/Items/Pipe.png",
                SoundEffectPath = String.Empty,
                AnimationRow = 0,
                AnimationFrames = (0, 0),
                Repeat = false,
                Time = .1f,
                GridSize = (1, 1)
            }
        };

        #region Descriptions
        private ItemDescription SubConsole_Description => new ItemDescription
        (
            "A pipe:\n",
            (PrintToken.Pause, "1"),
            "Good for beating off",
            (PrintToken.Speed, "0.2"),
            "... ",
            (PrintToken.Pause, "1"),
            (PrintToken.Speed, "0.05"),
            "barnicles.",
            (PrintToken.Pause, "1"),
            (PrintToken.Speed, "0.02"),
            "\nHAHAHAHAHAHAHA ",
            (PrintToken.Pause, "0.5"),
            (PrintToken.Method, nameof(PrintMethod.Word)),
            (PrintToken.Speed, "0.5"),
            "GET IT LOL",
            (PrintToken.Speed, "0.05"),
            (PrintToken.Method, nameof(PrintMethod.Character))
        );
        #endregion
    }
}
