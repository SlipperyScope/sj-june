using Godot;
using System;

public class DebugPlayButton : Button
{
    [Export]
    public SceneID Scene { get; private set; } = SceneID.MainMenu;
}
