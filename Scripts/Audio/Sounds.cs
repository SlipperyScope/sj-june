using Godot;
using System;

namespace Audio
{
    public static class Paths
    {
        public const String MusicManagerPath = "/root/MusicManager";
        public const String SFXMangerPath = "/root/SFXManager";

        public static T GetManager<T>(this Node node, String path) where T: class => node.GetNode<T>(path);
    }
    public enum SongID
    {
        None,
        JourneyNorth,
        Descent,
        Depths,
        DarkPassages,
        JustPressPlay
    }

    public enum SFXID
    {
        None,
        UIHover,
        UIUse,
        C4,
        PipePickup,
        CrabPipe
    }

    public enum SFXBus
    {
        UI,
        Effect,
        Background,
        Generic
    }

    public struct Song
    {
        public String Name;
        public String Path;
        public Single BPM;
        public Int32 MeasureLength;

        public Single PlaybackVolume;

        public static Song None => new Song()
        {
            Name = "None",
            Path = "",
            BPM = 60,
            MeasureLength = 1,
            PlaybackVolume = -10
        };

        public override String ToString() => Name;
    }

    public struct Sound
    {
        public String Name;
        public String Path;
        public Single PlaybackVolume;
        public SFXBus Bus;
        public override String ToString() => Name;
    }

    public class BeatEventArgs : EventArgs
    {
        public readonly Int32 CurrentBeat;
        public readonly Int32 BeatInMeasure;

        public BeatEventArgs(Int32 currentBeat, Int32 beatInMeasure)
        {
            CurrentBeat = currentBeat;
            BeatInMeasure = beatInMeasure;
        }
    }

    public class SongChangedEventArgs : EventArgs
    {
        public readonly Song Song;
        public readonly Song LastSong;

        public SongChangedEventArgs(Song song, Song lastSong)
        {
            Song = song;
            LastSong = lastSong;
        }
    }
}
