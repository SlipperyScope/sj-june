using Godot;
using System;

public class SpawnPoint : Node
{
    [Export]
    public SceneID PreviousScene { get; private set; } 
}
