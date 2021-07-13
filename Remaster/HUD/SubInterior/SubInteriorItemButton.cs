using Godot;
using System;

namespace Remaster.HUD
{
    public class SubInteriorItemButton : HUDButton
    {
        const String FRAME_PROPERTY = "frame";

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
                    Animate(Light, 0.1f, value);
                    Animate(RingLight, 0.25f, true);
                }
            }
        }

        [Export]
        private Boolean _Enabled = true;

        private String RingLightPath = "LightRing";
        private String DomePath = "Bulb";
        private String LightPath = "Light";

        private Sprite RingLight;
        private Sprite Dome;
        private Sprite Light;

        private Tween Tweener;

        /// <summary>
        /// Ready
        /// </summary>
        protected override void OnReady()
        {
            RingLight = GetNode<Sprite>(RingLightPath);
            Dome = GetNode<Sprite>(DomePath);
            Light = GetNode<Sprite>(LightPath);

            AddChild(Tweener = new Tween());

            //MouseEvent += OnMouseEvent;

            Light.Frame = Enabled is true ? 0 : 4;
            Dome.Frame = 0;
            RingLight.Frame = 0;
        }

        ///// <summary>
        ///// Plays appropriate animation on a mouse event
        ///// </summary>
        //private void OnMouseEvent(object sender, ClickableMouseEventArgs e)
        //{
        //    if (Enabled is true)
        //    {
        //        if (e.MouseState == MouseEventType.Over)
        //        {
        //            Animate(RingLight, 0.25f);
        //        }
        //        else if (e.MouseState == MouseEventType.Out)
        //        {
        //            Animate(RingLight, 0.25f, true);
        //            Animate(Dome, 0.25f, true);
        //        }
        //    }

        //    if (Enabled is true && e.MouseState == MouseEventType.Up)
        //    {
        //        PressButton();
        //    }
        //}

        protected override void OnMouseDown() => Animate(Dome, 0.25f);
        protected override void OnMouseUp()
        {
            Animate(Dome, 0.25f, true);

            if (Enabled is true)
            {
                PressButton();
            }
        }

        private Int32 Debug = 0;

        protected override void OnMouseOver()
        {
            if (Enabled is true)
            {
                Animate(RingLight, 0.25f);
            }
        }

        protected override void OnMouseOut()
        {
            if (Enabled is true)
            {
                GD.Print($"{Debug++} out");
                Animate(RingLight, 0.25f, true);
                Animate(Dome, 0.25f, true);
            }
        }

        /// <summary>
        /// Starts button animations
        /// </summary>
        /// <param name="sprite">Sprite to animate</param>
        /// <param name="duration">Animation duration</param>
        /// <param name="reverse">Play in reverse from end</param>
        private void Animate(Sprite sprite, Single duration, Boolean reverse = false)
        {
            if (sprite is null) return;

            var total = sprite.Hframes;
            var start = sprite.Frame;
            var end = reverse is true ? 0 : (total - 1);

            Tweener.Remove(sprite, FRAME_PROPERTY);
            if (start == end) return;
            
            var frames = Math.Abs(end - start);
            var time = duration * frames / total;
            
            Tweener.InterpolateProperty(sprite, FRAME_PROPERTY, start, end, time);
            Tweener.Start();
        }
    }
}
