using Remaster.Characters;
using Remaster.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rItem = Remaster.Items.Item;

namespace Remaster
{
    public interface IHostItems
    {
        [Obsolete]
        /// <summary>
        /// Attempts to take an item from the item host
        /// </summary>
        /// <returns>Item or null</returns>
        public rItem RequestItem();

        [Obsolete]
        /// <summary>
        ///  Attempts to give an item to the item host
        /// </summary>
        /// <returns>True if item was received</returns>
        public Boolean GiveItem(rItem item);

        /// <summary>
        /// Exchange items
        /// </summary>
        /// <param name="item">Item to give</param>
        /// <returns>Item to receive</returns>
        public rItem Trade(rItem item);

        /// <summary>
        /// Gets the item host repsonse to an item 
        /// </summary>
        /// <param name="item">Item to respond to</param>
        /// <returns>Response</returns>
        public CharacterResponse Response(rItem item);
    }
}
