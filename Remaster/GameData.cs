using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster
{
    public static class GameData
    {
        public static Int32 Act = 1;
        public static HUDID CurrentHUD = HUDID.SubmarineInterior;
    }

    public enum HUDID
    {
        None,
        SubmarineInterior
    }
}
