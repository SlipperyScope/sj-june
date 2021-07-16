using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using rPlayerData = Remaster.Player.PlayerData;

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

        private Dictionary<String, rPlayerData> _PlayerData = new Dictionary<string, rPlayerData>();

        public rPlayerData GetPlayerData(String key) => _PlayerData.ContainsKey(key) ? _PlayerData[key] : null;
        public Boolean AddPlayerData(String key, rPlayerData data)
        {
            if (data is null) return false;

            var unique = _PlayerData.ContainsKey(key) is false;
            if (unique is true)
            {
                _PlayerData.Add(key, data);
            }
            return unique;
        }
        public rPlayerData RemovePlayerData(String key)
        {
            var data = _PlayerData.ContainsKey(key) ? _PlayerData[key] : null;
            if (data != null)
            {
                _PlayerData.Remove(key);
            }
            return data;
        }
    }
}
