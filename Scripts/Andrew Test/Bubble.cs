using Audio;
using Godot;
using System;

public class Bubble : Node2D
{
    private MusicManager MusicManager;
    private AnimationPlayer Animator;
    private Random Rand = new Random();
    private Int32 ChangeDirectionBeat;

    [Export]
    public Single FloatSpeed { get; private set; } = 100f;

    [Export]
    public Single ShiftSpeed { get; private set; } = 30f;

    private Single Direction = 1f;
    private Boolean DoRise = false;

    /// <summary>
    /// Ready 
    /// </summary>
    public override void _Ready()
    {
        MusicManager = GetNode<MusicManager>("/root/MusicManager");
        Animator = (AnimationPlayer)FindNode("AnimationPlayer");

        MusicManager.Beat += AnimateIn;
        MusicManager.SongChanged += MusicManager_SongChanged;

        FloatSpeed += Rand.Next(-5, 6);
        ShiftSpeed += Rand.Next(-5, 6);
        ChangeDirectionBeat = Rand.Next(0, MusicManager.NowPlaying.MeasureLength);

        CallDeferred(nameof(SetAnimationSpeed), args: MusicManager.NowPlaying.BPM);
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
            position.x += Direction * ShiftSpeed * delta;
            Position = position;
            if (GlobalPosition.y <= -50f)
            {
                MusicManager.Beat -= ChangeDirection;
                QueueFree();
            }
        }
    }

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
        }
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
        //GD.Print("Animation finished");
        MusicManager.Beat += ChangeDirection;
        DoRise = true;
    }

    /// <summary>
    /// Syncronizes direction change with beat
    /// </summary>
    private void ChangeDirection(object sender, BeatEventArgs e)
    {
        if (e.BeatInMeasure == ChangeDirectionBeat)
        {
            Direction *= -1;
        }
    }
}
