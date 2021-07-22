using Remaster.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster.Items
{
    /// <summary>
    /// Base class for items
    /// </summary>
    public abstract class Item : IPrintable
    {

        public const String HudWindowIdle = "HudWindowIdle";
        public const String HudWindowIn = "HudWindowIn";
        public const String HudWindowOut = "HudWindowOut";
        public const String ItemClawOutPut = "ItemClawOutput";
        public const String ItemClawIdle = "ItemClawIdle";

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

        public abstract ItemAnimationData Animation(String querier);

        public static Item GetItem(ItemID id) => id switch
        {
            ItemID.Pipe => new Pipe(),
            ItemID.Seaweed => new Seaweed(),
            _ => new NoneItem()
        };

        protected ItemDescription Default_Description => new ItemDescription(Name);

        public List<PrintBlock> PrintBlocks => Description(String.Empty).Blocks;
    }
}
