using Godot;
using Remaster.Items;
using Remaster.Utilities;
using System;
using System.Collections.Generic;

namespace Remaster
{
    /// <summary>
    /// Base class for clickable areas
    /// </summary>
    public class Clickable : Area2D, IPrintable
    {
        public delegate void ClickableMouseEventHandler(object sender, ClickableMouseEventArgs e);
        public event ClickableMouseEventHandler MouseEvent;

        /// <summary>
        /// Names of groups to be added to
        /// </summary>
        [Export]
        private List<String> Groups;

        [Export]
        public Int32 Index { get; private set; }

        //TODO: Remove this? 
        /// <summary>
        /// Nodes to register with
        /// </summary>
        [Export]
        private List<NodePath> Listeners;

        /// <summary>
        /// Simple description of the clickable
        /// </summary>
        [Export(PropertyHint.MultilineText)]
        public String Description { get; private set; } = "";

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

        #region IPrintable
        public virtual List<PrintBlock> PrintBlocks => Description.Length > 0 ? new List<PrintBlock> { Description } : null;
        #endregion

        protected Boolean _Active = true;

        /// <summary>
        /// Enter tree
        /// </summary>
        public sealed override void _EnterTree()
        {
            if (Groups?.Count > 0)
            {
                Groups.ForEach(name => AddToGroup(name));
            }

            InputPickable = _Active;
            Monitorable = false;
            Monitoring = false;
            Connect("mouse_entered", this, nameof(MouseEntered));
            Connect("mouse_exited", this, nameof(MouseExited));
            Connect("input_event", this, nameof(InputEvent));
            OnEnterTree();
        }

        protected virtual void OnEnterTree() { }

        /// <summary>
        /// Ready
        /// </summary>
        public sealed override void _Ready()
        {
            //GD.Print($"Listeners: {Listeners}");
            //Listeners?.ForEach(path => GD.Print($"{this}={Name} {GetNodeOrNull<IRegisterClickables>(path)}"));

            Listeners?.ForEach(path => GetNodeOrNull<IRegisterClickables>(path)?.Register(this));
            OnReady();
        }

        protected virtual void OnReady() { }

        private Int32 DebugCounter = 0;

        public sealed override void _Process(Single delta)
        {
            
            OnProcess(delta);
        }

        protected virtual void OnProcess(Single delta) { }

        /// <summary>
        /// Mouse entered signal emitted
        /// </summary>
        private void MouseEntered()
        {
            if (Active is false) return;

            if (_ClickState == MouseButtonState.Up && Input.IsActionPressed("click"))
            {
                _ClickState = MouseButtonState.Down;
            }
            else if (_ClickState == MouseButtonState.Down && Input.IsActionPressed("click") is false)
            {
                _ClickState = MouseButtonState.Up;
            }

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
        /// Input event signal emitted
        /// </summary>
        private void InputEvent(Node viewport, InputEvent e, Int32 ShapeIndex)
        {
            if (Active is false) return;

            if (e.IsActionPressed("click") && ClickState != MouseButtonState.Down)
            {
                _ClickState = MouseButtonState.Down;
                OnMouseDown();
                MouseEvent?.Invoke(this, new ClickableMouseEventArgs(MouseEventType.Down, "Down"));
                //GD.Print($"{DebugCounter++} Clicka clicka boom boom");
            }

            if (e.IsActionReleased("click") && ClickState != MouseButtonState.Up)
            {
                DebugCounter = 0;
                _ClickState = MouseButtonState.Up;
                OnMouseUp();
                MouseEvent?.Invoke(this, new ClickableMouseEventArgs(MouseEventType.Up, "Up"));
            }
        }

        /// <summary>
        /// Called when the mouse clicks in the clickable area
        /// </summary>
        protected virtual void OnMouseDown() { }

        /// <summary>
        /// Called when the mouse releases in the clickable area
        /// </summary>
        protected virtual void OnMouseUp() { }

        /// <summary>
        /// Called when the mouse moves over the clickable area
        /// </summary>
        protected virtual void OnMouseOver() { }

        /// <summary>
        /// Called when the mouse leaves the clickable area
        /// </summary>
        protected virtual void OnMouseOut() { }
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
