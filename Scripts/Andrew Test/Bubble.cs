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
    public Single FloatSpeed { get; private set; } = 30f;

    [Export]
    public Single ShiftSpeed { get; private set; } = 15f;

    private Single Direction = 1f;
    private Boolean DoRise = false;

    public override void _Ready()
    {
        MusicManager = GetNode<MusicManager>("/root/MusicManager");
        Animator = (AnimationPlayer)FindNode("AnimationPlayer");
        MusicManager.Beat += MusicManager_Beat;
        FloatSpeed += Rand.Next(-5, 6);
        ShiftSpeed += Rand.Next(-5, 6);
        ChangeDirectionBeat = Rand.Next(0, 4);
        //GD.Print($"beat: {ChangeDirectionBeat}");
    }

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

    private void MusicManager_Beat(object sender, EventArgs e)
    {
        Animator.Play("Expand");
        Animator.Connect("animation_finished", this, nameof(OnAnimationFinished));
        MusicManager.Beat -= MusicManager_Beat;
        DoRise = true;
    }

    private void OnAnimationFinished(String animationName)
    {
        //GD.Print("Animation finished");
        MusicManager.Beat += ChangeDirection;
    }

    private void ChangeDirection(object sender, EventArgs e)
    {
        if (MusicManager.BeatCount % 4 == ChangeDirectionBeat)
        {
            Direction *= -1;
        }
    }

    private void Rise()
    {
        
    }
}
