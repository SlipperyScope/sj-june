using Godot;
using System;
using System.Collections.Generic;

public class Obstacle : Clickable
{
    [Export] Boolean consumeItem;
    [Export] String itemNeeded;
    [Export] protected String completionText = "Thanks for the pipe";
    [Export] protected String failureText = "I need something to beat off.....all these barnacles";
    [Export] protected String alreadyCompletedText = "It's for beating, not smoking";
    Sprite startSprite;
    protected Sprite completeSprite;

    AudioStreamPlayer2D SFXPlayer;

    public override void _Ready()
    {
        base._Ready();
        startSprite = GetNode<Sprite>("StartSprite");
        completeSprite = GetNode<Sprite>("CompleteSprite");
        SFXPlayer = GetNode<AudioStreamPlayer2D>("SFXPlayer");
        // startSprite.Visible = true;
        // completeSprite.Visible = false;
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
        if (SFXPlayer.Stream != null)
        {
            SFXPlayer.Play();
        }
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
