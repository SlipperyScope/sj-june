using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Remaster.HUD
{
    public class InteriorCamera : Camera2D
    {
        //TODO: Account for when camera is not centered on HUD
        [Export]
        private NodePath HUDSpritePath;
        private Sprite HUD;
        private Vector2 CameraLimits;

        private Single UpDistance;
        private Single DownDistance;
        private Single LeftDistance;
        private Single RightDistance;

        private Vector2 ViewportSize => GetViewportRect().Size;
        private Vector2 StartingPosition;

        public override void _Ready()
        {
            HUD = GetNode<Sprite>(HUDSpritePath);
            var halfSize = HUD.GetRect().Size / 2f;
            var upperleft = HUD.Position - halfSize;
            var camHalfSize = ViewportSize * Zoom / 2f;
            StartingPosition = Position;

            var hudRect = HUD.GetRect();

            UpDistance = (Position.y - camHalfSize.y) - hudRect.Position.y;
            DownDistance = hudRect.End.y - (Position.y + camHalfSize.y);
            LeftDistance = (Position.x - camHalfSize.x) - hudRect.Position.x;
            RightDistance = hudRect.End.x - (Position.x + camHalfSize.x);

            CameraLimits = (HUD.GetRect().Size - ViewportSize * Zoom) / 2f;
        }

        public override void _Input(InputEvent e)
        {
            if (e is InputEventMouseMotion mouseMotion)
            {
                var mousePosition = 2f * (mouseMotion.Position / ViewportSize - new Vector2(0.5f, 0.5f));
                var offset = Vector2.Zero;
                offset.x = (mousePosition.x > 0f ? RightDistance : -LeftDistance) * CameraInterp(Math.Abs(mousePosition.x));
                offset.y = (mousePosition.y > 0f ? DownDistance : -UpDistance) * CameraInterp(Math.Abs(mousePosition.y));
                Position = StartingPosition + offset;
            }
        }

        /// <summary>
        /// Interpolates alpha (0..1) on an inout cubic easing curve
        /// </summary>
        /// <param name="alpha">Position along curve, 0 to 1</param>
        /// <returns>Interpolated value</returns>
        public static Single CameraInterp(Single alpha) => (Single)(-(Math.Cos(Math.PI * alpha) - 1d) / 2d);
        /*alpha < 0.5f
        //    ? 4f * (Single)Math.Pow(alpha, 3d)
        //    : (1f - (Single)Math.Pow(-2d * alpha + 2d, 3d) / 2f);
        {
            if (alpha == 0 || alpha == 1)
            {
                return alpha;
            }

            return alpha < 0.5f
                            ? (Single)Math.Pow(2d, 80d * alpha - 40d) / 2f
                            : (Single)(2d - Math.Pow(2d, -80d * alpha + 40d)) / 2f;
        }
        */
    }
}
