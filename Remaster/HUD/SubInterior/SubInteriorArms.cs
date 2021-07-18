using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using rItem = Remaster.Items.Item;

namespace Remaster.HUD
{
    public class SubInteriorArms : Node2D
    {
        public delegate void OperationEventHandler(System.Object sender, ArmOperationEventArgs e);
        public event OperationEventHandler OperationEvent;

        public Boolean Busy { get; private set; }
        public Boolean RightArmOpen => RightArm.ClawOpen;
        public Boolean RightArmExtended => RightArm.Extended; 

        private SubArm RightArm;
        private SubArm LeftArm;

        public override void _Ready()
        {
            RightArm = GetNode<SubArm>("Right");
            RightArm.OperationComplete += ArmOperationComplete;
        }

        public Boolean Use(SubArmSide side = SubArmSide.Right)
        {
            if (Busy is true) return false;
            Busy = true;

            var arm = RightArm;

            if (arm.ClawOpen is true)
            {
                arm.Close();
            }
            else
            {
                arm.Open();
            }

            return true;
        }

        public Boolean Move(SubArmSide side = SubArmSide.Right)
        {
            if (Busy is true) return false;
            Busy = true;

            var arm = RightArm;

            if (arm.Extended is true)
            {
                arm.Park();
            }
            else
            {
                arm.Extend();
            }

            return true;
        }

        private void ArmOperationComplete(System.Object sender, ArmOperationCompleteEventArgs e)
        {
            Busy = false;
        }

        public Boolean SwapItem(rItem item, SubArmSide side = SubArmSide.Right)
        {
            if (Busy is true) return false;
            Busy = true;

            var arm = side == SubArmSide.Right ? RightArm : LeftArm;

            

            return true;
        }

        public override void _Input(InputEvent e)
        {
            if (e is InputEventKey keyEvent && keyEvent.Pressed is true)
            {
                switch ((KeyList)keyEvent.Scancode)
                {
                    case KeyList.Kp6 when RightArmExtended is true:
                        RightArm.Park();
                        break;
                    case KeyList.Kp6 when RightArmExtended is false:
                        RightArm.Extend();
                        break;
                    case KeyList.Kp9 when RightArmOpen is true:
                        RightArm.Close();
                        break;
                    case KeyList.Kp9 when RightArmOpen is false:
                        RightArm.Open();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public class ArmOperationEventArgs : EventArgs
    {
        public readonly ArmOperationEvent State;
        public readonly SubArmSide Side;
    }
}
