using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Remaster
{
    public class Globals : Node
    {
        public static Globals Global
        {
            get
            {
                if (_Global is null)
                {
                    _Global = new Globals();
                }
                return _Global;
            }
        }
        private static Globals _Global = null;

        public static PlayerData PlayerData
        {
            get
            {
                if (Global._PlayerData is null)
                {
                    Global._PlayerData = Global.GetNodeOrNull<PlayerData>("/root/PlayerData");
                }

                return Global._PlayerData;
            }
        }
        private PlayerData _PlayerData;
    }
}
