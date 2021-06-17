using Audio;
using Godot;
using System;
using System.Collections.Generic;

public class LevelManager : Node
{
    private Dictionary<SceneID, Scene> Scenes = new Dictionary<SceneID, Scene>()
    {
        {
            SceneID.Surface, new Scene
            {
                Path = "res://Scenes/SteveGetsHisFins.tscn",
                Song = SongID.JourneyNorth
            }
        },
        {
            SceneID.Bottom, new Scene
            {
                Path = "",
                Song = SongID.Depths
            }
        },
        {
            SceneID.Inside, new Scene
            {
                Path = "",
                Song = SongID.DarkPassages
            }
        }
    };

    private MusicManager MusicManager;

    public override void _Ready()
    {
        MusicManager = GetNode<MusicManager>("/root/MusicManager");
    }

    public void LoadScene(SceneID id)
    {
        GetTree().ChangeScene(Scenes[id].Path);
        MusicManager.FadeTo(Scenes[id].Song);
    }
}
