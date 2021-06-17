using Godot;
using System;



public class Crossfader : Node
{
    const Single LowState = -45f;
    const Single HighState = -10f;

    public enum STREAM { A, B };

    [Export]
    public Single FadeTime { get; private set; } = 2f;

    [Export]
    public AudioStreamMP3 StreamA;

    [Export]
    public AudioStreamMP3 StreamB;

    private AudioStreamPlayer PlayerA = new AudioStreamPlayer();
    private AudioStreamPlayer PlayerB = new AudioStreamPlayer();

    private STREAM FadeTo = STREAM.A;
    private Boolean Fading = false;

    public override void _Ready()
    {
        StreamA.Loop = true;
        StreamB.Loop = true;

        PlayerA.Stream = StreamA;
        PlayerA.Autoplay = true;
        PlayerA.VolumeDb = HighState;

        PlayerB.Stream = StreamB;
        PlayerB.Autoplay = true;
        PlayerB.VolumeDb = LowState;

        AddChild(PlayerA);
        AddChild(PlayerB);

        PlayerB.StreamPaused = true;
    }

    public override void _Process(Single delta)
    {
        if (Input.IsActionJustPressed("ui_select"))
        {
            BeginFade(FadeTo == STREAM.A ? STREAM.B : STREAM.A);
        }

        if (Fading is true)
        {
            switch (FadeTo)
            {
                case STREAM.A:
                    //GD.Print($"Current: {PlayerA.VolumeDb} Alpha:{CurrentAlpha(LowState, HighState, PlayerA.VolumeDb)}");
                    PlayerA.VolumeDb = Interp(LowState, HighState, CurrentAlpha(LowState, HighState, PlayerA.VolumeDb) + delta / FadeTime);
                    PlayerB.VolumeDb = Interp(HighState, LowState, CurrentAlpha(HighState, LowState, PlayerB.VolumeDb) + delta / FadeTime);
                    
                    if (PlayerA.VolumeDb > HighState)
                    {
                        PlayerA.VolumeDb = HighState;
                    }
                    if (PlayerB.VolumeDb < LowState)
                    {
                        PlayerB.VolumeDb = LowState;
                    }

                    if (PlayerA.VolumeDb == HighState && PlayerB.VolumeDb == LowState)
                    {
                        Fading = false;
                        PlayerB.StreamPaused = true;
                    }
                    break;
                case STREAM.B:
                    //GD.Print($"Current: {PlayerA.VolumeDb} Alpha:{CurrentAlpha(HighState, LowState, PlayerA.VolumeDb)}");
                    PlayerA.VolumeDb = Interp(HighState, LowState, CurrentAlpha(HighState, LowState, PlayerA.VolumeDb) + delta / FadeTime);
                    PlayerB.VolumeDb = Interp(LowState, HighState, CurrentAlpha(LowState, HighState, PlayerB.VolumeDb) + delta / FadeTime);

                    if (PlayerA.VolumeDb < LowState)
                    {
                        PlayerA.VolumeDb = LowState;
                    }
                    if (PlayerB.VolumeDb > HighState)
                    {
                        PlayerB.VolumeDb = HighState;
                    }

                    if (PlayerA.VolumeDb == LowState && PlayerB.VolumeDb == HighState)
                    {
                        Fading = false;
                        PlayerA.StreamPaused = true;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void BeginFade(STREAM fadeTo)
    {
        FadeTo = fadeTo;
        if (FadeTo == STREAM.A)
        {
            PlayerA.VolumeDb = LowState;
            PlayerA.StreamPaused = false;
        } else
        {
            PlayerB.VolumeDb = LowState;
            PlayerB.StreamPaused = false;
        }
        Fading = true;
    }

    private Single Interp(Single a, Single b, Single alpha) => a + (b - a) * alpha;
    private Single CurrentAlpha(Single a, Single b, Single current) => (current - a) / (b - a);
}
