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

        public void ChangeItem(rItem item, Boolean AnimateIn = false, Boolean AnimateOut = false)
        {

            if (AnimateOut is true)
            {
                AnimatedItem.AnimationComplete += OnAnimateOutComplete;
                AnimatedItem.AnimationData = Item.Animation("HudWindowOut");
                Item = item; 
                return;
            }

            Item = item;

            if (AnimateIn is true)
            {
                AnimatedItem.AnimationComplete += OnAnimateInComplete;
                AnimatedItem.AnimationData = Item.Animation("HudWindowIn");
                return;
            }

            AnimatedItem.AnimationData = Item.Animation("HudWindowIdle");
        }

        private void OnAnimateOutComplete(object sender, EventArgs e)
        {
            AnimatedItem.AnimationComplete -= OnAnimateOutComplete;
            AnimatedItem.AnimationComplete += OnAnimateInComplete;
            AnimatedItem.AnimationData = Item.Animation("HudWindowIn");
        }

        private void OnAnimateInComplete(object sender, EventArgs e)
        {
            AnimatedItem.AnimationComplete -= OnAnimateInComplete;
            AnimatedItem.AnimationData = Item.Animation("HudWindowIdle");
        }
    }
}
