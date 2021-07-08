using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster
{
    /// <summary>
    /// States a button can be in
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
    /// State of mouse click
    /// </summary>
    public enum MouseButtonState
    {
        Up,
        Down
    }

    /// <summary>
    /// State of mouse hover
    /// </summary>
    public enum MouseHoverState
    {
        Over,
        Out
    }
}
