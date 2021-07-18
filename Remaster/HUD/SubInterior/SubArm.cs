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
        private const String ItemOutPutAnimation = "Item_Output";
        private const String ItemInTakeAnimation = "Item_Intake";

        public delegate void OperationCompleteHandler(System.Object sender, ArmOperationCompleteEventArgs e);
        public event OperationCompleteHandler OperationComplete;

        /// <summary>
        /// True is the arm is extended
        /// </summary>
        public Boolean Extended { get; private set; }  = false;

        /// <summary>
        /// True if the claw is open
        /// </summary>
        public Boolean ClawOpen { get; private set; } = false;

        /// <summary>
        /// Current operation
        /// </summary>
        public SubArmAction CurrentOperation { get; private set; }

        /// <summary>
        /// Whether the arm is currently in an animation
        /// </summary>
        public Boolean Busy { get; private set; } = false;

        /// <summary>
        /// Currently held item
        /// </summary>
        public rItem Item { get; private set; }

        private AnimationPlayer Animator;
        private SpriteAnimator ItemSprite;
        private Timer TransitTimer;

        [Export]
        private Single TransitTime = 0.5f;

        private Queue<String> AnimationQueue = new Queue<String>();

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            AddChild(TransitTimer = new Timer());
            TransitTimer.WaitTime = TransitTime;
            TransitTimer.Connect("timeout", this, nameof(OnTransitComplete));

            Animator = GetNode<AnimationPlayer>("AnimationPlayer");
            Animator.Connect("animation_finished", this, nameof(OnAnimationFinished));
            ItemSprite = GetNode<SpriteAnimator>("Claw/Item");
            ItemSprite.AnimationComplete += OnItemAnimationComplete;
            Item = new NoneItem();
        }


        /// <summary>
        /// Processes the animation queue
        /// </summary>
        private void ProcessQueue()
        {
            if (AnimationQueue.Count == 0)
            {
                if (CurrentOperation == SubArmAction.Output)
                { 
                    ItemSprite.AnimationData = Item.Animation(rItem.HudWindowIdle);
                }

                Busy = false;
                var operation = CurrentOperation;
                CurrentOperation = SubArmAction.None;
                OperationComplete?.Invoke(this, new ArmOperationCompleteEventArgs(operation, Item, Extended, ClawOpen));
                return;
            }

            var animation = AnimationQueue.Dequeue();

            switch (animation)
            {
                case ItemInTakeAnimation:
                    ItemSprite.AnimationData = Item.Animation(rItem.HudWindowOut);
                    Busy = true;
                    break;
                case ItemOutPutAnimation:
                    ItemSprite.AnimationData = Item.Animation(rItem.HudWindowIn);
                    Busy = true;
                    break;
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
        public void Intake()
        {
            if (Busy is true) return;
            CurrentOperation = SubArmAction.Intake;
            QueueAnimation(ClawOpenAnimation, ItemInTakeAnimation);
        }

        /// <summary>
        /// Push and item out
        /// </summary>
        /// <param name="item">Item to push out</param>
        /// <returns>True if push is possible</returns>
        public Boolean Output(rItem item)
        {
            if (Busy is true) return false;

            Item = item;
            CurrentOperation = SubArmAction.Output;
            QueueAnimation(ClawOpenAnimation, ItemOutPutAnimation);//, ClawCloseAnimation);

            return true;
        }

        public rItem ForceChangeItem(rItem item)
        {
            var oldItem = Item;
            Item = item;
            ItemSprite.AnimationData = item.Animation(rItem.HudWindowIdle);
            return oldItem;
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

        private void OnItemAnimationComplete(System.Object sender, EventArgs e)
        {
            if (CurrentOperation == SubArmAction.Output || CurrentOperation == SubArmAction.Intake)
            {
                TransitTimer.Start();
                //ProcessQueue();
            }
        }

        private void OnTransitComplete()
        {
            if (CurrentOperation == SubArmAction.Output || CurrentOperation == SubArmAction.Intake)
            {
                ProcessQueue();
            }
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
