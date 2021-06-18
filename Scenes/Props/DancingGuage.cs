using Audio;
using Godot;
using System;
using System.Collections.Generic;

public class DancingGuage : Node2D
{
    private Random Rand = new Random();

    private MusicManager MusicManager;
    private Sprite Needle;
    private AnimationPlayer Animator;

    private List<String> Animations = new List<string>()
    {
        "Wobble", "Pulse"
    };

    /// <summary>
    /// Ready
    /// </summary>
    public override void _Ready()
    {
        Needle = GetNode<Sprite>("Needle");
        MusicManager = GetNode<MusicManager>("/root/MusicManager");
        Animator = FindNode("Animator") as AnimationPlayer;

        MusicManager.Beat += OnBeat;
        MusicManager.SongChanged += OnSongChanged;
        CallDeferred(nameof(SetAnimationSpeed), args: MusicManager.NowPlaying.BPM);
    }

    /// <summary>
    /// On Beat
    /// </summary>
    private void OnBeat(object sender, BeatEventArgs e)
    {
        if (e.BeatInMeasure == 0 && e.CurrentBeat % MusicManager.NowPlaying.MeasureLength * 2 == 0)
        {
            Animator.CurrentAnimation = Animations[Rand.Next(0,25) == 0 ? 0 : 1];
        }
    }

    /// <summary>
    /// On Song Changed
    /// </summary>
    private void OnSongChanged(object sender, SongChangedEventArgs e)
    {
        if (e.Song.BPM != e.LastSong.BPM)
        {
            SetAnimationSpeed(e.Song.BPM);
        }
    }

    /// <summary>
    /// Sets the playback speed of the animations
    /// </summary>
    /// <param name="BPM">Song BPM</param>
    private void SetAnimationSpeed(Single BPM)
    {
        Animator.PlaybackSpeed = BPM / 10f;
    }
}
