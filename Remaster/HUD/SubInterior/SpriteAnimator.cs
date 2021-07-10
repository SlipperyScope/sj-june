using Godot;
using System;

namespace Remaster.HUD
{
    public class SpriteAnimator : Sprite
    {
        /// <summary>
        /// Frame parameter name
        /// </summary>
        const String FRAME = "frame";

        /// <summary>
        /// Animation tween reference
        /// </summary>
        private Tween Animator = new Tween();

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
        public void StartAnimation()
        {
            Animator.Stop(this, FRAME);

            if (Hframes == 1)
            {
                Frame = 0;
                return;
            }

            Animator.InterpolateProperty(this, FRAME, 0, Hframes - 1, 1f);
            Animator.Repeat = true;
            Animator.Start();
        }

        private void OnAnimationComplete(object obj, NodePath property)
        {
            StartAnimation();
        }

        /// <summary>
        /// Stops the animation
        /// </summary>
        public void StopAnimation()
        {
            Animator.Stop(this, FRAME);
        }
    } 
}
