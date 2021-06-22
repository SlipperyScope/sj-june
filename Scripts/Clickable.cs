using Audio;
using Godot;
using System;
using System.Collections.Generic;

public class Clickable : StaticBody2D
{
	protected SFXManager SFXManager;

	[Export] String name = "Something you can click on";
	[Export]
	public SFXID HoverSound { get; private set; } = SFXID.UIHover;
	[Export]
	public SFXID ClickSound { get; private set; } = SFXID.UIUse;

    Player steve;
	protected Boolean completed = false;
	int activationDistance = 300;

	public override void _Ready()
	{
		// InputPickable = true;
		this.Connect("mouse_entered", this, nameof(_onMouseEntered));
		this.Connect("mouse_exited", this, nameof(_onMouseExited));
		SFXManager = this.GetManager<SFXManager>(Paths.SFXMangerPath);
	}

	public void setSteve(Player theSteve)
	{
		steve = theSteve;
	}
	public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
	{
		if (@event.IsActionPressed("click"))
		{
			SFXManager?.PlaySFX(ClickSound, GlobalPosition);
			steve.interactWhenClose = this;
			GetTree().SetInputAsHandled();
		}
	}

	public override void _Process(float delta)
	{
		if (steve.interactWhenClose == this && steve.Position.DistanceTo(Position) < activationDistance)
		{
			steve.interactWhenClose = null;
			interact();
		}
	}

	public Player getSteve() {
		return steve;
	}

	private void completeTask() {}

	public virtual void interact() {}

	public virtual void _onMouseEntered()
	{
		steve.setClickableName(name);
		SFXManager?.PlaySFX(HoverSound, GlobalPosition);
	}
	public virtual void _onMouseExited()
	{
		steve.setClickableName("");
	}
}
