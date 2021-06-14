using Godot;
using System;
using System.Collections.Generic;

public class Clickable : StaticBody2D
{
    Player steve;
    Boolean completed = false;
    int activationDistance = 300;

    public override void _Ready()
    {
        InputPickable = true;
        steve = GetParent().GetNode<Player>("ScubaSteve");
        this.Connect("mouse_entered", this, nameof(_onMouseEntered));
        this.Connect("mouse_exited", this, nameof(_onMouseExited));
    }

    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
		if (@event.IsActionPressed("click"))
		{
            steve.interactWhenClose = this;
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

    public virtual void _onMouseEntered() {}
    public virtual void _onMouseExited() {}
}
