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
        public override ItemID ID => ItemID.Seaweed;
        public override string Name => "Seaweed";

        public override ItemDescription Description(String querier) => querier switch
        {
            nameof(SubConsole) => SubConsole_Description,
            _ => Default_Description
        };

        public override ItemAnimationData Animation(string querier)
        {
            throw new NotImplementedException();
        }

        #region Descriptions
        private ItemDescription SubConsole_Description => new ItemDescription
        (
            new PrintBlock("Seaweed:\n"),
            new PrintBlock("1", PrintToken.Pause),
            new PrintBlock("It's like lettuce, but salty.")
        );
        #endregion
    }
}
