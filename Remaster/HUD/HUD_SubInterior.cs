using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Remaster.HUD
{
    public class HUD_SubInterior : HUDBase, IRegisterClickables
    {
        private List<SubInteriorItemButton> ItemButtons = new List<SubInteriorItemButton>();
        private List<SubInteriorItemButton> ToolButtons = new List<SubInteriorItemButton>();

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            
        }

        public override void _Input(InputEvent @event)
        {
            
        }

        private void HUDButtonPressed(object sender, ButtonEventArgs e)
        {
            switch (e.Type)
            {
                case HUDButtonType.None:
                    break;
                case HUDButtonType.Item:
                    break;
                case HUDButtonType.Tool:
                    break;
                default:
                    break;
            }
        }

        #region IRegisterClickables
        public void Register(Clickable clickable)
        {
            switch (clickable)
            {
                case SubInteriorItemButton itemButton:
                    itemButton.ButtonPress += HUDButtonPressed;
                    break;
                case ItemWindow itemWindow:
                    itemWindow.MouseEvent += ItemWindow_MouseEvent;
                    break;
                default:
                    break;
            }
        } 
        #endregion

        private void ItemWindow_MouseEvent(object sender, ClickableMouseEventArgs e)
        {
            if (e.MouseState == MouseEventType.Down)
            {
                GD.Print($"Clicked {sender as ItemWindow}");
            }
        }
    }
}
