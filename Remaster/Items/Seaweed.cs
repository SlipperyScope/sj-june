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
            "SubConsole" => new ItemDescription { Text = SubConDesc},
            _ => new ItemDescription { Text = Name }
        };

        #region Descriptions
        private String SubConDesc => $"Seaweed:\n{Ps}It's like lettuce, but salty.";
        #endregion
    }
}
