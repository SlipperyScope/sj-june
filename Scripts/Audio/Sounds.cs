using Godot;
using System;

namespace Audio
{
    public enum SongID
    {
        JourneyNorth,
        Descent,
        Depths,
        DarkPassages
    }

    public struct Song
    {
        public String Path;

        public Boolean UseLoopPoints;
        public Single LoopAt;
        public Single LoopTo;

        public Single PlaybackVolume;
    }
}
