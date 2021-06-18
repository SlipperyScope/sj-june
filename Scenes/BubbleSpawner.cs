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

    private void MusicManager_Beat(object sender, EventArgs e)
    {
        if (MusicManager.BeatCount % 8 == Rand.Next(0,8))
        {
            var bubble = Bubble.Instance() as Node2D;
            bubble.Position = new Vector2(Rand.Next(-10, 11), Rand.Next(-10, 11));
            AddChild(bubble);
        }
    }
}