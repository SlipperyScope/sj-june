using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : KinematicBody2D
{

	Vector2 target;
	int speed = 300;
    List<String> itemNames = new List<String>();
    Hashtable itemTextures = new Hashtable();
    RichTextLabel textbox;
    RichTextLabel clickableName;
    public Clickable interactWhenClose = null;
	
	public override void _Ready()
	{
		target = Position;
        textbox = GetNode<RichTextLabel>("HUD/TextboxContainer/Textbox");
        clickableName = GetNode<RichTextLabel>("HUD/ClickableName");
	}

    public List<String> GetItemNames() {
        return itemNames;
    }

	public override void _Input(InputEvent ie)
	{
		if (ie.IsActionPressed("click"))
		{
            interactWhenClose = null;
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

    public void grabItem(String itemName, Texture itemTexture)
    {
        printMessage("got item: " + itemName);
        itemNames.Add(itemName);
        itemTextures[itemName] = itemTexture;
    }

    public void removeItem(String itemName)
    {
        itemTextures.Remove(itemName);
        itemNames.Remove(itemName);
        printMessage("removed item: " + itemName);
    }

    public void printMessage(String msg)
    {
        textbox.Text = msg + "\n" + textbox.Text;
    }

    public void setClickableName(String name)
    {
        clickableName.Text = name;
    }
}
