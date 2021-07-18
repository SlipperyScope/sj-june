using Godot;
using System;
using Remaster.Items;
using rItem = Remaster.Items.Item;
using System.Collections.Generic;
using System.Linq;

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

        public delegate void OperationCompleteHandler(System.Object sender, ArmOperationCompleteEventArgs e);
        public event OperationCompleteHandler OperationComplete;

        public Boolean Extended = false;
        public Boolean ClawOpen = false;

        /// <summary>
        /// Whether the arm is currently in an animation
        /// </summary>
        public Boolean Busy { get; private set; } = false;

        public SubArmAction CurrentOperation = SubArmAction.None;

        /// <summary>
        /// Currently held item
        /// </summary>
        public rItem Item
        {
            get => _Item;
            private set
            {
                _Item = value;
                ItemSprite.AnimationData = value.Animation(rItem.HudWindowIdle);
            }
        }
        private rItem _Item;

        /// <summary>
        /// Item to be swapped in
        /// </summary>
        private rItem PendingItem;

        private AnimationPlayer Animator;
        private SpriteAnimator ItemSprite;

        private Queue<String> AnimationQueue = new Queue<String>();

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            Animator = GetNode<AnimationPlayer>("AnimationPlayer");
            Animator.Connect("animation_finished", this, nameof(OnAnimationFinished));
            ItemSprite = GetNode<SpriteAnimator>("Claw/Item");
            Item = new NoneItem();
        }

        /// <summary>
        /// Processes the animation queue
        /// </summary>
        private void ProcessQueue()
        {
            if (AnimationQueue.Count == 0)
            {
                switch (CurrentOperation)
                {
                    case SubArmAction.Push:
                        break;
                    case SubArmAction.Pull:

                        break;
                    default:
                        Busy = false;
                        OperationComplete?.Invoke(this, new ArmOperationCompleteEventArgs(CurrentOperation, Item, Extended, ClawOpen));
                        CurrentOperation = SubArmAction.None;
                        break;
                }

                return;
            }

            var animation = AnimationQueue.Dequeue();

            switch (animation)
            {
                case ClawOpenAnimation when ClawOpen is true:
                case ClawCloseAnimation when ClawOpen is false:
                case ArmExtendAnimation when Extended is true:
                case ArmParkAnimation when Extended is false:
                    ProcessQueue();
                    break;
                default:
                    StartAnimation(animation);
                    break;
            }
        }

        /// <summary>
        /// Queue and animation
        /// </summary>
        private void QueueAnimation(String animation, params String[] animations)
        {
            if (Busy is true) return;

            animations.Prepend(animation).ToList().ForEach(a => AnimationQueue.Enqueue(a));
            Busy = true;
            ProcessQueue();
        }

        /// <summary>
        /// Extend the arm
        /// </summary>
        public void Extend()
        {
            if (Busy is true) return;

            CurrentOperation = SubArmAction.Extend;
            QueueAnimation(ArmExtendAnimation);
        }

        /// <summary>
        /// Park the arm
        /// </summary>
        public void Park()
        {
            if (Busy is true) return;

            CurrentOperation = SubArmAction.Park;
            QueueAnimation(ArmParkAnimation);
        }

        /// <summary>
        /// Open the claw
        /// </summary>
        public void Open()
        {
            if (Busy is true) return;

            CurrentOperation = SubArmAction.Open;
            QueueAnimation(ClawOpenAnimation);
        }

        /// <summary>
        /// Close the claw
        /// </summary>
        public void Close()
        {
            if (Busy is true) return;

            CurrentOperation = SubArmAction.Close;
            QueueAnimation(ClawCloseAnimation);
        }

        /// <summary>
        /// Pull an item in
        /// </summary>
        public void Pull()
        {
            if (Busy is true) return;

            CurrentOperation = SubArmAction.Pull;
            QueueAnimation(ClawOpenAnimation);
        }

        /// <summary>
        /// Push and item out
        /// </summary>
        /// <param name="item">Item to push out</param>
        /// <returns>True if push is possible</returns>
        public Boolean Push(rItem item)
        {
            if (Busy is true || Item is NoneItem is false) return false;



            CurrentOperation = SubArmAction.Push;
            QueueAnimation(ClawOpenAnimation);

            return true;
        }

        private void OnPullAnimationComplete(System.Object sender, EventArgs e)
        {
            QueueAnimation(ClawCloseAnimation);
        }

        private Boolean StartAnimation(String animationName)
        {
            var animated = false;

            if (Animator.IsPlaying() is false)
            {
                Animator.Play(animationName);
                animated = true;
                Busy = true;
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

            ProcessQueue();
        }
    }

    public class ArmOperationCompleteEventArgs
    {
        public readonly SubArmAction Operation;
        public readonly rItem Item;
        public readonly Boolean Extended;
        public readonly Boolean Open;

        public ArmOperationCompleteEventArgs(SubArmAction operation, rItem item, Boolean extended, Boolean open)
        {
            Operation = operation;
            Item = item;
            Extended = extended;
            Open = open;
        }
    }
}
