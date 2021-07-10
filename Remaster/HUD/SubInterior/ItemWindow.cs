using Godot;
using System;

namespace Remaster.HUD
{
    public class ItemWindow : Node2D
    {
        /// <summary>
        /// Item texture
        /// </summary>
        [Export]
        public Texture Texture
        {
            get => Item?.Texture ?? _Texture;
            set
            {
                Item?.StopAnimation();
                _Texture = value;
                if (Item != null)
                {
                    Item.Texture = value;
                    Item.StartAnimation();
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
            get => Item?.Hframes ?? _Frames;
            set
            {
                Item?.StopAnimation();
                _Frames = value;
                if (Item != null)
                {
                    Item.Hframes = value;
                    Item.StartAnimation();
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
        private SpriteAnimator Item;

        /// <summary>
        /// Tween for animations
        /// </summary>
        private Tween Animator;

        /// <summary>
        /// Enter Tree
        /// </summary>
        public override void _EnterTree()
        {
            Animator = new Tween();
            AddChild(Animator);
        }

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            Item = GetNode<SpriteAnimator>(ItemPath);
            Frames = _Frames;
            Texture = _Texture;
        }
    } 
}
