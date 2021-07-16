using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Remaster.Items;
using Remaster.Player;
using rItem = Remaster.Items.Item;

namespace Remaster.HUD
{
    public class HUD_SubInterior : HUDBase
    {
        [Export]
        private String DataKey = "Submarine Data";

        /// <summary>
        /// Player data
        /// </summary>
        private SubmarineData PlayerData
        {
            get
            {
                var data = Globals.GetPlayerData(DataKey);
                if (data is null)
                {
                    Globals.AddPlayerData(DataKey, new SubmarineData());
                    data = Globals.GetPlayerData(DataKey);
                }
                return data as SubmarineData;
            }
        }

        private List<SubInteriorItemButton> ItemButtons = new List<SubInteriorItemButton>();
        private List<ItemWindow> ItemWindows = new List<ItemWindow>();

        private List<SubInteriorItemButton> ToolButtons = new List<SubInteriorItemButton>();
        private List<ItemWindow> ToolWindows = new List<ItemWindow>();

        private List<Clickable> Clickables;

        private SubConsole Console;

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            Clickables = GetTree().GetNodesInGroup("HUD_Clickable").Cast<Clickable>().ToList();
            Clickables.ForEach(c => c.MouseEvent += OnClickableMouseEvent);

            // Console
            Console = Clickables.First(c => c is SubConsole) as SubConsole;
            
            // Item Buttons
            Clickables.Where(clickable => clickable.IsInGroup("HUD_ItemButtons")).Cast<SubInteriorItemButton>().ToList().ForEach(button => button.ButtonPress += OnItemButtonPressed);

            // Item Windows
            foreach (var window in Clickables.Where(clickable => clickable.IsInGroup("HUD_ItemWindows")).Cast<ItemWindow>())
            {
                window.ChangeItem(PlayerData.Items[window.Index]);
            }

            // Tool Buttons
            // Tool Windows
        }

        private void OnClickableMouseEvent(object sender, ClickableMouseEventArgs e)
        {
            if (e.MouseState == MouseEventType.Down)
            {
                switch (sender)
                {
                    case ItemWindow window:
                        Console.Print(window.Item);
                        break;
                    default:
                        Console.Print(sender);
                        break;
                }
            }
        }

        private void OnItemButtonPressed(object sender, ButtonEventArgs e)
        {
            //ItemWindows[ItemButtons.IndexOf(sender as SubInteriorItemButton)].ChangeItem(new Seaweed(), true, true);
        }
    }
}
