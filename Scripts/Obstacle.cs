using Godot;
using System;
using System.Collections.Generic;

public class Obstacle : Clickable
{
    [Export] Boolean consumeItem;
    [Export] String itemNeeded;
    [Export] String completionText = "Thanks for the pipe";
    [Export] String failureText = "I need something to beat off.....all these barnacles";
    [Export] String alreadyCompletedText = "It's for beating, not smoking";
    Sprite startSprite;
    Sprite completeSprite;
    Boolean completed = false;

    public override void _Ready()
    {
        base._Ready();
        startSprite = GetNode<Sprite>("StartSprite");
        completeSprite = GetNode<Sprite>("CompleteSprite");
        startSprite.Visible = true;
        completeSprite.Visible = false;
    }

    protected virtual void completeTask()
    {
        if (consumeItem)
        {
            getSteve().removeItem(itemNeeded);
        }
        completeSprite.Visible = true;
        startSprite.Visible = false;
        completed = true;
    }

    public override void interact() {
        String selectedItem = getSteve().getSelectedItem();
        if (completed) {
            getSteve().printMessage(alreadyCompletedText);
        } else if (selectedItem == itemNeeded)
        {
            completeTask();
            getSteve().printMessage(completionText);
        } else
        {
            getSteve().printMessage(failureText);
        }
    }

    public override void _onMouseEntered()
    {
        ((ShaderMaterial)startSprite.Material).SetShaderParam("outlined", true);
        ((ShaderMaterial)completeSprite.Material).SetShaderParam("outlined", true);
        base._onMouseEntered();
    }
    public override void _onMouseExited()
    {
        ((ShaderMaterial)startSprite.Material).SetShaderParam("outlined", false);
        ((ShaderMaterial)completeSprite.Material).SetShaderParam("outlined", false);
        base._onMouseExited();
    }
}
