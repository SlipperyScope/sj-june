using Audio;
using Godot;
using System;

public class Bubble : Node2D
{
    const Single TwoPi = (Single)Math.PI * 2f;
    const Single FloatSpeed = 150f; 

    private MusicManager MusicManager;
    private AnimationPlayer Animator;
    private Random Rand = new Random();
    private Int32 ChangeDirectionBeat;

    public Single Amplitude { get; set; } = 50f;
    public Single Period { get; set; } = 0f;

    private Boolean DoRise = false;
    public Single Time { get; set; } = 0f;
    private Single StartX = 0f;

    /// <summary>
    /// Ready 
    /// </summary>
    public override void _Ready()
    {
        MusicManager = GetNode<MusicManager>("/root/MusicManager");
        Animator = (AnimationPlayer)FindNode("AnimationPlayer");

        MusicManager.Beat += AnimateIn;
        MusicManager.SongChanged += MusicManager_SongChanged;
        SetPeriod(MusicManager.NowPlaying.BPM, MusicManager.NowPlaying.MeasureLength);

        CallDeferred(nameof(SetAnimationSpeed), args: MusicManager.NowPlaying.BPM);
        StartX = Position.x;
    }

    /// <summary>
    /// Process
    /// </summary>
    public override void _Process(Single delta)
    {
        if (DoRise is true)
        {
            var position = Position;

            position.y -= FloatSpeed * delta;
            
            position.x = StartX + Amplitude * (Single)Math.Sin(Period * Time * Math.PI);
            Time += delta;

            Position = position;

            if (GlobalPosition.y <= -50f)
            {
                QueueFree();
            }
        }
    }

    /// <summary>
    /// Exit Tree
    /// </summary>
    public override void _ExitTree()
    {
        MusicManager.Beat -= AnimateIn;
        MusicManager.SongChanged -= MusicManager_SongChanged;
    }

    /// <summary>
    /// Song changed
    /// </summary>
    private void MusicManager_SongChanged(object sender, SongChangedEventArgs e)
    {
        if (e.Song.BPM != e.LastSong.BPM)
        {
            SetAnimationSpeed(e.Song.BPM);
            SetPeriod(e.Song.BPM, e.Song.MeasureLength);
        }
    }

    private void SetPeriod(Single BPM, Int32 MeasureLength)
    {
        Period = BPM / MeasureLength / 60f;
        GD.Print(Period);
    }

    /// <summary>
    /// Updates animation speed to match BPM
    /// </summary>
    private void SetAnimationSpeed(Single BPM)
    {
        Animator.PlaybackSpeed = BPM / 10f;
    }

    /// <summary>
    /// Animates bubble in
    /// </summary>
    private void AnimateIn(object sender, BeatEventArgs e)
    {
        Animator.Play("Expand");
        Animator.Connect("animation_finished", this, nameof(StartFloating));
        MusicManager.Beat -= AnimateIn;
    }

    /// <summary>
    /// Starts floating when animation is finished
    /// </summary>
    private void StartFloating(String animationName)
    {
        DoRise = true;
    }
}
