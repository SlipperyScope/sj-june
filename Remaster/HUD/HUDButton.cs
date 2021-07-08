﻿using System;
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
        /// Ready
        /// </summary>
        public sealed override void _Ready()
        {
            Ready();
        }
        protected virtual void Ready() { }
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

    }
}