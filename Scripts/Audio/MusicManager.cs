using Godot;
using System;
using System.Collections.Generic;

namespace Audio
{
    public class MusicManager : Node2D
    {
        public delegate void OnBeatHandler(object sender, EventArgs e);
        public event OnBeatHandler Beat;

        private Dictionary<SongID, Song> Songs = new Dictionary<SongID, Song>
        {
            {
                SongID.JourneyNorth, new Song()
                {
                    Path = "res://Audio/Music/Journey North/Journey North 03.mp3",
                    BPM = 130,
                    TimeSignature = TimeSignature.FourFour,
                    PlaybackVolume = -10f
                }
            },
            {
                SongID.Descent, new Song()
                {
                    Path = "res://Audio/Music/Descent/Descent 03.mp3",
                    BPM = 120,
                    TimeSignature = TimeSignature.FourFour,
                    PlaybackVolume = -5f
                }
            },
            {
                SongID.Depths, new Song()
                {
                    Path = "res://Audio/Music/Depths/Depths.mp3",
                    BPM = 120,
                    TimeSignature = TimeSignature.FourFour,
                    PlaybackVolume = 0f
                }
            },
            {
                SongID.DarkPassages, new Song()
                {
                    Path = "res://Audio/Music/Dark Passages/Dark Passages.mp3",
                    BPM = 120,
                    TimeSignature = TimeSignature.FourFour,
                    PlaybackVolume = -10f
                }
            }
        };

        public Int32 BeatCount { get; private set; } = -1;
        const Single FadeTime = 1f;
        const Single InterpMinVolume = -40f;
        
        private AudioStreamPlayer Main;
        private AudioStreamPlayer Fade;

        private Song CurrentSong;
        private Song FadeToSong;

        private Boolean Fading = false;

        private Timer BeatTimer;

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            Main = new AudioStreamPlayer();
            Fade = new AudioStreamPlayer();
            AddChild(Main);
            AddChild(Fade);

            GD.Print("Adding timer");
            BeatTimer = new Timer();
            BeatTimer.Connect("timeout", this, nameof(OnBeat));
            AddChild(BeatTimer);
            BeatTimer.Stop();
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
                var next = ran.Next(0, 4);
                FadeTo((SongID)next);
                GD.Print($"{next} {(SongID)next}");
            }
        }

        /// <summary>
        /// Plays a song from the beginning with no fade
        /// </summary>
        /// <param name="song">Song to play</param>
        public void Play(SongID id)
        {
            var song = Songs[id];
            CurrentSong = song;
            var stream = GD.Load<AudioStreamMP3>(song.Path);
            stream.Loop = true;
            Main.Stream = stream;
            Main.VolumeDb = song.PlaybackVolume;
            Main.Play();
            BeatCount = -1;
            OnBeat();
            BeatTimer.Start(60f / CurrentSong.BPM);
        }

        /// <summary>
        /// Fades from the current song to another
        /// </summary>
        /// <param name="song">Song to fade to</param>
        public void FadeTo(SongID id)
        {
            var song = Songs[id];
            FadeToSong = song;
            var stream = GD.Load<AudioStreamMP3>(song.Path);
            stream.Loop = true;
            Fade.Stream = stream;
            Fade.VolumeDb = InterpMinVolume;
            Fade.Play();
            BeatCount = -1;
            OnBeat();
            BeatTimer.WaitTime = 60f / FadeToSong.BPM;
            BeatTimer.Start();
            Fading = true;
        }

        /// <summary>
        /// Dispatches beat event
        /// </summary>
        public void OnBeat()
        {
            BeatCount++;
            Beat?.Invoke(this, new EventArgs());
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


