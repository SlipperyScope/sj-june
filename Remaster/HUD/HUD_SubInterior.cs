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
        #region Elements
        private readonly List<SubInteriorItemButton> ItemButtons = new SubInteriorItemButton[6].ToList();
        private readonly List<ItemWindow> ItemWindows = new ItemWindow[6].ToList();
        private readonly List<SubInteriorItemButton> ToolButtons = new SubInteriorItemButton[6].ToList();

        private SubConsole Console;

        private SubInteriorArms Arms;
        private SubInteriorItemButton RightArmButton;
        private HUDToggle RightArmToggle;

        /// <summary>
        /// Gets all the HUD elements and adds them to the Elements dictionary
        /// </summary>
        private void GetHUDElements()
        {
            Arms = GetNode<SubInteriorArms>("Arms");

            GetTree().GetNodesInGroup("HUD_Clickable").Cast<Clickable>().ToList().ForEach(clickable =>
            {
                switch (clickable)
                {
                    case SubInteriorItemButton rightArmButton when rightArmButton.Name == "RightClawButton":
                        RightArmButton = rightArmButton;
                        break;
                    case SubInteriorItemButton itemButton when itemButton.GetParent().Name == "ItemButtons":
                        GD.Print($"ItemButtons count {ItemButtons.Count}");
                        ItemButtons[itemButton.Index] = itemButton;
                        break;
                    case SubInteriorItemButton toolButton when toolButton.GetParent().Name == "ToolButons":
                        ToolButtons[toolButton.Index] = toolButton;
                        break;
                    case ItemWindow window:
                        ItemWindows[window.Index] = window;
                        break;
                    case SubConsole console:
                        Console = console;
                        break;
                    case HUDToggle toggle when toggle.Name == "RightArmButton":
                        RightArmToggle = toggle;
                        break;
                    default:
                        GD.PushWarning($"{this}={Name} unknown clickable {clickable.GetPath()}");
                        break;
                }
            });
        }
        #endregion

        /// <summary>
        /// Key to use for persistent player data
        /// </summary>
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

        public Boolean Busy { get; private set; }

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            GetHUDElements();

            PlayerData.Items.Select((item, index) => (item, index)).ToList().ForEach(d => ItemWindows[d.index].ChangeItem(d.item));

            foreach (var button in ItemButtons)
            {
                button.ButtonPress += OnItemButtonPressed;
            }

            foreach (var window in ItemWindows)
            {
                window.ItemChanged += OnWindowItemChanged;
            }

            RightArmButton.ButtonPress += OnRightArmButtonPressed;
            RightArmToggle.Toggled += OnRightArmToggled;
        }

        private void OnRightArmToggled(System.Object sender, ToggleEventArgs e)
        {
            if (Busy is false)
            {
                Arms.Move(SubArmSide.Right);
            }
        }

        /// <summary>
        /// Opens and closes right claw
        /// </summary>
        private void OnRightArmButtonPressed(System.Object sender, ButtonEventArgs e)
        {
            if (Busy is false)
            {
                //Busy = true;
                Arms.Use(SubArmSide.Right);
            }
        }


        /// <summary>
        /// Changes item in respective item bay
        /// </summary>
        private void OnItemButtonPressed(object sender, ButtonEventArgs e)
        {
            if (Busy is false)
            {
                Busy = true;
                //RightArm.AnimationComplete += OnArmAnimationFinished;
                //RightArm.ChangeItem(ItemWindows[e.ButtonIndex]);
            }
        }

        /// <summary>
        /// Updates item button state when item changes
        /// </summary>
        private void OnWindowItemChanged(System.Object sender, ItemChangedEventArgs e)
        {
            var window = sender as ItemWindow;
            //ItemButtons[window.Index].Empty = e.Item is NoneItem;
        }

        /// <summary>
        /// Sets HUD to not busy when arm animation finished
        /// </summary>
        private void OnArmAnimationFinished(System.Object sender, EventArgs e)
        {
            //(sender as SubArm).OperationComplete -= OnArmAnimationFinished;
            Busy = false;
        }
    }
}
