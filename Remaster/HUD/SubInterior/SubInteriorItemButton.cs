using Godot;
using System;

namespace Remaster.HUD
{
    public class SubInteriorItemButton : HUDButton
    {
        const String Frame = "frame";
        const Single LightTime = 0.1f;
        const Single RingLightTime = 0.6f;
        const Single DomeTime = 0.3f;

        const String RingLightPath = "LightRing";
        const String DomePath = "Bulb";
        const String LightPath = "Light";

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
                    Animate(Light, LightTime, value);
                    Animate(RingLight, RingLightTime, true);
                }
            }
        }

        [Export]
        private Boolean _Enabled = true;

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

            Light.Frame = Enabled is true ? 0 : 4;
            Dome.Frame = 0;
            RingLight.Frame = 0;
        }

        /// <summary>
        /// Mouse click
        /// </summary>
        protected override void OnMouseDown() => Animate(Dome, DomeTime);

        /// <summary>
        /// Mouse unclick
        /// </summary>
        protected override void OnMouseUp()
        {
            Animate(Dome, DomeTime, true);

            if (Enabled is true)
            {
                PressButton();
            }
        }

        /// <summary>
        /// Mouse over
        /// </summary>
        protected override void OnMouseOver()
        {
            if (Enabled is true)
            {
                Animate(RingLight, RingLightTime);
            }
        }

        /// <summary>
        /// Mouse out
        /// </summary>
        protected override void OnMouseOut()
        {
            if (Enabled is true)
            {
                Animate(RingLight, RingLightTime, true);
                Animate(Dome, DomeTime, true);
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

            Tweener.Remove(sprite, Frame);
            if (start == end) return;
            
            var frames = Math.Abs(end - start);
            var time = duration * frames / total;
            
            Tweener.InterpolateProperty(sprite, Frame, start, end, time);
            Tweener.Start();
        }
    }
}
