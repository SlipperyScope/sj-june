using Godot;
using System;
using Remaster.Items;
using rItem = Remaster.Items.Item;

namespace Remaster.HUD
{
    public class ItemWindow : Clickable
    {
        private const String ItemSpritePath = "Item";

        public delegate void ItemWindowEventHandler(System.Object sender, ItemWindowEventArgs e);
        public event ItemWindowEventHandler ItemWindowEvent;

        /// <summary>
        /// True if the item window is busy
        /// </summary>
        public Boolean Busy { get; private set; }

        /// <summary>
        /// Item currently in the window
        /// </summary>
        public rItem Item { get; private set; } 

        /// <summary>
        /// Item sprite reference
        /// </summary>
        private SpriteAnimator Sprite;

        /// <summary>
        /// Ready
        /// </summary>
        protected override void OnReady()
        {
            Sprite = GetNode<SpriteAnimator>(ItemSpritePath);
        }

        /// <summary>
        /// Expels and item from the bay
        /// </summary>
        /// <returns>True if expel is possible</returns>
        public Boolean Output()
        {
            if (Busy is false)
            {
                Busy = true;

                Sprite.AnimationComplete += OnAnimateOutComplete;
                Sprite.AnimationData = Item.Animation(rItem.HudWindowOut);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Intakes an item
        /// </summary>
        /// <param name="item">Item to intake</param>
        /// <returns>True if intake is possible</returns>
        public Boolean Intake(rItem item)
        {
            if (Busy is false)
            {
                Busy = true;

                Item = item;
                Sprite.AnimationData = item.Animation(rItem.HudWindowIn);
                Sprite.AnimationComplete += OnAnimateInComplete;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Forces an item change without animations
        /// </summary>
        /// <param name="item">Item to change to</param>
        /// <returns>Item currently in bay</returns>
        public rItem ForceChangeItem(rItem item)
        {
            var oldItem = Item;
            Item = item;
            Sprite.AnimationData = item.Animation(rItem.HudWindowIdle);
            return oldItem;
        }

        /// <summary>
        /// Window is clicked
        /// </summary>
        protected override void OnMouseDown() => ItemWindowEvent?.Invoke(this, new ItemWindowEventArgs(Item, ItemWindowEventType.Click, Index));

        /// <summary>
        /// On Animate Out Complete
        /// </summary>
        private void OnAnimateOutComplete(object sender, EventArgs e)
        {
            Busy = false;

            Sprite.AnimationComplete -= OnAnimateOutComplete;
            ItemWindowEvent?.Invoke(this, new ItemWindowEventArgs(Item, ItemWindowEventType.Expel, Index));
        }

        /// <summary>
        /// On Animate In Complete
        /// </summary>
        private void OnAnimateInComplete(object sender, EventArgs e)
        {
            Busy = false;

            Sprite.AnimationComplete -= OnAnimateInComplete;
            Sprite.AnimationData = Item.Animation(rItem.HudWindowIdle);
            ItemWindowEvent?.Invoke(this, new ItemWindowEventArgs(Item, ItemWindowEventType.Intake, Index));
        }
    }
    
    /// <summary>
    /// Item window event args
    /// </summary>
    public class ItemWindowEventArgs : EventArgs
    {
        /// <summary>
        /// Item relevent to event
        /// </summary>
        public readonly rItem Item;

        /// <summary>
        /// Type of event
        /// </summary>
        public readonly ItemWindowEventType Type;

        /// <summary>
        /// Index of item window
        /// </summary>
        public readonly Int32 Index;

        /// <summary>
        /// Creates a new item window event arg
        /// </summary>
        /// <param name="item">Item relevent to event</param>
        /// <param name="type">Type of event</param>
        /// <param name="index">Index of item window</param>
        public ItemWindowEventArgs(rItem item, ItemWindowEventType type, Int32 index)
        {
            Item = item;
            Type = type;
            Index = index;
        }
    }
}
