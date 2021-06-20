using Godot;
using System;

public class Lockbox : Popup
{
	RichTextLabel[] combo = new RichTextLabel[5];
	[Export] String correctCombo = "00001";
	public override void _Ready()
	{
		combo[0] = GetNode<RichTextLabel>("Number1");
		combo[1] = GetNode<RichTextLabel>("Number2");
		combo[2] = GetNode<RichTextLabel>("Number3");
		combo[3] = GetNode<RichTextLabel>("Number4");
		combo[4] = GetNode<RichTextLabel>("Number5");
		
	}

	public void _onButtonPressed(int buttonNum, Boolean increase)
	{
		GD.Print("Button pressed: " + buttonNum + " :: " + increase);
		int digit = int.Parse(combo[buttonNum - 1].Text);
		if (increase) {
			if (++digit > 9)
				digit = 0;
		} else {
			if (--digit < 0)
				digit = 9;
		}

		combo[buttonNum - 1].Text = "" + digit;
	}

	public Boolean checkCombo()
	{
		String checking = "";
		for (var i = 0; i < combo.Length; i++)
		{
			checking += combo[i].Text.Trim();
		}

		return checking == correctCombo;
	}
}
