using Audio;
using Godot;
using System;

public class BubbleSpawner : Node2D
{
    const String BubblePath = "res://Scripts/Andrew Test/Bubble.tscn";

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

    private void MusicManager_Beat(object sender, EventArgs e)
    {
        if (MusicManager.BeatCount % 6 == Rand.Next(0,6))
        {
            var bubble = Bubble.Instance() as Node2D;
            bubble.Position = new Vector2(Rand.Next(-100, 101), Rand.Next(-100, 101));
            AddChild(bubble);
        }
    }
}
