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
            nameof(SubConsole) => new ItemDescription { Text = SubConsole_desc},
            _                  => new ItemDescription { Text = Name }
        };

        public override ItemAnimationData Animation(string querier)
        {
            throw new NotImplementedException();
        }

        #region Descriptions
        private String SubConsole_desc => $"A pipe:\n{Ps}Good for beating off{Ps}.{Ps}.{Ps}.{Pl} barnicles."; 
        #endregion
    }
}
