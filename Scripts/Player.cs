using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : KinematicBody2D
{

	Vector2 target;
	int speed = 300;
    String selectedItem;
    Hashtable itemCursors = new Hashtable();
    RichTextLabel textbox;
    RichTextLabel clickableName;
    GridContainer inventoryButtons;
    Camera2D camera;
    PlayerData playerData;
    public Clickable interactWhenClose = null;
	
	public override void _Ready()
	{
        playerData = GetNode<PlayerData>("/root/PlayerData");
        playerData.Reference = this;
		target = Position;

        textbox = GetNode<RichTextLabel>("HUD/TextboxContainer/Textbox");
        textbox.Text = playerData.getTextHistory();

        clickableName = GetNode<RichTextLabel>("HUD/ClickableName");

        inventoryButtons = GetNode<GridContainer>("HUD/InventoryButtons");
        Button button = inventoryButtons.GetNode<Button>("ItemButton/Button");
        button.Connect("pressed", this, nameof(itemButtonPressed), new Godot.Collections.Array {"", null, ""});

        foreach (var name in playerData.getItemNames())
        {
            addItemButton(name, playerData.getItem(name));
        }

        camera = GetNode<Camera2D>("Camera2D");
        Sprite background = GetParent().GetNode<Sprite>("TheBackground");
        setCameraLimit(background);

        GetParent().GetNode("MovementZone").Connect("input_event", this, nameof(_onMovementZoneInputEvent));

        foreach(var child in GetParent().GetChildren())
        {
            if (child is Clickable)
            {
                ((Clickable)child).setSteve(this);

            }
        }
	}

    public void _onMovementZoneInputEvent(Node viewport, InputEvent ie, int shapeIdx)
    {
        if (ie.IsActionPressed("click"))
        {
            if (!GetTree().IsInputHandled())
            {
                interactWhenClose = null;
            }
            target = GetGlobalMousePosition() + new Vector2(0f, 200f);
            GetNode<Sprite>("PlayerSprite").FlipH = target.x < Position.x;
        }
    }

	public override void _PhysicsProcess(float delta)
	{
		if (Position.DistanceTo(target) > 5)
		{
			MoveAndSlide(Position.DirectionTo(target) * speed);
		}
	}

    public String getSelectedItem() 
    {
        return selectedItem;
    }

    private void itemButtonPressed(String itemName, Texture itemTexture, String itemDescription = "")
    {
        //print text
        selectedItem = itemName;
        if (itemDescription != "") {
            printMessage(itemDescription);
        }
        // if (itemName == "") {
        //     Input.SetCustomMouseCursor(null);
        //     return;
        // }
        // Texture cursorTexture = (Texture)itemCursors[itemName];
        // if (cursorTexture == null)
        // {
        //     cursorTexture = ResourceLoader.Load<Texture>("res://Assets/cursors/" + itemName + ".png");
        //     itemCursors[itemName] = cursorTexture;
        // }
        // Input.SetCustomMouseCursor(cursorTexture);
    }

    // public void grabItem(String itemName, Texture itemTexture, String itemDescription = "")
    public void grabItem(String itemName, ItemInfo itemInfo)
    {
        printMessage("got item: " + itemName);
        playerData.addItem(itemName, itemInfo);
        addItemButton(itemName, itemInfo);
    }

    private void addItemButton(String itemName, ItemInfo itemInfo)
    {
        var itemButton = ((PackedScene)GD.Load("res://Game/ItemButton.tscn")).Instance();
        itemButton.Name = itemName + "Button";
        Button button = itemButton.GetNode<Button>("Button");
        button.Connect("pressed", this, nameof(itemButtonPressed), new Godot.Collections.Array {itemName, itemInfo.texture, itemInfo.description});

        Sprite buttonSprite = button.GetNode<Sprite>("ButtonSprite");
        buttonSprite.Texture = itemInfo.texture;
        buttonSprite.Scale = new Vector2(button.RectSize.x / itemInfo.texture.GetWidth(), button.RectSize.y / itemInfo.texture.GetHeight());
        inventoryButtons.AddChild(itemButton);
    }

    public void removeItem(String itemName)
    {
        playerData.removeItem(itemName);
        inventoryButtons.RemoveChild(inventoryButtons.GetNode(itemName + "Button"));
        selectedItem = "";
        Input.SetCustomMouseCursor(null);
        printMessage("removed item: " + itemName);
    }

    public void printMessage(String msg)
    {
        playerData.prependTextHistory(msg);
        textbox.Text = playerData.getTextHistory();
    }

    public void setClickableName(String name)
    {
        clickableName.Text = name;
    }

    public void setCameraLimit(Sprite background)
    {
        camera.LimitLeft = 0;
        camera.LimitTop = 0;
        camera.LimitRight = (int)(background.GetRect().Size.x * background.Scale.x);
        camera.LimitBottom = (int)(background.GetRect().Size.y * background.Scale.y) + 280;
    }
}
