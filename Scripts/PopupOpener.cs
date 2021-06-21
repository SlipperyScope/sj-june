using Godot;
using System;

public class PopupOpener : Obstacle
{
    Lockbox popup;
    public override void _Ready()
    {
        base._Ready();
        popup = GetParent().GetNode<Lockbox>("Popup");
    }

    public override void interact()
    {
        if (completed) {
            getSteve().printMessage(alreadyCompletedText);
        } else {
            getSteve().Visible = false;
            popup.Popup_();
        }
    }

    public void _tryToOpenBox()
    {
        if (popup.checkCombo()) {
            getSteve().printMessage(completionText);
            getSteve().grabItem("knife", new ItemInfo{texture = completeSprite.Texture, description = "A knife. It doesn't look very sharp"});
            getSteve().Visible = true;
            completed = true;
            popup.Hide();
        } else
        {
            getSteve().printMessage(failureText);
        }
    }

    public void _closePopup()
    {
        getSteve().Visible = true;
        popup.Hide();
    }

}
