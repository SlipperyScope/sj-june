using Godot;
using System;

namespace Remaster.HUD
{
    public class SubConsole : Clickable, IRegisterClickables
    {
        private const String TextBoxPath = "Textbox";
        private RichTextLabel TextBox;

        public void Register(Clickable clickable)
        {
            if (clickable is ItemWindow window)
            {
                window.MouseEvent += Window_MouseEvent;
            }
        }

        private void Window_MouseEvent(object sender, ClickableMouseEventArgs e)
        {
            if (sender is ItemWindow window && e.MouseState == MouseEventType.Down)
            {
                TextBox.Text += $"\n{window.Item.Description(nameof(SubConsole)).Text}";
            }
        }

        /// <summary>
        /// Ready
        /// </summary>
        protected override void OnReady()
        {
            TextBox = GetNode<RichTextLabel>(TextBoxPath);
        }
    }
}

