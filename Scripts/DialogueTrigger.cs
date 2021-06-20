using Godot;
using System;

public class DialogueTrigger : Clickable
{
    [Export] String dialogueText = "This is very interesting";

    public override void interact() {
        getSteve().printMessage(dialogueText);
    }
}