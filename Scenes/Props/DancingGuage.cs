using Audio;
using Godot;
using System;
using System.Collections.Generic;

public class DancingGuage : Node2D
{
    private enum Animation
    {
        Wobble,
        Pulse
    }

    private Dictionary<Animation, String> Animations = new Dictionary<Animation, string>()
    {
        { Animation.Wobble, "Wobble" },
        { Animation.Pulse, "Pulse" }
    };

    private MusicManager MusicManager;
    private Sprite Needle;

    public override void _Ready()
    {
        Needle = GetNode<Sprite>("Needle");
        MusicManager = GetNode<MusicManager>("/root/MusicManager");
        MusicManager.Beat += MusicManager_Beat;
    }

    private void MusicManager_Beat(object sender, EventArgs e)
    {
        if (MusicManager.BeatCount % 16 == 0)
        {

        }
    }

    private void PlayRandomAnimation()
    {

    }
}
