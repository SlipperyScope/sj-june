using Godot;
using System;

namespace Remaster.Audio
{
    /// <summary>
    /// Defines various properties of a sound effect
    /// </summary>
    public class SoundEffect : Resource
    {
        /// <summary>
        /// Sound effect name
        /// </summary>
        [Export(PropertyHint.PlaceholderText, "Sound effect name")]
        public String Name { get; private set; } = "";

        /// <summary>
        /// Path to audio resource
        /// </summary>
        [Export(PropertyHint.File, "*.mp3,*.wav")]
        public String File { get; private set; } = "";

        /// <summary>
        /// Intended playback volume in dB
        /// </summary>
        [Export(PropertyHint.Range, "-50,20,0.5")]
        public Single PlaybackVolume { get; private set; } = 0f;

        public override String ToString() => Name;
    } 
}
