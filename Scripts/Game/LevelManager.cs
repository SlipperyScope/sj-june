using Audio;
using Godot;
using System;
using System.Collections.Generic;

public class LevelManager : Node
{
    const String SteveScenePath = "res://Game/Characters/ScubaSteve.tscn";

    private Dictionary<SceneID, Scene> Scenes = new Dictionary<SceneID, Scene>()
    {
        {
            SceneID.MainMenu, new Scene
            {
                Path = "res://Scenes/MainScene.tscn",
                Song = SongID.JustPressPlay
            }
        },
        {
            SceneID.Submarine, new Scene
            {
                Path = "res://Scenes/SteveGetsHisFins.tscn",
                Song = SongID.JourneyNorth
            }
        },
        {
            SceneID.ShipEntrance, new Scene
            {
                Path = "res://Scenes/Levels/ShipEntrance.tscn",
                Song = SongID.Descent
            }
        },
        {
            SceneID.Hatch, new Scene
            {
                Path = "",
                Song = SongID.Depths
            }
        }
    };

    private MusicManager MusicManager;
    private PlayerData PlayerData;
    private PackedScene PlayerScene;

    public SceneID CurrentScene = SceneID.MainMenu;
    public SceneID LastScene = SceneID.MainMenu;

    /// <summary>
    /// Ready
    /// </summary>
    public override void _Ready()
    {
        MusicManager = GetNode<MusicManager>("/root/MusicManager");
        PlayerData = GetNode<PlayerData>("/root/PlayerData");
        PlayerScene = GD.Load<PackedScene>(SteveScenePath);
        MusicManager.ChangeSong(Scenes[SceneID.MainMenu].Song);
    }

    /// <summary>
    /// Loads a scene
    /// </summary>
    /// <param name="id">Scene ID</param>
    public void LoadScene(SceneID id)
    {
        LastScene = CurrentScene;
        CurrentScene = id;

        var scene = Scenes[id];

        GetTree().ChangeScene(scene.Path);
        MusicManager.ChangeSong(scene.Song);

        if (id != SceneID.MainMenu)
        {
            CallDeferred(nameof(SpawnPlayer));
        }
    }

    /// <summary>
    /// Spawns a player at the proper spawn point
    /// </summary>
    private void SpawnPlayer()
    {
        var scene = GetTree().CurrentScene;
        var player = PlayerScene.Instance<Player>();

        SpawnPoint spawn = null;
        foreach (var child in scene.GetChildren())
        {
            var spawnPoint = child as SpawnPoint;
            if (spawnPoint != null && spawnPoint.PreviousScene == SceneID.MainMenu)
            {
                spawn = spawnPoint;
                break;
            }
        }

        player.Position = spawn?.Position ?? new Vector2(100f, 100f);
        player.Scale = spawn?.Scale ?? Vector2.One;
        // Change steve type sprite

        scene.AddChild(player);
    }
}
