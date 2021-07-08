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

        protected List<HUDButton> ToolButtons = new List<HUDButton>();

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
        }

        public override void _Input(InputEvent @event)
        {
            
        }

        private void ItemButtonPressed(object sender, ButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }

        
        //
    }
}
