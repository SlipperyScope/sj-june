using Godot;
using Remaster.Items;
using System;

namespace Remaster.HUD
{
    public class SpriteAnimator : Sprite
    {
        public delegate void AnimationCompleteHandler(object sender, EventArgs e);
        public event AnimationCompleteHandler AnimationComplete;

        /// <summary>
        /// Frame parameter name
        /// </summary>
        const String FRAME = "frame";

        //[Export]
        //public Boolean AutoStart { get; set; } = true;

        /// <summary>
        /// Animation tween reference
        /// </summary>
        private Tween Animator = new Tween();

        /// <summary>
        /// Data about the animation
        /// </summary>
        public ItemAnimationData AnimationData
        {
            get => _AnimationData;
            set
            {
                _AnimationData = value;
                ChangeAnimation();
            }
        }
        private ItemAnimationData _AnimationData;

        /// <summary>
        /// Enter Tree
        /// </summary>
        public override void _EnterTree()
        {
            AddChild(Animator);
            Animator.Connect("tween_completed", this, nameof(OnAnimationComplete));
        }

        /// <summary>
        /// Start Animation
        /// </summary>
        public void StartAnimation() => ChangeAnimation();

        /// <summary>
        /// Called when animation is complete
        /// </summary>
        private void OnAnimationComplete(object obj, NodePath property)
        {
            if (Animator.Repeat is true)
            {
                ChangeAnimation();
            }
            else
            {
                AnimationComplete?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Stops the animation
        /// </summary>
        public void StopAnimation()
        {
            Animator.Stop(this, FRAME);
            AnimationComplete?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Changes an animation
        /// </summary>
        private void ChangeAnimation()
        {
            Texture = AnimationData.Texture;
            Hframes = AnimationData.GridSize.Columns;
            Vframes = AnimationData.GridSize.Rows;
            FrameCoords = new Vector2(AnimationData.AnimationFrames.start, AnimationData.AnimationRow);
            Animator.Stop(this, FRAME);
            Animator.Repeat = AnimationData.Repeat;
            Animator.InterpolateProperty(this, FRAME, AnimationData.AnimationFrames.start, AnimationData.AnimationFrames.end, AnimationData.Time);
            Animator.Start();
        }
    } 
}
