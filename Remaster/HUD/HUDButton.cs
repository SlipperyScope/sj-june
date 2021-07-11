using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Remaster.HUD
{ 
    /// <summary>
    /// Button intended to be used in HUDs
    /// </summary>
    public class HUDButton : Clickable
    {
        const String ButtonSpritePath = "./ButtonSprite";

        [Export]
        public HUDButtonType Type { get; private set; } = HUDButtonType.Item;

        public delegate void OnButtonPressEventHandler(object sender, ButtonEventArgs e);
        public event OnButtonPressEventHandler ButtonPress;

        /// <summary>
        /// Path to animation node
        /// </summary>
        [Export]
        public NodePath AnimatorPath { get; private set; }
        protected AnimationPlayer Animator;

        protected Sprite ButtonSprite;

        /// <summary>
        /// Initiailze Sprites
        /// </summary>
        private void GetNodes()
        {
            ButtonSprite = GetNode<Sprite>(ButtonSpritePath);
            Animator = GetNode<AnimationPlayer>(AnimatorPath);
        }

        /// <summary>
        /// Invokes button pressed event
        /// </summary>
        public void PressButton()
        {
            ButtonPress?.Invoke(this, new ButtonEventArgs());
        }
    }

    public class ButtonEventArgs
    {
        public HUDButtonType Type { get; private set; }

        public ButtonEventArgs(HUDButtonType type = HUDButtonType.None)
        {
            Type = type;
        }
    }
}