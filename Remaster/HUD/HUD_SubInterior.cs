using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Remaster.Items;
using Remaster.Player;

namespace Remaster.HUD
{
    public class HUD_SubInterior : HUDBase, IRegisterClickables
    {
        [Export]
        private String DataKey = "Submarine Data";

        private SubmarineData Data
        {
            get
            {
                var data = Globals.Global.GetPlayerData(DataKey);
                if (data is null)
                {
                    Globals.Global.AddPlayerData(DataKey, new SubmarineData());
                    data = Globals.Global.GetPlayerData(DataKey);
                }
                return data as SubmarineData;
            }
        }

        [Export]
        private List<NodePath> ItemButtonPaths = new List<NodePath>();
        [Export]
        private List<NodePath> ItemWindowPaths = new List<NodePath>();

        private List<SubInteriorItemButton> ItemButtons = new List<SubInteriorItemButton>();
        private List<ItemWindow> ItemWindows = new List<ItemWindow>();

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            ItemButtonPaths.ForEach(p => ItemButtons.Add(GetNode<SubInteriorItemButton>(p)));
            ItemWindowPaths.ForEach(p => ItemWindows.Add(GetNode<ItemWindow>(p)));

            ItemButtons.ForEach(b => b.ButtonPress += OnItemButtonPressed); 

            for (var i = 0; i < Data.Items.Count; i++)
            {
                ItemWindows[i].ChangeItem(Data.Items[i]);
                //ItemButtons[i].Enabled = Data.Items[i].ID != ItemID.None;
            }
        }

        private void OnItemButtonPressed(object sender, ButtonEventArgs e)
        {
            ItemWindows[ItemButtons.IndexOf(sender as SubInteriorItemButton)].ChangeItem(new Seaweed(), true, true);
        }

        #region IRegisterClickables
        public void Register(Clickable clickable)
        {
            //switch (clickable)
            //{
            //    case SubInteriorItemButton itemButton:
            //        itemButton.ButtonPress += ItemButtonPressed;
            //        break;
            //    case ItemWindow itemWindow:
            //        itemWindow.MouseEvent += ItemWindow_MouseEvent;
            //        break;
            //    default:
            //        break;
            //}
        } 
        #endregion
    }
}
