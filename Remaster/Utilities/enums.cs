using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster.Utilities
{
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
