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
                Path = "res://Scenes/Levels/Submarine.tscn",
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
                Path = "res://Scenes/Levels/Hatch.tscn",
                Song = SongID.Depths
            }
        },
        {
            SceneID.UpperHatch, new Scene
            {
                Path = "res://Scenes/Levels/UpperHatch.tscn",
                Song = SongID.Depths
            }
        },
        {
            SceneID.LowerHatch, new Scene
            {
                Path = "res://Scenes/Levels/LowerHatch.tscn",
                Song = SongID.Depths
            }
        },
        {
            SceneID.CargoBay, new Scene
            {
                Path = "res://Scenes/Levels/CargoBay.tscn",
                Song = SongID.DarkPassages
            }
        },
        {
            SceneID.LivingQuarters, new Scene
            {
                Path = "res://Scenes/Levels/LivingQuarters.tscn",
                Song = SongID.DarkPassages
            }
        },
        {
            SceneID.ShipSurface, new Scene
            {
                Path = "res://Scenes/Levels/ShipSurface.tscn",
                Song = SongID.DarkPassages
            }
        },
        {
            SceneID.MoreSurface, new Scene
            {
                Path = "res://Scenes/Levels/MoreSurface.tscn",
                Song = SongID.DarkPassages
            }
        },
        {
            SceneID.Hose, new Scene
            {
                Path = "res://Scenes/Levels/Hose.tscn",
                Song = SongID.DarkPassages
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

        foreach (var child in GetTree().CurrentScene.GetChildren())
        {
            var button = child as DebugPlayButton;
            if (button != null)
            {
                button.Connect("pressed", this, nameof(LoadScene), new Godot.Collections.Array() {button.Scene});
            }
        }
    }

    /// <summary>
    /// Loads a scene
    /// </summary>
    /// <param name="id">Scene ID</param>
    public void LoadScene(SceneID id)
    {
        var leavingScene = Scenes[CurrentScene];
        var packed = new PackedScene();
        var savingError = packed.Pack(GetTree().CurrentScene);
        if (savingError == Error.Ok) {
            leavingScene.SavedScene = packed;
            Scenes[CurrentScene] = leavingScene;
        }

        LastScene = CurrentScene;
        CurrentScene = id;

        var scene = Scenes[id];
        if (scene.SavedScene != null) {
            GetTree().ChangeSceneTo(scene.SavedScene);
        } else {
            GetTree().ChangeScene(scene.Path);
        }
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
            if (spawnPoint != null && spawnPoint.PreviousScene == LastScene)
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
