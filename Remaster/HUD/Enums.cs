using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster.HUD
{
    public enum ArmID
    {
        Tool,
        Item
    }

    public enum SubArmSide
    {
        Left,
        Right
    }

    public enum SubArmAction
    {
        None,
        Extend,
        Park,
        Open,
        Close,
        Push,
        Pull
    }

    public enum ArmOperationEvent
    {
        Started,
        Stopped,
        Aborted
    }
}
