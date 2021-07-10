using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Remaster.HUD
{
    public class HUD_SubInterior : HUDBase
    {
        [Export]
        private List<NodePath> ItemButtonPaths = new List<NodePath>();
        private List<SubInteriorItemButton> ItemButtons = new List<SubInteriorItemButton>();

        [Export]
        private List<NodePath> ToolButtonPaths = new List<NodePath>();
        private List<SubInteriorItemButton> ToolButtons = new List<SubInteriorItemButton>();

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            foreach (var path in ItemButtonPaths)
            {
                var button = GetNodeOrNull<SubInteriorItemButton>(path);
                if (button != null)
                {
                    ItemButtons.Add(button);
                    button.ButtonPress += ItemButtonPressed;
                }
            }

            foreach (var path in ToolButtonPaths)
            {
                var button = GetNodeOrNull<SubInteriorItemButton>(path);
                if (button != null)
                {
                    ToolButtons.Add(button);
                    button.ButtonPress += ToolButtonPressed;
                }
            }
        }

        public override void _Input(InputEvent @event)
        {
            
        }

        /// <summary>
        /// Equips item
        /// </summary>
        private void ItemButtonPressed(object sender, ButtonEventArgs e)
        {

        }

        /// <summary>
        /// Equips tool
        /// </summary>
        private void ToolButtonPressed(object sender, ButtonEventArgs e)
        {

        }
    }
}
