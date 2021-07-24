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
        #region Animation constants
        public const String HudWindow_Idle = nameof(HudWindow_Idle);
        public const String HudWindow_In = nameof(HudWindow_In);
        public const String HudWindow_Out = nameof(HudWindow_Out);
        public const String ItemClaw_Output = nameof(ItemClaw_Output);
        public const String ItemClaw_Intake = nameof(ItemClaw_Intake);
        public const String ItemClaw_Idle = nameof(ItemClaw_Idle); 
        #endregion

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
