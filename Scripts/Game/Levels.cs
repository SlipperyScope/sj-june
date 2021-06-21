using Audio;
using Godot;
using System;
using System.Collections.Generic;


public enum SceneID
{
    MainMenu,
    Submarine,
    ShipEntrance,
    Hatch,
    UpperHatch,
    LowerHatch,
    CargoBay,
    LivingQuarters,
    ShipSurface,
    MoreSurface,
    Hose,
    EndScene
}

public struct Scene
{
    public String Path;
    public SongID Song;
    public SteveID Steve;
    public PackedScene SavedScene;
}
