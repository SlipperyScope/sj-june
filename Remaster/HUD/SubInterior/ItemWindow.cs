using Godot;
using System;
using Remaster.Items;
using rItem = Remaster.Items.Item;

namespace Remaster.HUD
{
    public class ItemWindow : Clickable
    {
        const String ItemSpritePath = "Item";

        public rItem Item { get; private set; }

        /// <summary>
        /// Item sprite reference
        /// </summary>
        private SpriteAnimator AnimatedItem;

        /// <summary>
        /// Ready
        /// </summary>
        protected override void OnReady()
        {
            AnimatedItem = GetNode<SpriteAnimator>(ItemSpritePath);
        }

        public rItem ChangeItem(rItem item, Boolean AnimateIn = false, Boolean AnimateOut = false)
        {
            var currentItem = Item;
            if (AnimateOut is true)
            {
                AnimatedItem.AnimationComplete += OnAnimateOutComplete;
                AnimatedItem.AnimationData = Item.Animation(rItem.HudWindowOut);
                Item = item; 
                return currentItem;
            }

            Item = item;

            if (AnimateIn is true)
            {
                AnimatedItem.AnimationComplete += OnAnimateInComplete;
                AnimatedItem.AnimationData = Item.Animation(rItem.HudWindowIn);
                return currentItem;
            }

            AnimatedItem.AnimationData = Item.Animation(rItem.HudWindowIdle);
            return currentItem;
        }

        private void OnAnimateOutComplete(object sender, EventArgs e)
        {
            AnimatedItem.AnimationComplete -= OnAnimateOutComplete;
            AnimatedItem.AnimationComplete += OnAnimateInComplete;
            AnimatedItem.AnimationData = Item.Animation(rItem.HudWindowIn);
        }

        private void OnAnimateInComplete(object sender, EventArgs e)
        {
            AnimatedItem.AnimationComplete -= OnAnimateInComplete;
            AnimatedItem.AnimationData = Item.Animation(rItem.HudWindowIdle);
        }
    }
}
