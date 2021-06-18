using Godot;
using System;
using System.Collections.Generic;

public enum SteveID
{
    Scuba
}

public class PlayerData : Node
{
    public Dictionary<SteveID, String> StevePaths = new Dictionary<SteveID, string>()
    {
        { SteveID.Scuba, "res://Game/Characters/ScubaSteve.tscn" }
    };
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
