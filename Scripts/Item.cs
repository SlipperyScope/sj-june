using Godot;
using System;

public class Item : Clickable
{
    [Export] String itemName;
    [Export] String itemDescription;
    Sprite itemSprite;

    public override void _Ready()
    {
        base._Ready();
        itemSprite = GetNode<Sprite>("ItemSprite");
    }

    public override void interact()
    {
        ItemInfo info = new ItemInfo{texture = itemSprite.Texture, description = itemDescription};
        getSteve().grabItem(itemName, info);
        InputPickable = false;
        itemSprite.Visible = false;
        GetNode<CollisionShape2D>("ItemCollider").Disabled = true;
    }
    public override void _onMouseEntered()
    {
        ((ShaderMaterial)itemSprite.Material).SetShaderParam("outlined", true);
        base._onMouseEntered();
    }

    public override void _onMouseExited()
    {
        ((ShaderMaterial)itemSprite.Material).SetShaderParam("outlined", false);
        base._onMouseExited();
    }
}

public struct ItemInfo
{
    public Texture texture;
    public String description;

}
