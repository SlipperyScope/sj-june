using Godot;
using System;

public class PlayerMovementAllDirections : KinematicBody2D
{

	Vector2 target;
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
		if (Position.DistanceTo(target) > 5)
		{
			MoveAndSlide(Position.DirectionTo(target) * speed);
		}
	}
}
