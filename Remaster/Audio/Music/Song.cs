using Godot;
using System;

namespace Remaster.Audio
{
    /// <summary>
    /// Defines properties of a song
    /// </summary>
    public class Song : Resource
    {
        /// <summary>
        /// Song name
        /// </summary>
        [Export(PropertyHint.PlaceholderText, "Song name")]
        public String Name { get; private set; } = "";

        /// <summary>
        /// Path to audio resource
        /// </summary>
        [Export(PropertyHint.File, "*.mp3,*.wav")]
        public String File { get; private set; } = "";

        [Export(PropertyHint.Range, "-50,20,0.5")]
        public Single PlaybackVolume { get; private set; } = 0f;

        /// <summary>
        /// Song BPM
        /// </summary>
        [Export]
        public Single BPM { get; private set; } = 120f;

        /// <summary>
        /// Beats per measure
        /// </summary>
        [Export]
        public Int32 MeasureLength { get; private set; } = 4;

        public override String ToString() => Name;
    }
}
