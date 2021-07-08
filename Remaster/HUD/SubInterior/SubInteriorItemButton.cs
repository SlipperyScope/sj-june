using Godot;
using System;

namespace Remaster.HUD
{
    public class SubInteriorItemButton : HUDButton
    {
        /// <summary>
        /// Whether the button is enabled, functionally and visibly
        /// </summary>
        public Boolean Enabled
        {
            get => _Enabled;
            set
            {
                if (value != _Enabled)
                {
                    _Enabled = value;
                    //Animator?.Play(value is true ? "Rollout" : "Disable");
                    Animate(Light, LightTween, 0.1f, value);
                    Animate(RingLight, RingLightTween, 0.25f, true);
                }
            }
        }

        [Export]
        private Boolean _Enabled = true;

        [Export]
        private NodePath RingLightPath;

        [Export]
        private NodePath DomePath;

        [Export]
        private NodePath LightPath;

        private Sprite RingLight;
        private Sprite Dome;
        private Sprite Light;

        private Tween RingLightTween;
        private Tween DomeTween;
        private Tween LightTween;

        /// <summary>
        /// Ready
        /// </summary>
        protected override void Ready()
        {
            RingLight = GetNode<Sprite>(RingLightPath);
            Dome = GetNode<Sprite>(DomePath);
            Light = GetNode<Sprite>(LightPath);

            RingLightTween = new Tween();
            DomeTween = new Tween();
            LightTween = new Tween();

            AddChild(RingLightTween);
            AddChild(DomeTween);
            AddChild(LightTween);

            MouseEvent += OnMouseEvent;

            Light.Frame = Enabled is true ? 0 : 4;
            Dome.Frame = 0;
            RingLight.Frame = 0;

            //if (_Enabled is false)
            //{
            //    Animator.Play("Disable");
            //}
        }

        /// <summary>
        /// Plays appropriate animation on a mouse event
        /// </summary>
        private void OnMouseEvent(object sender, ClickableMouseEventArgs e)
        {
            if (Enabled is true)
            {
                if (e.MouseState == MouseEventType.Over)
                {
                    Animate(RingLight, RingLightTween, 0.25f);
                }
                else if (e.MouseState == MouseEventType.Out)
                {
                    Animate(RingLight, RingLightTween, 0.25f, true);
                }
            }
            //if (Enabled is false) return;

            //GD.Print($"{e.MouseState} {e.Message}");

            //Animator.Play(e.MouseState switch
            //{
            //    MouseEventType.Up => "Release",
            //    MouseEventType.Over => "Rollover",
            //    MouseEventType.Down => "Click",
            //    MouseEventType.Out => "Rollout",
            //    _ => throw new NotImplementedException()
            //});

            if (Enabled is true && e.MouseState == MouseEventType.Up)
            {
                PressButton();
            }
        }

        public override void OnMouseDown() => Animate(Dome, DomeTween, 0.25f);
        public override void OnMouseUp() => Animate(Dome, DomeTween, 0.25f, true);

        private void Animate(Sprite sprite, Tween animator, Single duration, Boolean reverse = false)
        {
            animator.Stop(sprite);
            var total = sprite.Hframes;
            var start = sprite.Frame;
            var end = reverse is true ? 0 : (total - 1);
            if (start == end) return;
            var frames = Math.Abs(end - start);
            var time = duration * frames / total;
            animator.InterpolateProperty(sprite, "frame", start, end, time);
            animator.Start();
        }
    }
}
