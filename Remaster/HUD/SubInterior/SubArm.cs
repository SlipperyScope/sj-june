using Godot;
using System;
using rItem = Remaster.Items.Item;

namespace Remaster.HUD
{
    /// <summary>
    /// Controls a sub arm
    /// </summary>
    public class SubArm : Node2D
    {
        /// <summary>
        /// Current arm state
        /// </summary>
        public SubArmState State { get; private set; } = SubArmState.Parked;

        /// <summary>
        /// Whether the arm is currently in an animation
        /// </summary>
        public Boolean Animating { get; private set; } = false;

        /// <summary>
        /// Currently held item
        /// </summary>
        public rItem Item { get; private set; } = null;


    }
}
