using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Remaster.HUD;

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

        public override ItemAnimationData Animation(string querier)
        {
            return new ItemAnimationData
            {
                TexturePath = "res://Remaster/Assets/Items/Pipe.png",
                SoundEffectPath = String.Empty,
                AnimationRow = 0,
                AnimationFrames = (0, 0),
                Repeat = false,
                Time = .1f,
                GridSize = (1, 1)
            };
        }

        #region Descriptions
        private ItemDescription SubConsole_Description => new ItemDescription
        (
            new PrintBlock("A pipe:\n"),
            new PrintBlock("1", PrintToken.Pause),
            new PrintBlock("Good for beating off"),
            new PrintBlock("0.5", PrintToken.Pause),
            new PrintBlock("."),
            new PrintBlock("0.5", PrintToken.Pause),
            new PrintBlock("."),
            new PrintBlock("0.5", PrintToken.Pause),
            new PrintBlock("."),
            new PrintBlock("2", PrintToken.Pause),
            new PrintBlock("barnicles.")
        );
        #endregion
    }
}
