using Godot;
using System;

public class MainMenu : Node
{
    public void OnButtonPressed()
    {
        var levelManager = GetNode<LevelManager>("/root/LevelManager");
        levelManager.LoadScene(SceneID.Surface);
    }
}
