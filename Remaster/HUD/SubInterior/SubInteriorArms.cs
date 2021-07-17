using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Remaster.HUD
{
    public class SubInteriorArms : Node2D
    {
        public Boolean RightArmOpen => RightArm.ClawOpen;
        public Boolean RightArmExtended => RightArm.Extended; 

        private SubArm RightArm;

        private readonly Queue<SubArmAction> ActionQueue = new Queue<SubArmAction>();

        public override void _Ready()
        {
            RightArm = GetNode<SubArm>("Right");
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
}
