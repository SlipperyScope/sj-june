using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : KinematicBody2D
{

	Vector2 target;
	int speed = 300;
    List<String> itemNames = new List<String>();
    Hashtable itemSprites = new Hashtable();
	
	public override void _Ready()
	{
		target = Position;
	}

    public List<String> GetItemNames() {
        return itemNames;
    }

	public override void _Input(InputEvent ie)
	{
		if (ie.IsActionPressed("click"))
		{
			target = GetGlobalMousePosition();
			GetNode<Sprite>("PlayerSprite").FlipH = target.x < Position.x;
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		if (Position.DistanceTo(target) > 5)
		{
			MoveAndSlide(Position.DirectionTo(target) * speed);
		}
	}

    public void grabItem(String itemName, Sprite itemSprite)
    {
        GD.Print("got item: " + itemName);
        itemNames.Add(itemName);
        itemSprites[itemName] = itemSprite;
    }
}
