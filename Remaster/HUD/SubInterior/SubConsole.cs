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
            switch (clickable)
            {
                case ItemWindow window:
                    window.MouseEvent += OnRegisteredMouseEvent;
                    break;
                case SubConsole console:
                    console.MouseEvent += OnRegisteredMouseEvent;
                    break;
            }
        }

        private void OnRegisteredMouseEvent(object sender, ClickableMouseEventArgs e)
        {
            switch (sender)
            {
                case ItemWindow window when e.MouseState == MouseEventType.Down:
                    TextBox.Text += $"{window.Item.Description(nameof(SubConsole)).Text}\n";
                    break;
                case Clickable clickable when e.MouseState == MouseEventType.Down:
                    TextBox.Text += $"{clickable.Description}\n";
                    break;
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

