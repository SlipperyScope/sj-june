using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster
{
    /// <summary>
    /// Button states
    /// </summary>
    public enum ButtonState
    {
        Over,
        Down,
        Out,
        Disabled
    }

    /// <summary>
    /// Mouse event types
    /// </summary>
    public enum MouseEventType
    {
        Up,
        Over,
        Down,
        Out
    }

    /// <summary>
    /// Mouse button states
    /// </summary>
    public enum MouseButtonState
    {
        Up,
        Down
    }

    /// <summary>
    /// Mouse hover states
    /// </summary>
    public enum MouseHoverState
    {
        Over,
        Out
    }

    /// <summary>
    /// List of game items
    /// </summary>
    public enum ItemID
    {
        None,
        Pipe,
        Seaweed
    }

    public enum HUDButtonType
    {
        None,
        Item,
        Tool
    }

    public enum PrintMethod
    {
        All,
        Section,
        Word,
        Character
    }

    public enum PrintToken
    {
        Text,
        Pause,
        Image,
        Clear
    }
}
