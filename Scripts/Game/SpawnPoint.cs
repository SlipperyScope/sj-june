using Godot;
using System;

public class SpawnPoint : Node2D
{
    [Export]
    public SceneID PreviousScene { get; private set; } 
}
