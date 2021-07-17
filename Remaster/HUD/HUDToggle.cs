using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster.HUD
{
    public class HUDToggle : Clickable
    {
        private const String FrameProperty = "frame";

        public delegate void ToggleEventHandler(System.Object sender, ToggleEventArgs e);
        public event ToggleEventHandler Toggled;

        [Export]
        public Boolean On { get; private set; }

        private Tween Tweener = new Tween();
        private Sprite Handle;

        public Boolean Enabled
        {
            get => _Enabled;
            set
            {
                if (value != _Enabled)
                {
                    _Enabled = value;
                    OnMouseOut();
                }
            }
        }
        [Export]
        private Boolean _Enabled = true;

        protected override void OnEnterTree()
        {
            AddChild(Tweener);
        }

        protected override void OnReady()
        {
            Handle = GetNode<Sprite>("Sprite");
            Handle.Frame = On is true ? 0 : Handle.Hframes - 1;
        }

        protected override void OnMouseOver() => Animate(Handle, On is true ? 1 : Handle.Hframes - 2);
        protected override void OnMouseOut() => Animate(Handle, On is true ? 0 : Handle.Hframes - 1);
        protected override void OnMouseDown()
        {
            On = !On;
            OnMouseOver();
            Toggled?.Invoke(this, new ToggleEventArgs(On));
        }

        private void Animate(Sprite sprite, Int32 TweenTo, Single duration = 0.2f)
        {
            if (Enabled is false) return;
            if (sprite is null) return;

            var total = sprite.Hframes;
            var start = sprite.Frame;

            Tweener.Remove(sprite, FrameProperty);
            if (start == TweenTo) return;

            var frames = Math.Abs(TweenTo - start);
            var time = duration * frames / total;

            Tweener.InterpolateProperty(sprite, FrameProperty, start, TweenTo, time);
            Tweener.Start();
        }
    }

    public class ToggleEventArgs : EventArgs
    {
        public readonly Boolean On;
        public readonly String Message;

        public ToggleEventArgs(Boolean isOn) : this(isOn, String.Empty) { }

        public ToggleEventArgs(Boolean isOn, String message)
        {
            On = isOn;
            Message = message;
        }
    }
}
