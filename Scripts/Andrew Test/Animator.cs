using Godot;
using System;

public class Animator : AnimationPlayer
{
    public void OnDiveButtonDown()
    {
        if (IsPlaying() is false)
        {
            CurrentAnimation = "Down";
            Play();
        }
    }

    public void OnRiseButtonDown()
    {
        if (IsPlaying() is false)
        {
            CurrentAnimation = "Up";
            Play();
        }
    }
}
