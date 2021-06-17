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

    public enum TimeSignature
    {
        FourFour,
        ThreeFour,
        NineEight
    }

    public struct Song
    {
        public String Path;

        public Single BPM;
        public TimeSignature TimeSignature;

        public Single PlaybackVolume;
    }
}
