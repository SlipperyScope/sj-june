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
            SceneID.Surface, new Scene
            {
                Path = "res://Scenes/SteveGetsHisFins.tscn",
                Song = SongID.JourneyNorth
            }
        },
        {
            SceneID.Bottom, new Scene
            {
                Path = "res://Scenes/Levels/ShipEntrance.tscn",
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
    private PlayerData PlayerData;
    private PackedScene PlayerScene;

    public SceneID CurrentScene = SceneID.MainMenu;
    public SceneID LastScene = SceneID.MainMenu;

    public override void _Ready()
    {
        MusicManager = GetNode<MusicManager>("/root/MusicManager");
        PlayerData = GetNode<PlayerData>("/root/PlayerData");
        PlayerScene = GD.Load<PackedScene>(SteveScenePath);
    }

    public void LoadScene(SceneID id)
    {
        var scene = Scenes[id];
        GetTree().ChangeScene(scene.Path);
        MusicManager.ChangeSong(scene.Song);
        CallDeferred(nameof(SpawnPlayer));
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
