using Godot;
using System;
using System.Collections.Generic;

public class TransitionBlocker : Obstacle
{
    [Export] String transitionName;
    // [Export] String completionText = "";  
    // [Export] String failureText = "";
    // [Export] String alreadyCompletedText = "";
    // Sprite startSprite;
    // Sprite completeSprite;
    //PlayerMovementAllDirections steve;

    //Boolean completed = false;
    [Export]
    public String AnimationName { get; private set; } = "No animation set";
    protected override void completeTask()
     {
         TransitionTrigger trigger = GetParent().GetNode<TransitionTrigger>(transitionName);
         trigger.IsOpen = true;
         trigger.Open();
         if (base.completeSprite.Texture == null) {
             GD.Print("get rid of shit");
             InputPickable = false;
         }
         base.completeTask();
        foreach (var child in GetChildren())
        {
            if (child is AnimationPlayer)
            {
                (child as AnimationPlayer).Play(AnimationName);
            }
        }
     }
}
