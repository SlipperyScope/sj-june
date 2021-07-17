using Godot;
using System;
using Remaster.Items;
using rItem = Remaster.Items.Item;

namespace Remaster.HUD
{
    /// <summary>
    /// Item arm of sub (right arm)
    /// </summary>
    public class SubArm : Node2D
    {
        private const String ArmExtendAnimation = "Arm_Extend";
        private const String ArmParkAnimation = "Arm_Park";
        private const String ClawOpenAnimation = "Claw_Open";
        private const String ClawCloseAnimation = "Claw_Close";

        public delegate void AnimationCompleteHandler(System.Object sender, EventArgs e);
        public event AnimationCompleteHandler AnimationComplete;

        public Boolean Extended = false;
        public Boolean ClawOpen = false;

        /// <summary>
        /// Whether the arm is currently in an animation
        /// </summary>
        public Boolean Animating { get; private set; } = false;

        /// <summary>
        /// Currently held item
        /// </summary>
        public rItem Item
        {
            get => _Item;
            private set
            {
                _Item = value;
                var animation = value.Animation(rItem.HudWindowIdle);
                ItemSprite.Texture = animation.Texture;
                ItemSprite.Hframes = animation.GridSize.Columns;
                ItemSprite.Vframes = animation.GridSize.Rows;
                ItemSprite.FrameCoords = new Vector2(animation.AnimationRow, animation.AnimationFrames.end);
            }
        }
        private rItem _Item;

        private AnimationPlayer Animator;
        private Sprite ItemSprite;

        public override void _Ready()
        {
            Animator = GetNode<AnimationPlayer>("AnimationPlayer");
            Animator.Connect("animation_finished", this, nameof(OnAnimationFinished));
            ItemSprite = GetNode<Sprite>("Claw/Item");
            Item = new NoneItem();
        }

        public void Extend()
        {
            StartAnimation(ArmExtendAnimation);
        }

        public void Park()
        {
            StartAnimation(ArmParkAnimation);
        }

        public void Open()
        {
            StartAnimation(ClawOpenAnimation);
        }

        public void Close()
        {
            StartAnimation(ClawCloseAnimation);
        }

        private Boolean StartAnimation(String animationName)
        {
            var animated = false;

            if (Animator.IsPlaying() is false)
            {
                Animator.Play(animationName);
                animated = true;
                Animating = true;
            }

            return animated;
        }

        private void OnAnimationFinished(String animationName)
        {
            switch (animationName)
            {
                case ArmExtendAnimation:
                    Extended = true;
                    break;
                case ArmParkAnimation:
                    Extended = false;
                    break;
                case ClawOpenAnimation:
                    ClawOpen = true;
                    break;
                case ClawCloseAnimation:
                    ClawOpen = false;
                    break;
                default:
                    break;
            }
            Animating = false;
            AnimationComplete?.Invoke(this, new EventArgs());
        }

        public void ChangeItem(ItemWindow bay)
        {
            Item = bay.ChangeItem(Item, true, true);
        }
    }
}
