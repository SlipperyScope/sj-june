using Godot;
using System;

public class TransitionTrigger : Clickable
{
    protected LevelManager LevelManager;

    [Export]
    public String TransitionName { get; private set; } = "Unnamed transition";

    [Export]
    public SceneID TransitionTo { get; private set; } = SceneID.MainMenu;

    /// <summary>
    /// Whether the transition point is useable
    /// </summary>
    [Export]
    public Boolean IsOpen
    {
        get => _IsOpen;
        set
        {
            if (value == _IsOpen)
            {
                return;
            }
            if (value is true)
            {
            }
            else
            {
                Close();
            }
            _IsOpen = value;
        }
    }
    private Boolean _IsOpen = false;

    private Sprite Sprite = null;

    /// <summary>
    /// Weady UwU
    /// </summary>
    public override void _Ready()
    {
        // Intentionally not running base._Ready();
        LevelManager = GetNode<LevelManager>("/root/LevelManager");
        if (IsOpen is true)
        {
            Open();
        }
        else
        {
            Close();
        }

        foreach (var child in GetChildren())
        {
            if (child is Sprite)
            {
                Sprite = child as Sprite;
                break;
            }
        }

        if (Sprite is null)
        {
            GD.PrintErr("Way to forget to put a sprite on it, ididoth");
        }
    }

    /// <summary>
    /// Opens the transition point
    /// </summary>
    public virtual void Open()
    {
        InputPickable = true;
        Connect("mouse_entered", this, nameof(_onMouseEntered));
        Connect("mouse_exited", this, nameof(_onMouseExited));
    }

    /// <summary>
    /// Closes the transition point
    /// </summary>
    public virtual void Close()
    {
        InputPickable = false;
        if (this.IsConnected("mouse_entered", this, nameof(_onMouseEntered))) {
            Disconnect("mouse_entered", this, nameof(_onMouseEntered));
            Disconnect("mouse_exited", this, nameof(_onMouseExited));
        }
    }

    public override void interact()
    {
        GD.Print("hey I interacted");
        LevelManager.LoadScene(TransitionTo);
    }
    public override void _onMouseEntered()
    {
        (Sprite.Material as ShaderMaterial)?.SetShaderParam("outlined", true);
        base._onMouseEntered();
    }

    public override void _onMouseExited()
    {
        (Sprite.Material as ShaderMaterial)?.SetShaderParam("outlined", false);
        base._onMouseExited();
    }
}
