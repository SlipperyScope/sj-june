using Godot;
using System;
using System.Collections.Generic;

namespace Audio
{
    public class SFXManager : Node
    {
        public PlayerData PlayerData;

        private Dictionary<SFXID, Sound> SFX = new Dictionary<SFXID, Sound>()
        {
            {
                SFXID.UIHover, new Sound()
                {
                    Name = "UIHover",
                    Path = "res://Audio/Sfx/UIHover.mp3",
                    PlaybackVolume = 0f,
                    Bus = SFXBus.UI
                }
            },
            {
                SFXID.UIUse, new Sound()
                {
                    Name = "UIUse",
                    Path = "res://Audio/Sfx/Click.mp3",
                    PlaybackVolume = -15f,
                    Bus = SFXBus.UI
                }
            },
            {
                SFXID.C4, new Sound()
                {
                    Name = "C4",
                    Path = "res://Audio/Sfx/Explosion.mp3",
                    PlaybackVolume = 5f,
                    Bus = SFXBus.Effect
                }
            },
            {
                SFXID.PipePickup, new Sound()
                {
                    Name = "Pickup pipe",
                    Path = "res://Audio/Sfx/PipePickup.mp3",
                    PlaybackVolume = -5f,
                    Bus = SFXBus.Effect
                }
            },
            {
                SFXID.CrabPipe, new Sound()
                {
                    Name = "Give Crab Pipe",
                    Path = "res://Audio/Sfx/CrabPipe.mp3",
                    PlaybackVolume = -10f,
                    Bus = SFXBus.Effect
                }
            },
            {
                SFXID.Box, new Sound()
                {
                    Name = "Box grabbing sound",
                    Path = "res://Audio/Sfx/Box.mp3",
                    PlaybackVolume = 0f,
                    Bus = SFXBus.Effect
                }
            }
        };
        
        private AudioStreamPlayer2D UI = new AudioStreamPlayer2D();
        private AudioStreamPlayer2D Effect = new AudioStreamPlayer2D();
        private AudioStreamPlayer2D Background = new AudioStreamPlayer2D();
        private AudioStreamPlayer2D Generic = new AudioStreamPlayer2D();

        public override void _Ready()
        {
            PlayerData = GetNode<PlayerData>("/root/PlayerData");
            AddChild(UI);
            AddChild(Effect);
            AddChild(Background);
            AddChild(Generic);
        }

        public void PlaySFX(SFXID id, Vector2 position) => PlaySFX(id, position, id == SFXID.None ? SFXBus.Generic : SFX[id].Bus);

        public void PlaySFX(SFXID id, Vector2 position, SFXBus busOverride)
        {
            if (id == SFXID.None) return;
            var sound = SFX[id];

            AudioStreamPlayer2D bus;
            switch (busOverride)
            {
                case SFXBus.UI:
                    bus = UI;
                    break;
                case SFXBus.Effect:
                    bus = Effect;
                    break;
                case SFXBus.Background:
                    bus = Background;
                    break;
                default:
                    bus = Generic;
                    break;
            }

            bus.Stream = GD.Load<AudioStreamMP3>(sound.Path);
            bus.VolumeDb = sound.PlaybackVolume;
            var transform = PlayerData.Reference.Transform;
            transform.origin = position;
            bus.Transform = transform;
            bus.Play();
            GD.Print($"Playing {sound.Name} at {transform}");
        }
    }
}
