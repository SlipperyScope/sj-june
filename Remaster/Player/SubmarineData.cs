using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Remaster.Items;
using rItem = Remaster.Items.Item;

namespace Remaster.Player
{
    public class SubmarineData : PlayerData
    {
        /// <summary>
        /// Items available to the sub
        /// </summary>
        public List<rItem> Items { get; private set; } = new List<rItem>
        {
            new NoneItem(), new Pipe(), new NoneItem(), new Seaweed(), new NoneItem(), new NoneItem()
        };

        /// <summary>
        /// Tools available to the sub
        /// </summary>
        public List<rItem> Tools { get; private set; } = new List<rItem>
        {
            new NoneItem(), new NoneItem(), new NoneItem(), new NoneItem(), new NoneItem(), new NoneItem()
        };
    }
}
