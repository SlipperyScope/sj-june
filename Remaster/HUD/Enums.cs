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
        Output,
        Intake
    }

    public enum ArmOperationEventType
    {
        Started,
        Stopped,
        Aborted
    }

    public enum ItemWindowEventType
    {
        Expel,
        Intake,
        Click
    }
}
