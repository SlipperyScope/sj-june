using Godot;
using System;

public class PlayerMovement : KinematicBody2D
{

	Vector2 target;
	Vector2 velocity = new Vector2(0, 0);
	int speed = 200;
	
	public override void _Ready()
	{
		target = Position;
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
		Vector2 xTarget = new Vector2(target.x, Position.y);
		if (Position.DistanceTo(xTarget) > 5)
		{
			velocity.x = speed * (target.x < Position.x ? -1 : 1);
			MoveAndSlideWithSnap(velocity, new Vector2(0, 100), new Vector2(0, -1));
		}
	}
}
