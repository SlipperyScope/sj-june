using Godot;
using System;
using System.Collections.Generic;

namespace Audio
{
    public class MusicManager : Node2D
    {
        private Dictionary<SongID, Song> Songs = new Dictionary<SongID, Song>
        {
            {
                SongID.JourneyNorth, new Song()
                {
                    Path = "res://Audio/Music/Journey North/Journey North 02.mp3",
                    UseLoopPoints = false,
                    PlaybackVolume = -10f
                }
            },
            {
                SongID.Descent, new Song()
                {
                    Path = "res://Audio/Music/Descent/Descent 03.mp3",
                    UseLoopPoints = true,
                    LoopAt = 0f,
                    LoopTo = 0f,
                    PlaybackVolume = -5f
                }
            },
            {
                SongID.Depths, new Song()
                {
                    Path = "res://Audio/Music/Depths/Depths.mp3",
                    UseLoopPoints = true,
                    LoopAt = 0f,
                    LoopTo = 0f,
                    PlaybackVolume = 0f
                }
            },
            {
                SongID.DarkPassages, new Song()
                {
                    Path = "res://Audio/Music/Dark Passages/Dark Passages.mp3",
                    UseLoopPoints = true,
                    LoopAt = 0f,
                    LoopTo = 0f,
                    PlaybackVolume = -10f
                }
            }
        };

        const Single FadeTime = 1f;
        const Single InterpMinVolume = -40f;
        
        private AudioStreamPlayer Main;
        private AudioStreamPlayer Fade;

        private Song CurrentSong;
        private Song FadeToSong;

        private Boolean Fading = false;

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            Main = new AudioStreamPlayer();
            Fade = new AudioStreamPlayer();
            AddChild(Main);
            AddChild(Fade);
            Play(Songs[SongID.JourneyNorth]);
            GD.Print("playing");
        }

        /// <summary>
        /// Process
        /// </summary>
        public override void _Process(Single delta)
        {
            if (Fading is true)
            {
                Main.VolumeDb = Interpolate(CurrentSong.PlaybackVolume, InterpMinVolume, FindAlpha(CurrentSong.PlaybackVolume, InterpMinVolume, Main.VolumeDb) + delta / FadeTime);
                Fade.VolumeDb = Interpolate(InterpMinVolume, FadeToSong.PlaybackVolume, FindAlpha(InterpMinVolume, FadeToSong.PlaybackVolume, Fade.VolumeDb) + delta / FadeTime);
                if (Fade.VolumeDb >= FadeToSong.PlaybackVolume)
                {
                    Fade.VolumeDb = FadeToSong.PlaybackVolume;
                    var main = Main;
                    var fade = Fade;
                    Main = fade;
                    Fade = main;
                    Fade.Stop();
                    Fading = false;
                }
            }

            if (Input.IsActionJustPressed("ui_select"))
            {
                var ran = new Random();
                var next = ran.Next(0, 3);
                FadeTo(Songs[(SongID)next]);
            }
        }

        /// <summary>
        /// Plays a song from the beginning with no fade
        /// </summary>
        /// <param name="song">Song to play</param>
        public void Play(Song song)
        {
            CurrentSong = song;
            var stream = GD.Load<AudioStreamMP3>(song.Path);
            stream.Loop = true;
            Main.Stream = stream;
            Main.VolumeDb = song.PlaybackVolume;
            Main.Play();
        }

        /// <summary>
        /// Fades from the current song to another
        /// </summary>
        /// <param name="song">Song to fade to</param>
        public void FadeTo(Song song)
        {
            FadeToSong = song;
            var stream = GD.Load<AudioStreamMP3>(song.Path);
            stream.Loop = true;
            Fade.Stream = stream;
            Fade.VolumeDb = InterpMinVolume;
            Fade.Play();
            Fading = true;
        }

        /// <summary>
        /// Interpolates <paramref name="a"/> and <paramref name="b"/> via <paramref name="alpha"/>
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <param name="alpha">Distance between the two, [0...1]</param>
        /// <returns>Value between a and b</returns>
        public Single Interpolate(Single a, Single b, Single alpha) => a + (b - a) * alpha;

        /// <summary>
        /// Converts a value to the alpha between a and b
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <param name="value">Value to convert to interp alpha</param>
        /// <returns>Interpolation alpha</returns>
        public Single FindAlpha(Single a, Single b, Single value) => (value - a) / (b - a);
    } 
}


