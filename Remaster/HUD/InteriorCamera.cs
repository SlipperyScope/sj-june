using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Remaster.HUD
{
    /// <summary>
    /// Interior camera for the submarine
    /// 
    /// <para/>Assumes the default position is at coordinates (0,0)
    /// <para/>Compensates for camera's zoom level
    /// </summary>
    public class InteriorCamera : Camera2D
    {
        /// <summary>
        /// Camera views
        /// </summary>
        private enum CameraView
        {
            Window,
            Console
        }

        /// <summary>
        /// How far down the screen to shift to cockpit view
        /// </summary>
        [Export(PropertyHint.Range, "0,1,0.05")]
        private Single CockpitViewShiftPercent = 0.75f;

        /// <summary>
        /// Position of camera in console view
        /// </summary>
        [Export]
        private Vector2 ConsoleCameraPosition;

        /// <summary>
        /// How far the camera can extend past centered view in window mode
        /// </summary>
        [Export]
        private Rect2 WindowCameraRange;

        /// <summary>
        /// How far the camera can extend past centered view in console mode
        /// </summary>
        [Export]
        private Rect2 ConsoleCameraRange;

        /// <summary>
        /// Size of the viewport
        /// </summary>
        private Vector2 ViewportSize;

        /// <summary>
        /// Tween used to animate elements
        /// </summary>
        private Tween Animator;

        /// <summary>
        /// Current camera view
        /// </summary>
        private CameraView View = CameraView.Window;

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            ViewportSize = GetViewportRect().Size * Zoom;
            GD.Print(ConsoleCameraRange);
            Animator = new Tween();
            AddChild(Animator);
            Animator.Start();
        }

        /// <summary>
        /// Changes camera position based on mouse screen position
        /// </summary>
        public override void _Input(InputEvent e)
        {
            if (e is InputEventMouseMotion mouseMotion)
            {
                var mouseNormal = ScreenPercent(mouseMotion.Position);

                if (View == CameraView.Window && mouseNormal.y > CockpitViewShiftPercent)
                {
                    View = CameraView.Console;
                }
                else if (View == CameraView.Console && mouseNormal.y < 1f - CockpitViewShiftPercent)
                {
                    View = CameraView.Window;
                }

                var position = View switch { CameraView.Console => ConsoleCameraPosition, _ => Vector2.Zero };
                position += (new Vector2(CameraInterp(mouseNormal.x), CameraInterp(mouseNormal.y)) * 2f - Vector2.One) * ShiftFactor(mouseNormal);
                Position = position;
            }
        }

        /// <summary>
        /// Gets the position offset factor based on configured values
        /// </summary>
        /// <param name="position">normalized camera offset</param>
        /// <returns>Camera position</returns>
        public Vector2 ShiftFactor(Vector2 position)
        {
            var range = View switch { CameraView.Console => ConsoleCameraRange, _ => WindowCameraRange };
            var x = position.x == 0.5f ? 1f : (position.x < 0.5f ? range.Position.x : range.Size.x);
            var y = position.y == 0.5f ? 1f : (position.y < 0.5f ? range.Position.y : range.Size.y);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Remaps viewport coordinates to [-1..1]
        /// </summary>
        /// <param name="coordinates">viewport coordinates</param>
        /// <returns>[-1..1] where 0 is screen center</returns>
        private Vector2 ScreenPercent(Vector2 coordinates) => coordinates / ViewportSize;

        /// <summary>
        /// Interpolates alpha (0..1) on an inout sin easing curve
        /// </summary>
        /// <param name="alpha">Position along curve, 0 to 1</param>
        /// <returns>Interpolated value</returns>
        //public static Single CameraInterp(Single alpha) => alpha < 0.5f
        //                                                 ? 4f * (Single)Math.Pow(alpha, 3d)
        //                                                 : 1f - (Single)Math.Pow(-2d * alpha + 2d, 3d) / 2f;
        public static Single CameraInterp(Single alpha) => (Single)(Math.Cos(Math.PI * alpha) - 1) / -2f;
    }
}
