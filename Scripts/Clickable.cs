using Godot;
using System;
using System.Collections.Generic;

public class Clickable : StaticBody2D
{
    [Export] String name = "Something you can click on";
    Player steve;
    Boolean completed = false;
    int activationDistance = 300;

    public override void _Ready()
    {
        InputPickable = true;
        this.Connect("mouse_entered", this, nameof(_onMouseEntered));
        this.Connect("mouse_exited", this, nameof(_onMouseExited));
    }

    public void setSteve(Player theSteve)
    {
        steve = theSteve;
    }
    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
		if (@event.IsActionPressed("click"))
		{
            GD.Print("Clicked clickable");
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
    }
    public virtual void _onMouseExited()
    {
        steve.setClickableName("");
    }
}
