using Godot;
using Remaster.Items;
using Remaster.Utilities;
using System.Collections.Generic;
using System;

namespace Remaster.HUD
{
    public class SubConsole : Clickable, IRegisterClickables
    {
        private const String TextBoxPath = "Textbox";
        private TextboxPrinter TextBox;

        public void Register(Clickable clickable)
        {
            //GD.Print($"{this}={Name} registering {clickable.Name}");
            clickable.MouseEvent += OnRegisteredMouseEvent;
            //switch (clickable)
            //{
                //case ItemWindow window:
                //    window.MouseEvent += OnRegisteredMouseEvent;
                //    break;
                //case SubConsole console:
                //    console.MouseEvent += OnRegisteredMouseEvent;
                //    break;
            //}
        }

        private void OnRegisteredMouseEvent(object sender, ClickableMouseEventArgs e)
        {
            switch (sender)
            {
                case ItemWindow window when e.MouseState == MouseEventType.Down:
                    TextBox.GoBrrr(window.Item?.Description(nameof(SubConsole)).Blocks ?? new List<PrintBlock> { new PrintBlock(window.Description) });
                    break;
                case Clickable clickable when e.MouseState == MouseEventType.Down:
                    TextBox.GoBrrr(new PrintBlock(clickable.Description));
                    break;
            }
        }

        /// <summary>
        /// Ready
        /// </summary>
        protected override void OnReady()
        {
            TextBox = GetNode<TextboxPrinter>(TextBoxPath);
        }
    }
}

