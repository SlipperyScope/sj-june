using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : KinematicBody2D
{

	Vector2 target;
	int speed = 300;
    String selectedItem;
    List<String> itemNames = new List<String>();
    Hashtable itemCursors = new Hashtable();
    RichTextLabel textbox;
    RichTextLabel clickableName;
    GridContainer inventoryButtons;
    Camera2D camera;
    public Clickable interactWhenClose = null;
	
	public override void _Ready()
	{
		target = Position;
        textbox = GetNode<RichTextLabel>("HUD/TextboxContainer/Textbox");
        clickableName = GetNode<RichTextLabel>("HUD/ClickableName");
        inventoryButtons = GetNode<GridContainer>("HUD/InventoryButtons");
        Button button = inventoryButtons.GetNode<Button>("ItemButton/Button");
        button.Connect("pressed", this, nameof(itemButtonPressed), new Godot.Collections.Array {"", null});
        camera = GetNode<Camera2D>("Camera2D");
        Sprite background = GetParent().GetNode<Sprite>("TheBackground/background");
        setCameraLimit(background);
	}

    public List<String> GetItemNames() {
        return itemNames;
    }

    public void _onMovementZoneInputEvent(Node viewport, InputEvent ie, int shapeIdx)
    {
        if (ie.IsActionPressed("click"))
        {
            interactWhenClose = null;
            target = GetGlobalMousePosition();
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

    private void itemButtonPressed(String itemName, Texture itemTexture)
    {
        selectedItem = itemName;
        if (itemName == "") {
            Input.SetCustomMouseCursor(null);
            return;
        }
        Texture cursorTexture = (Texture)itemCursors[itemName];
        if (cursorTexture == null)
        {
            cursorTexture = ResourceLoader.Load<Texture>("res://Assets/cursors/" + itemName + ".png");
            itemCursors[itemName] = cursorTexture;
        }
        Input.SetCustomMouseCursor(cursorTexture);
    }

    public void grabItem(String itemName, Texture itemTexture)
    {
        printMessage("got item: " + itemName);
        itemNames.Add(itemName);

        var itemButton = ((PackedScene)GD.Load("res://Game/ItemButton.tscn")).Instance();
        itemButton.Name = itemName + "Button";
        Button button = itemButton.GetNode<Button>("Button");
        button.Connect("pressed", this, nameof(itemButtonPressed), new Godot.Collections.Array {itemName, itemTexture});

        Sprite buttonSprite = button.GetNode<Sprite>("ButtonSprite");
        buttonSprite.Texture = itemTexture;
        buttonSprite.Scale = new Vector2(button.RectSize.x / itemTexture.GetWidth(), button.RectSize.y / itemTexture.GetHeight());
        inventoryButtons.AddChild(itemButton);
    }

    public void removeItem(String itemName)
    {
        itemNames.Remove(itemName);
        inventoryButtons.RemoveChild(inventoryButtons.GetNode(itemName + "Button"));
        selectedItem = "";
        Input.SetCustomMouseCursor(null);
        printMessage("removed item: " + itemName);
    }

    public void printMessage(String msg)
    {
        textbox.Text = msg + "\n" + textbox.Text;
    }

    public void setClickableName(String name)
    {
        clickableName.Text = name;
    }

    public void setCameraLimit(Sprite background)
    {
        camera.LimitLeft = 0;
        camera.LimitTop = 0;
        camera.LimitRight = (int)background.GetRect().Size.x;
        camera.LimitBottom = (int)background.GetRect().Size.y + 420;
    }
}
