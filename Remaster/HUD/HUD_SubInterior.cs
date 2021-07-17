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
        /// <summary>
        /// HUD element IDs
        /// </summary>
        private enum HUDElementID
        {
            Console,
            MainControls,
            RightArm,
            RightArmToggle,
            RightArmUse,
            ItemButton0,
            ItemButton1,
            ItemButton2,
            ItemButton3,
            ItemButton4,
            ItemButton5,
            ItemWindow0,
            ItemWindow1,
            ItemWindow2,
            ItemWindow3,
            ItemWindow4,
            ItemWindow5,
            ToolButton0,
            ToolButton1,
            ToolButton2,
            ToolButton3,
            ToolButton4,
            ToolButton5
        }

        /// <summary>
        /// Dictionary of HUD element references
        /// </summary>
        private Dictionary<HUDElementID, System.Object> Elements = new Dictionary<HUDElementID, System.Object>();

        /// <summary>
        /// Gets a HUD element by ID and returns it as type T
        /// </summary>
        /// <typeparam name="T">Type to return element as</typeparam>
        /// <param name="ID">ID of element</param>
        /// <returns>Element as type T</returns>
        private T Get<T>(HUDElementID ID) where T : class => Elements[ID] as T;

        /// <summary>
        /// Gets an item button by index
        /// </summary>
        /// <param name="index">Index of button</param>
        /// <returns>Item button</returns>
        private SubInteriorItemButton ItemButton(Int32 index) => index switch
        {
            0 => Get<SubInteriorItemButton>(HUDElementID.ItemButton0),
            1 => Get<SubInteriorItemButton>(HUDElementID.ItemButton1),
            2 => Get<SubInteriorItemButton>(HUDElementID.ItemButton2),
            3 => Get<SubInteriorItemButton>(HUDElementID.ItemButton3),
            4 => Get<SubInteriorItemButton>(HUDElementID.ItemButton4),
            5 => Get<SubInteriorItemButton>(HUDElementID.ItemButton5),
            _ => throw new ArgumentException("Invalid index", nameof(index))
        };

        private List<SubInteriorItemButton> ItemButtons => (from value in Elements.Values
                                                            where value is SubInteriorItemButton ib && ib.GetParent().Name == "ItemButtons"
                                                            select value as SubInteriorItemButton).ToList();

        /// <summary>
        /// Gets an item window by index
        /// </summary>
        /// <param name="index">Index of window</param>
        /// <returns>Item window</returns>
        private ItemWindow ItemWindow(Int32 index) => index switch
        {
            0 => Get<ItemWindow>(HUDElementID.ItemWindow0),
            1 => Get<ItemWindow>(HUDElementID.ItemWindow1),
            2 => Get<ItemWindow>(HUDElementID.ItemWindow2),
            3 => Get<ItemWindow>(HUDElementID.ItemWindow3),
            4 => Get<ItemWindow>(HUDElementID.ItemWindow4),
            5 => Get<ItemWindow>(HUDElementID.ItemWindow5),
            _ => throw new ArgumentException("Invalid index", nameof(index))
        };

        /// <summary>
        /// Gets all the HUD elements and adds them to the Elements dictionary
        /// </summary>
        private void GetHUDElements()
        {
            Elements.Add(HUDElementID.RightArm, GetNode("Arms/Right"));

            GetTree().GetNodesInGroup("HUD_Clickable").Cast<Clickable>().ToList().ForEach(clickable =>
            {
                Elements.Add(clickable switch
                {
                    SubInteriorItemButton rightClaw when rightClaw.Name == "RightClawButton" => HUDElementID.RightArmUse,
                    SubInteriorItemButton button when button.GetParent().Name == "ItemButtons" => ToElementID($"ItemButton{button.Index}"),
                    SubInteriorItemButton button when button.GetParent().Name == "ToolButtons" => ToElementID($"ToolButton{button.Index}"),
                    ItemWindow window => ToElementID($"ItemWindow{window.Index}"),
                    HUDToggle toggle => HUDElementID.RightArmToggle,
                    SubConsole console => HUDElementID.Console,
                    _ => throw new KeyNotFoundException($"Could not find an element ID for {clickable.GetPath()}")
                }, clickable);
            });
        }

        /// <summary>
        /// Attemps to convert a string to a HUDElementID
        /// </summary>
        /// <param name="name">Name of element id</param>
        /// <returns>ID</returns>
        private static HUDElementID ToElementID(String name) => (HUDElementID)Enum.Parse(typeof(HUDElementID), name);
        #endregion

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

        //private List<Clickable> Clickables;

        //private SubConsole Console;
        //private SubInteriorArms Arms;
        //private SubArm RightArm;

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            GetHUDElements();

            foreach (var button in ItemButtons)
            {
                button.ButtonPress += OnItemButtonPressed;
            }

            PlayerData.Items.Select((item, index) => (item, index)).ToList().ForEach(d => ItemWindow(d.index).ChangeItem(d.item));
        }

        /// <summary>
        /// On Item Button Pressed
        /// </summary>
        private void OnItemButtonPressed(object sender, ButtonEventArgs e)
        {
            if (Busy is false)
            {
                Get<SubArm>(HUDElementID.RightArm).ChangeItem(ItemWindow(e.ButtonIndex));
                //ItemWindow(e.ButtonIndex).ChangeItem(Get<SubArm>(HUDElementID.RightArm).Item, true, true);
            }
        }

        //private void OnArmButtonPressed(System.Object sender, ButtonEventArgs e)
        //{
        //    GD.Print($"armbutton: {sender}{(sender as Node).Name}");

        //    var button = sender as SubInteriorItemButton;
        //    if (button.Name == "LeftArmButton")
        //    {
        //        //Arms.QueueAction(ArmAction.LeftExtend, ArmAction.LeftPark);
        //    }
        //    else if (button.Name == "RightArmButton")
        //    {
        //        //Arms.QueueAction(ArmAction.RightExtend, ArmAction.RightPark);
        //    }
        //}

        //private void OnClickableMouseEvent(object sender, ClickableMouseEventArgs e)
        //{
        //    if (e.MouseState == MouseEventType.Down)
        //    {
        //        switch (sender)
        //        {
        //            case ItemWindow window:
        //                Console.Print(window.Item);
        //                break;
        //            default:
        //                Console.Print(sender);
        //                break;
        //        }
        //    }
        //}

        //private void OnItemButtonPressed(object sender, ButtonEventArgs e)
        //{
        //    //ItemWindows[ItemButtons.IndexOf(sender as SubInteriorItemButton)].ChangeItem(new Seaweed(), true, true);
        //}
    }
}
