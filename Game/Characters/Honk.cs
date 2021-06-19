using Godot;
using System;

public class Honk : Button
{
    private AudioStreamPlayer Honker;
    public override void _Ready()
    {
        Honker = FindNode("Honker") as AudioStreamPlayer;
    }

    public void _on_Button_pressed()
    {
        Honker.Play();
    }
}
