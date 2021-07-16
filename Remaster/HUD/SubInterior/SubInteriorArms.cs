using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Remaster.HUD
{
    public class SubInteriorArms : Node2D
    {
        private const String FrameProperty = "frame";

        private Sprite LeftArm;
        private Sprite RightArm;

        private Tween Animator = new Tween();

        private Queue<ArmAction> ActionQueue = new Queue<ArmAction>();

        public override void _EnterTree()
        {
            AddChild(Animator);
            Animator.Connect("tween_all_completed", this, nameof(AnimationFinished));
        }

        public override void _Ready()
        {
            LeftArm = GetNode<Sprite>("Left");
            RightArm = GetNode<Sprite>("Right");
            
            LeftArm.Frame = 0;
            RightArm.Frame = 0;
        }

        public void QueueAction(ArmAction action, params ArmAction[] actions)
        {
            actions.Prepend(action).ToList().ForEach(action => ActionQueue.Enqueue(action));
            if (Animator.IsActive() is false && ActionQueue.Count > 0)
            {
                ProcessAction();
            }
        }

        private void ProcessAction()
        {
            var action = ActionQueue.Peek();
            switch (ActionQueue.Dequeue())
            {
                case ArmAction.LeftExtend:
                    Animate(LeftArm);
                    break;
                case ArmAction.LeftPark:
                    Animate(LeftArm, true);
                    break;
                case ArmAction.RightExtend:
                    Animate(RightArm);
                    break;
                case ArmAction.RightPark:
                    Animate(RightArm, true);
                    break;
                default:
                    break;
            }

        }

        private void AnimationFinished()
        {
            if (ActionQueue.Count > 0)
            {
                ProcessAction();
            }
        }

        private void Animate(Sprite sprite, Boolean reverse = false, Single duration = 0.5f)
        {
            if (sprite is null) return;

            var total = sprite.Hframes;
            var start = sprite.Frame;
            var end = reverse is true ? 0 : (total - 1);

            Animator.Remove(sprite, FrameProperty);
            if (start == end) return;

            var frames = Math.Abs(end - start);
            var time = duration * frames / total;

            Animator.InterpolateProperty(sprite, FrameProperty, start, end, time);
            Animator.Start();
        }
    }

    public enum ArmAction
    {
        LeftExtend,
        LeftPark,
        RightExtend,
        RightPark
    }
}
