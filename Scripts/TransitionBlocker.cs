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

     protected override void completeTask()
     {
         GetParent().GetNode<TransitionTrigger>(transitionName).Open();
         if (base.completeSprite.Texture == null) {
             GD.Print("get rid of shit");
             InputPickable = false;
         }
         base.completeTask();
     }
}
