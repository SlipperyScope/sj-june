using Godot;
using System;
using Audio;

public class ItemButton : Button
{
    public SFXManager SFXManager;
    private PlayerData PlayerData;
    public override void _Ready()
    {
        SFXManager = this.GetManager<SFXManager>(Paths.SFXMangerPath);
        PlayerData = GetNode<PlayerData>("/root/PlayerData");
        Connect("mouse_entered", this, nameof(OnMouseEnter));
        Connect("pressed", this, nameof(OnPress));
    }

    private void OnPress()
    {
        SFXManager.PlaySFX(SFXID.UIUse, GetViewportTransform().origin + PlayerData.Reference.Position);
    }

    private void OnMouseEnter()
    {
        SFXManager.PlaySFX(SFXID.UIHover, GetViewportTransform().origin + PlayerData.Reference.Position);
    }
}
