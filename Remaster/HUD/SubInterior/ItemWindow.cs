using Godot;
using System;
using Remaster.Items;
using GameItem = Remaster.Items.Item;

namespace Remaster.HUD
{
    public class ItemWindow : Clickable
    {
        [Export]
        private ItemID _Item = ItemID.None;

        public GameItem Item { get; private set; }

        /// <summary>
        /// Item texture
        /// </summary>
        [Export]
        public Texture Texture
        {
            get => AnimatedItem?.Texture ?? _Texture;
            set
            {
                AnimatedItem?.StopAnimation();
                _Texture = value;
                if (AnimatedItem != null)
                {
                    AnimatedItem.Texture = value;
                    AnimatedItem.StartAnimation();
                }
            }
        }
        private Texture _Texture;

        /// <summary>
        /// Item animation frames
        /// </summary>
        [Export]
        public Int32 Frames 
        {
            get => AnimatedItem?.Hframes ?? _Frames;
            set
            {
                AnimatedItem?.StopAnimation();
                _Frames = value;
                if (AnimatedItem != null)
                {
                    AnimatedItem.Hframes = value;
                    AnimatedItem.StartAnimation();
                }
            }
        }
        private Int32 _Frames = 1;

        /// <summary>
        /// Path to item sprite
        /// </summary>
        private NodePath ItemPath = new NodePath("Item");

        /// <summary>
        /// Item sprite reference
        /// </summary>
        private SpriteAnimator AnimatedItem;

        /// <summary>
        /// Tween for animations
        /// </summary>
        private Tween Animator;

        /// <summary>
        /// Enter Tree
        /// </summary>
        protected override void OnEnterTree()
        {
            Animator = new Tween();
            AddChild(Animator);
        }

        /// <summary>
        /// Ready
        /// </summary>
        protected override void OnReady()
        {
            Item = GameItem.GetItem(_Item);
            AnimatedItem = GetNode<SpriteAnimator>(ItemPath);
            Frames = _Frames;
            Texture = _Texture;
        }
    } 
}
