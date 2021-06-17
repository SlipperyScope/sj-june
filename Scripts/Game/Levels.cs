using Audio;
using Godot;
using System;
using System.Collections.Generic;


public enum SceneID
{
    MainMenu,
    Surface,
    Bottom,
    Inside
}

public struct Scene
{
    public String Path;
    public SongID Song;
}
