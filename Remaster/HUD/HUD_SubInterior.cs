using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Remaster.Items;
using Remaster.Player;
using Remaster.Utilities;

namespace Remaster.HUD
{
    public class HUD_SubInterior : HUDBase
    {
        #region Elements
        private readonly List<SubInteriorItemButton> ItemButtons = new SubInteriorItemButton[6].ToList();
        private readonly List<ItemWindow> ItemWindows = new ItemWindow[6].ToList();
        private readonly List<SubInteriorItemButton> ToolButtons = new SubInteriorItemButton[6].ToList();

        private SubConsole Console;
        private SubArm ItemArm;
        private SubInteriorItemButton ItemArmButton;
        private HUDToggle ItemArmToggle;
        private ItemTransitionBay ItemTransitionBay = new ItemTransitionBay();
        private IHostItems SubTarget;

        /// <summary>
        /// Gets all the HUD elements and adds them to the Elements dictionary
        /// </summary>
        private void GetHUDElements()
        {
            ItemArm = GetNode<SubArm>("ItemArm");

            GetTree().GetNodesInGroup("HUD_Clickable").Cast<Clickable>().ToList().ForEach(clickable =>
            {
                switch (clickable)
                {
                    case SubInteriorItemButton rightArmButton when rightArmButton.Name == "RightClawButton":
                        ItemArmButton = rightArmButton;
                        break;
                    case SubInteriorItemButton itemButton when itemButton.GetParent().Name == "ItemButtons":
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
                        ItemArmToggle = toggle;
                        break;
                    case IHostItems itemHost when SubTarget is null:
                        SubTarget = itemHost;
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

        /// <summary>
        /// True if sub is busy
        /// </summary>
        public Boolean Busy => ItemArm.Busy is true || ItemWindows.Any(w => w.Busy is true);

        private Boolean ItemArmBusy;
        private Boolean ItemBayBusy;

        private Single ItemTransitTime = 1f;

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            GetHUDElements();

            //Item windows
            PlayerData.Items.Select((item, index) => (item, index)).ToList().ForEach(d => ItemWindows[d.index].ForceChangeItem(d.item));

            foreach (var window in ItemWindows)
            {
                window.ItemWindowEvent += OnItemWindowEvent;
            }

            //Item arm
            ItemArm.ForceChangeItem(PlayerData.ItemArm);
            ItemArm.OperationComplete += OnItemArmOperationComplete;
            ItemArmButton.ButtonPress += OnRightArmButtonPressed;
            ItemArmToggle.Toggled += OnItemArmToggle;

            //Item buttons
            foreach (var button in ItemButtons)
            {
                button.ButtonPress += OnItemButtonPressed;
            }

            //Sub target
            if (SubTarget is Clickable clickableTarget && SubTarget is IPrintable)
            {
                clickableTarget.MouseEvent += OnSubTargetClick;
            }
        }

        private void OnSubTargetClick(System.Object sender, ClickableMouseEventArgs e)
        {
            if (e.MouseState == MouseEventType.Down)
            {
                Console.Print(sender);
            }
        }

        #region Item Arm
        /// <summary>
        /// Handles Item Arm operation complete events
        /// </summary>
        private void OnItemArmOperationComplete(System.Object sender, ArmOperationCompleteEventArgs e)
        {
            ItemArmBusy = false;
            if (e.Operation == SubArmAction.Intake)
            {
                ItemTransitionBay.Deposit(sender as SubArm, e.Item);
            }
        }

        /// <summary>
        /// Handles Item Arm Toggle events
        /// </summary>
        private void OnItemArmToggle(System.Object sender, ToggleEventArgs e) => MoveItemArm(e.On);

        /// <summary>
        /// Extends or parks the item arm
        /// </summary>
        private void MoveItemArm(Boolean extend)
        {
            if (Busy is false)
            {
                ItemArmBusy = true;
                if (extend is true && ItemArm.Extended is false)
                {
                    ItemArm.Extend();
                }
                else if (extend is false && ItemArm.Extended is true)
                {
                    ItemArm.Park();
                }
            }
        }

        /// <summary>
        /// Handles Item Arm Button events
        /// </summary>
        private void OnRightArmButtonPressed(System.Object sender, ButtonEventArgs e)
        {
            if (ItemArm.Extended is true)
            {
                var response = SubTarget.Response(ItemArm.Item);
                Console.Print(response);
                if (response.Willing is true)
                {
                    ItemArm.ForceChangeItem(SubTarget.Trade(ItemArm.Item));
                }
            }
        }

        //TODO: 'use item' parsing/animation triggers
        /// <summary>
        /// Opens and closes claw
        /// </summary>
        private void UseItemArm()
        {
            if (Busy is false)
            {
                ItemArmBusy = true;
                if (ItemArm.ClawOpen is true)
                {
                    ItemArm.Close();
                }
                else
                {
                    ItemArm.Open();
                }
            }
        }

        #endregion


        #region Items
        /// <summary>
        /// Changes item in respective item bay
        /// </summary>
        private void OnItemButtonPressed(object sender, ButtonEventArgs e)
        {
            if (Busy is false)
            {
                var window = ItemWindows[e.ButtonIndex];
                if (window.Busy is false && ItemArm.Busy is false)
                {
                    ItemBayBusy = true;
                    ItemArmBusy = true;
                    window.Output();
                    ItemArm.Intake();
                }
            }
        }

        /// <summary>
        /// Updates item button state when item changes
        /// </summary>
        private void OnItemWindowEvent(System.Object sender, ItemWindowEventArgs e)
        {
            if (e.Type != ItemWindowEventType.Click)
            {
                ItemBayBusy = false;
            }

            switch (e.Type)
            {
                case ItemWindowEventType.Expel:
                    ItemTransitionBay.Deposit(sender as ItemWindow, e.Item);
                    break;
                case ItemWindowEventType.Intake:
                    break;
                case ItemWindowEventType.Click:
                    Console.Print(e.Item.Description(nameof(SubConsole)));
                    break;
            }
        }
        #endregion
    }
}
