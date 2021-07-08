using Godot;
using System;

namespace Remaster.Audio
{
    /// <summary>
    /// Plays music
    /// </summary>
    public class MusicPlayer : Node
    {
        /// <summary>
        /// dB value of fully "faded out" 
        /// </summary>
        [Export]
        public Single FadeMin { get; private set; } = -40f;

        /// <summary>
        /// Seconds to fade in or out completely
        /// </summary>
        [Export]
        public Single FadeTime { get; private set; } = 1f;

        /// <summary>
        /// Player reference
        /// </summary>
        public AudioStreamPlayer Player { get; private set; } = new AudioStreamPlayer();

        /// <summary>
        /// True if music is playing
        /// </summary>
        public Boolean Playing => Player?.Playing ?? false;

        private Song CurrentSong;

        private Boolean Fading = false;
        private Single FadeTargetVolume = 0f;

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            AddChild(Player);
        }

        /// <summary>
        /// Process
        /// </summary>
        public override void _Process(Single delta)
        {
            if (Fading is true)
            {
                if (Player.VolumeDb < FadeTargetVolume)
                {
                    // Fade out
                    Player.VolumeDb = Interp(FadeMin, CurrentSong.PlaybackVolume, FindAlpha(FadeMin, CurrentSong.PlaybackVolume, Player.VolumeDb) + delta / FadeTime);
                    if (Player.VolumeDb >= FadeTargetVolume)
                    {
                        Player.VolumeDb = FadeTargetVolume;
                        Fading = false;
                    }
                }
                else if (Player.VolumeDb > FadeTargetVolume)
                {
                    //Fade in
                    Player.VolumeDb = Interp(CurrentSong.PlaybackVolume, FadeMin, FindAlpha(CurrentSong.PlaybackVolume, FadeMin, Player.VolumeDb) + delta / FadeTime);
                    if (Player.VolumeDb <= FadeMin)
                    {
                        Fading = false;
                        Player.Stop();
                    }
                }
            }
        }

        /// <summary>
        /// Plays a song
        /// </summary>
        /// <param name="song">Song</param>
        /// <param name="fadeIn">Fade song in</param>
        public void Play(Song song, Boolean fadeIn = false)
        {
            if (song is null) return;
            CurrentSong = song;
            Player.Stream = GD.Load<AudioStream>(song.File);
            Player.VolumeDb = fadeIn is true ? FadeMin : song.PlaybackVolume;
            FadeTargetVolume = song.PlaybackVolume;
            Fading = fadeIn;
            Player.Play();
            GD.Print($"Now playing {song}");
        }

        /// <summary>
        /// Resumes song, fading back in if it is not at normal playbaack volume
        /// </summary>
        public void Resume()
        {
            if (CurrentSong is null) return;

            if (Player.VolumeDb != CurrentSong.PlaybackVolume)
            {
                FadeTargetVolume = CurrentSong.PlaybackVolume;
                Fading = true;
            }

            Player.StreamPaused = false;
        }

        /// <summary>
        /// Stops music playback
        /// </summary>
        /// <param name="fadeOut">Fade out music instead of stopping immediately</param>
        public void Stop(Boolean fadeOut = false)
        {
            Fading = fadeOut;

            if (fadeOut is false)
            {
                Player.Stop();
                return;
            }

            FadeTargetVolume = FadeMin;
        }

        /// <summary>
        /// Pauses song
        /// </summary>
        public void Pause()
        {
            Player.StreamPaused = true;
        }

        /// <summary>
        /// Interpolates between two values
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <param name="alpha">Alpha value</param>
        /// <returns>Interpolated value</returns>
        public static Single Interp(Single a, Single b, Single alpha) => a + (b - a) * alpha;

        /// <summary>
        /// Returns the alpha that results in the current value when interpolating between A and B
        /// </summary>
        /// <param name="a">Value A</param>
        /// <param name="b">Value B</param>
        /// <param name="current">Curret Value</param>
        /// <returns>Alpha</returns>
        public static Single FindAlpha(Single a, Single b, Single current) => (current - a) / (b - a);
    }
}