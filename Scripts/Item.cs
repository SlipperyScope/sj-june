using Godot;
using System;

public class Item : StaticBody2D
{
    [Export] String itemName;
    Sprite itemSprite;
    Player steve;
    int activationDistance = 300;

    public override void _Ready()
    {
        InputPickable = true;
        itemSprite = GetNode<Sprite>("ItemSprite");
        steve = GetParent().GetNode<Player>("ScubaSteve");
        this.Connect("mouse_entered", this, nameof(_onMouseEntered));
        this.Connect("mouse_exited", this, nameof(_onMouseExited));
    }

    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
		if (@event.IsActionPressed("click") && steve.Position.DistanceTo(Position) < 300)
		{
            //probably don't want to actually pass the sprite, but it's a placeholder for now
            steve.grabItem(itemName,  itemSprite);
            InputPickable = false;
            itemSprite.Visible = false;
            GetNode<CollisionShape2D>("ItemCollider").Disabled = true;
        }
    }
  private void _onMouseEntered() 
    {
        if (steve.Position.DistanceTo(Position) < activationDistance)
        ((ShaderMaterial)itemSprite.Material).SetShaderParam("outLineSize", .02);
    }
    private void _onMouseExited() 
    {
        ((ShaderMaterial)itemSprite.Material).SetShaderParam("outLineSize", .00);
    }
}
