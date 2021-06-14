using Godot;
using System;

public class Item : Clickable
{
    [Export] String itemName;
    Sprite itemSprite;

    public override void _Ready()
    {
        base._Ready();
        itemSprite = GetNode<Sprite>("ItemSprite");
    }

    public override void interact()
    {
        getSteve().grabItem(itemName,  itemSprite.Texture);
        InputPickable = false;
        itemSprite.Visible = false;
        GetNode<CollisionShape2D>("ItemCollider").Disabled = true;
    }
    public override void _onMouseEntered()
    {
        ((ShaderMaterial)itemSprite.Material).SetShaderParam("outlined", true);
    }

    public override void _onMouseExited()
    {
        ((ShaderMaterial)itemSprite.Material).SetShaderParam("outliend", false);
    }
}
