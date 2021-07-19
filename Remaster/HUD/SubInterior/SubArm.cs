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
            Animator = GetNode<AnimationPlayer>("AnimationPlayer");
            Animator.Connect("animation_finished", this, nameof(OnArmAnimationFinished));
            ItemSprite = GetNode<SpriteAnimator>("Claw/Item");
            ItemSprite.AnimationComplete += OnItemAnimationFinished;
            Item = new NoneItem();
        }

        /// <summary>
        /// Processes the animation queue
        /// </summary>
        private void ProcessQueue()
        {
            GD.Print($"[{OS.GetTicksMsec() / 1000f}] Processing {(AnimationQueue.Count > 0 ? AnimationQueue.Peek() : "EMPTY")}");
            if (AnimationQueue.Count == 0)
            {
                if (CurrentOperation == SubArmAction.Output)
                {
                    GD.Print($"[{OS.GetTicksMsec() / 1000f}] Changing animation to idle");
                    ItemSprite.ChangeAnimationAndStop(Item.Animation(rItem.HudWindowIdle));
                    ItemSprite.StartAnimation();
                }

                Busy = false;
                var operation = CurrentOperation;
                CurrentOperation = SubArmAction.None;
                GD.Print($"[{OS.GetTicksMsec() / 1000f}] Dispatching operation complete");
                OperationComplete?.Invoke(this, new ArmOperationCompleteEventArgs(operation, Item, Extended, ClawOpen));
                return;
            }

            var animation = AnimationQueue.Dequeue();
            
            switch (animation)
            {
                case ItemInTakeAnimation:
                    ItemSprite.ChangeAnimationAndStop(Item.Animation(rItem.HudWindowOut));
                    ItemSprite.StartAnimation();
                    break;
                case ItemOutPutAnimation:
                    GD.Print($"[{OS.GetTicksMsec() / 1000f}] Starting timer before output");
                    NewTimer();
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
            if (Busy is true)
            {
                GD.PushWarning($"[{OS.GetTicksMsec() / 1000f}] INTAKE: Tried to intake but was busy");
                return;
            }
            GD.Print($"[{OS.GetTicksMsec() / 1000f}]  INTAKE: Taking in item; queueing animations");
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
            if (Busy is true)
            {
                GD.PushWarning($"[{OS.GetTicksMsec() / 1000f}] OUTPUT: Tried to output {Item} but was busy");
                return false;
            }

            GD.Print($"[{OS.GetTicksMsec() / 1000f}] OUTPUT: Changing item to {item}");

            ItemSprite.ChangeAnimationAndStop(item.Animation(rItem.HudWindowIn));
            Item = item;
            CurrentOperation = SubArmAction.Output;

            GD.Print($"[{OS.GetTicksMsec() / 1000f}] OUTPUT: Queing animation");
            QueueAnimation(ClawOpenAnimation, ItemOutPutAnimation);

            return true;
        }

        public rItem ForceChangeItem(rItem item)
        {
            var oldItem = Item;
            Item = item;
            ItemSprite.AnimationData = Item.Animation(rItem.HudWindowIdle);
            return oldItem;
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

        private void OnArmAnimationFinished(String animationName)
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

        private void OnItemAnimationFinished(System.Object sender, EventArgs e)
        {
            if (CurrentOperation == SubArmAction.Intake)
            {
                GD.Print($"[{OS.GetTicksMsec() / 1000f}] INTAKE: Starting timer");
                NewTimer();
            }

            if (CurrentOperation == SubArmAction.Output)
            {
                GD.Print($"[{OS.GetTicksMsec() / 1000f}] OUTPUT: Animation complete, processing remaining queue");
                ProcessQueue();
            }
        }

        private void OnTransitComplete()
        {
            GD.Print($"TIMEOUT");

            if (CurrentOperation == SubArmAction.Intake)
            {
                GD.Print($"[{OS.GetTicksMsec() / 1000f}] INTAKE: Timer complete, processing remaining queue");
                ProcessQueue();
            }
            else if (CurrentOperation == SubArmAction.Output)
            {
                GD.Print($"[{OS.GetTicksMsec() / 1000f}] OUTPUT: Timer complete, starting animation");
                ItemSprite.ChangeAnimationAndStop(Item.Animation(rItem.HudWindowIn));
                ItemSprite.StartAnimation();
            }
        }

        /// <summary>
        /// Dumb workaround hack for timer not working the second time
        /// </summary>
        private void NewTimer(Boolean start = true)
        {
            if (TransitTimer != null)
            {
                RemoveChild(TransitTimer);
            }
            TransitTimer = new Timer();
            AddChild(TransitTimer);
            TransitTimer.WaitTime = TransitTime;
            TransitTimer.OneShot = true;
            TransitTimer.Connect("timeout", this, nameof(OnTransitComplete));
            if (start is true)
            {
                TransitTimer.Start();
            }
        }


        //public override void _Process(Single delta)
        //{
        //    if (TransitTimer != null && TransitTimer.TimeLeft > 0)
        //    {
        //        GD.Print($"Time left {TransitTimer.TimeLeft}");
        //    }
        //}
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
