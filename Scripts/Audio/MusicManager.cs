using Godot;
using System;
using System.Collections.Generic;

namespace Audio
{
    public class MusicManager : Node2D
    {
        public delegate void OnBeatHandler(object sender, BeatEventArgs e);
        public event OnBeatHandler Beat;

        public delegate void OnSongChangedHandler(object sender, SongChangedEventArgs e);
        public event OnSongChangedHandler SongChanged;

        private Dictionary<SongID, Song> Songs = new Dictionary<SongID, Song>
        {
            {
                SongID.None, Song.None
            },
            {
                SongID.JourneyNorth, new Song()
                {
                    Name = "Journey North",
                    Path = "res://Audio/Music/Journey North/Journey North 03.mp3",
                    BPM = 130f,
                    MeasureLength = 4,
                    PlaybackVolume = -15f
                }
            },
            {
                SongID.Descent, new Song()
                {
                    Name = "Descent",
                    Path = "res://Audio/Music/Descent/Descent 03.mp3",
                    BPM = 120f,
                    MeasureLength = 4,
                    PlaybackVolume = -10f
                }
            },
            {
                SongID.Depths, new Song()
                {
                    Name = "Depths",
                    Path = "res://Audio/Music/Depths/Depths.mp3",
                    BPM = 60f,
                    MeasureLength = 4,
                    PlaybackVolume = -5f
                }
            },
            {
                SongID.DarkPassages, new Song()
                {
                    Name = "Dark Passages",
                    Path = "res://Audio/Music/Dark Passages/Dark Passages.mp3",
                    BPM = 60f,
                    MeasureLength = 4,
                    PlaybackVolume = -15f
                }
            },
            {
                SongID.JustPressPlay, new Song()
                {
                    Name = "Just Press Play",
                    Path = "res://Audio/Music/Just Press Play/Just Press Play.mp3",
                    BPM = 108f,
                    MeasureLength = 4,
                    PlaybackVolume = -15f
                }
            }
        };

        public Int32 BeatCount { get; private set; } = -1;
        const Single FadeTime = 1f;
        const Single InterpMinVolume = -40f;
        
        private AudioStreamPlayer Main;
        private AudioStreamPlayer Fade;

        public Song NowPlaying { get; private set; } = Song.None;
        public Song LastPlayed { get; private set; } = Song.None;

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
            // Fade in main if it's not at volume
            if (Main.VolumeDb < NowPlaying.PlaybackVolume)
            {
                Main.VolumeDb = Interpolate(InterpMinVolume, NowPlaying.PlaybackVolume, FindAlpha(InterpMinVolume, NowPlaying.PlaybackVolume, Main.VolumeDb) + delta / FadeTime);
                if (Main.VolumeDb > NowPlaying.PlaybackVolume)
                {
                    Main.VolumeDb = NowPlaying.PlaybackVolume;
                }
            }

            // Fade out fade if it's not at min
            if (Fade.VolumeDb > InterpMinVolume)
            {
                Fade.VolumeDb = Interpolate(LastPlayed.PlaybackVolume, InterpMinVolume, FindAlpha(LastPlayed.PlaybackVolume, InterpMinVolume, Fade.VolumeDb) + delta / FadeTime);
                if (Fade.VolumeDb <= InterpMinVolume)
                {
                    Fade.Stop();
                }
            }

            // Debug manual music change
            if (Input.IsActionJustPressed("ui_select"))
            {
                var ran = new Random();
                var next = ran.Next(1, 5);
                ChangeSong((SongID)next);
                GD.Print($"{next} {(SongID)next}");
            }
        }

        /// <summary>
        /// Changes the playing song
        /// </summary>
        /// <param name="id">Song ID</param>
        /// <param name="fade">Crossfade the songs</param>
        public void ChangeSong(SongID id, Boolean fade = true)
        {
            var nextSong = Songs[id];
            if (NowPlaying.Name != nextSong.Name)
            {
                LastPlayed = NowPlaying;
                NowPlaying = nextSong;
                BeatCount = -1;

                if (fade is true)
                {
                    var oldfade = Fade;
                    Fade = Main;
                    Main = oldfade;
                }

                if (id == SongID.None)
                {
                    Main.Stop();
                    BeatTimer.Stop();
                }
                else
                {
                    Main.Stream = GD.Load<AudioStreamMP3>(nextSong.Path);
                    BeatTimer.Start(60f / nextSong.BPM);
                    Main.VolumeDb = fade ? InterpMinVolume : nextSong.PlaybackVolume;
                    Main.Play();
                    OnBeat();
                }

                SongChanged?.Invoke(this, new SongChangedEventArgs(NowPlaying, LastPlayed));
            }
        }

        /// <summary>
        /// Dispatches beat event
        /// </summary>
        public void OnBeat()
        {
            BeatCount++;
            //GD.Print($"ref: {ReferenceSong.Path}");
            var args = new BeatEventArgs(BeatCount, BeatCount % NowPlaying.MeasureLength);
            Beat?.Invoke(this, args);
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


