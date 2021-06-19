using Audio;
using Godot;
using System;

public class BubbleSpawner : Node2D
{
    private Random rand = new Random();

    public enum Speed
    {
        Fast,
        Normal,
        Slow
    }

    const String BubblePath = "res://Scripts/Andrew Test/Bubble.tscn";

    [Export]
    public Single SpawnDistance { get; private set; } = 100f;

    [Export]
    public Speed SpawnSpeed { get; private set; } = Speed.Normal;

    private MusicManager MusicManager;
    private Random Rand = new Random();
    private PackedScene Bubble;

    public override void _Ready()
    {
        MusicManager = MusicManager = GetNode<MusicManager>("/root/MusicManager");
        MusicManager.Beat += MusicManager_Beat;
        Bubble = GD.Load<PackedScene>(BubblePath);
    }

    public override void _ExitTree()
    {
        MusicManager.Beat -= MusicManager_Beat;
    }

    private void MusicManager_Beat(object sender, BeatEventArgs e)
    {
        Double chance = 0.5;
        switch (SpawnSpeed)
        {
            case Speed.Fast:
                chance = 1;
                break;
            case Speed.Normal:
                chance = 0.5;
                break;
            case Speed.Slow:
                chance = 0.25;
                break;
        }
        if (Rand.NextDouble() < chance)
        {
            var bubble = Bubble.Instance() as Bubble;
            bubble.Position = GetRandomSpawnPosition();
            bubble.Amplitude = (Single)Rand.NextDouble() * 100f - 50f;
            AddChild(bubble);
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        Single distance = (Single)Rand.NextDouble() * SpawnDistance;
        var angle = Rand.NextDouble() * 2f * Math.PI;
        return new Vector2((Single)Math.Cos(angle), (Single)Math.Sin(angle)) * distance;
    }
}
