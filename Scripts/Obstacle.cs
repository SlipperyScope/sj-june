
using Godot;
using System;
using System.Collections.Generic;

public class Obstacle : StaticBody2D
{
    [Export] String itemNeeded;
    [Export] String completionText = "Nice";
    [Export] String failureText = "Nope";
    [Export] String alreadyCompletedText = "Nice Again";
    Sprite startSprite;
    Sprite completeSprite;
    Player steve;
    Boolean completed = false;
    int activationDistance = 300;

    public override void _Ready()
    {
        InputPickable = true;
        startSprite = GetNode<Sprite>("StartSprite");
        completeSprite = GetNode<Sprite>("CompleteSprite");
        startSprite.Visible = true;
        completeSprite.Visible = false;
        steve = GetParent().GetNode<Player>("ScubaSteve");
        this.Connect("mouse_entered", this, nameof(_onMouseEntered));
        this.Connect("mouse_exited", this, nameof(_onMouseExited));
    }

    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
		if (@event.IsActionPressed("click") && steve.Position.DistanceTo(Position) < activationDistance)
		{
            GD.Print(interact());
        }
    }

    private void completeTask()
    {
        completeSprite.Visible = true;
        startSprite.Visible = false;
        completed = true;
    }

    public String interact() {
        List<String> items = steve.GetItemNames();
        if (completed) {
            return alreadyCompletedText;
        }
        if (items.Contains(itemNeeded))
        {
            completeTask();
            return completionText;
        }
        return failureText;
    }

    private void _onMouseEntered()
    {
        if (steve.Position.DistanceTo(Position) < activationDistance) {

            ((ShaderMaterial)startSprite.Material).SetShaderParam("outlined", true);
            ((ShaderMaterial)completeSprite.Material).SetShaderParam("outlined", true);
        }
    }
    private void _onMouseExited()
    {
        ((ShaderMaterial)startSprite.Material).SetShaderParam("outlined", false);
        ((ShaderMaterial)completeSprite.Material).SetShaderParam("outlined", false);
    }
}
