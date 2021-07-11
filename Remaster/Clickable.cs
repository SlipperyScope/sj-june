using Godot;
using System;

namespace Remaster
{
    /// <summary>
    /// Base class for clickable areas
    /// </summary>
    public class Clickable : Area2D
    {
        public delegate void ClickableMouseEventHandler(object sender, ClickableMouseEventArgs e);
        public event ClickableMouseEventHandler MouseEvent;

        /// <summary>
        /// Current state of the mouse click
        /// </summary>
        public MouseButtonState ClickState => _ClickState;
        private MouseButtonState _ClickState;

        /// <summary>
        /// Current state of the mouse position in relation to the clickable area
        /// </summary>
        public MouseHoverState HoverState => _HoverState;
        private MouseHoverState _HoverState;

        [Export]
        public virtual Boolean Active
        {
            get => _Active;
            set
            {
                _Active = value;
                InputPickable = value;
            }
        }
        protected Boolean _Active = true;

        /// <summary>
        /// Enter tree
        /// </summary>
        public sealed override void _EnterTree()
        {
            InputPickable = _Active;
            Connect("mouse_entered", this, nameof(MouseEntered));
            Connect("mouse_exited", this, nameof(MouseExited));
            OnEnterTree();
        }

        protected virtual void OnEnterTree() { }

        /// <summary>
        /// Input Event
        /// </summary>
        public sealed override void _InputEvent(Godot.Object viewport, InputEvent e, Int32 shapeIdx)
        {
            if (Input.IsActionJustPressed("click") && Active is true)
            {
                _ClickState = MouseButtonState.Down;
                OnMouseDown();
                MouseEvent?.Invoke(this, new ClickableMouseEventArgs(MouseEventType.Down, "Down"));

            }
            if (Input.IsActionJustReleased("click") && Active is true)
            {
                _ClickState = MouseButtonState.Up;
                OnMouseUp();
                MouseEvent?.Invoke(this, new ClickableMouseEventArgs(MouseEventType.Up, "Up"));
            }
        }

        /// <summary>
        /// Mouse entered signal emitted
        /// </summary>
        private void MouseEntered()
        {
            if (Active is false) return;

            _HoverState = MouseHoverState.Over;
            OnMouseOver();
            MouseEvent?.Invoke(this, new ClickableMouseEventArgs(MouseEventType.Over, "Over"));
        }

        /// <summary>
        /// Mouse exited signal emitted
        /// </summary>
        private void MouseExited()
        {
            if (Active is false) return;

            _HoverState = MouseHoverState.Out;
            OnMouseOut();
            MouseEvent?.Invoke(this, new ClickableMouseEventArgs(MouseEventType.Out, "Out"));
        }

        /// <summary>
        /// Called when the mouse clicks in the clickable area
        /// </summary>
        public virtual void OnMouseDown() { }

        /// <summary>
        /// Called when the mouse releases
        /// </summary>
        public virtual void OnMouseUp() { }

        /// <summary>
        /// Called when the mouse moves over the clickable area
        /// </summary>
        public virtual void OnMouseOver() { }

        /// <summary>
        /// Called when the mouse leaves the clickable area
        /// </summary>
        public virtual void OnMouseOut() { }
    }

    public class ClickableMouseEventArgs
    {
        public readonly MouseEventType MouseState;
        public readonly String Message;
        public ClickableMouseEventArgs(MouseEventType mouseState, String message)
        {
            MouseState = mouseState;
            Message = message;
        }
    }
}