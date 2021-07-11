using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster.Items
{
    public abstract class Item
    {
        public static String Ps = "`pause`";
        public static String Pl = "`pause_long`";

        /// <summary>
        /// Items ID
        /// </summary>
        public abstract ItemID ID { get; }

        /// <summary>
        /// Items name
        /// </summary>
        public abstract String Name { get; }

        /// <summary>
        /// Item descriptions
        /// </summary>
        /// <param name="querier">Entity requesting a description</param>
        /// <returns></returns>
        public abstract ItemDescription Description(String querier);
    }
}
