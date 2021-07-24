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
        public rItem RequestItem();

        public ItemDescription Response(rItem item);
    }
}
