using Godot;
using System;

namespace Remaster.Audio
{

    public class SoundEffectPlayer2D : Node2D
    {
        /// <summary>
        /// Player reference
        /// </summary>
        public AudioStreamPlayer2D Player { get; private set; } = new AudioStreamPlayer2D();

        /// <summary>
        /// True if the player is playing
        /// </summary>
        public Boolean Playing => Player?.Playing ?? false;

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            AddChild(Player);
        }

        /// <summary>
        /// Plays a sound effect
        /// </summary>
        /// <param name="soundEffect"></param>
        public void PlaySound(SoundEffect soundEffect)
        {
            Player.VolumeDb = soundEffect.PlaybackVolume;
            Player.Stream = GD.Load<AudioStream>(soundEffect.File);
            Player.Play();
            GD.Print($"Playing {soundEffect.Name}");
        }

        /// <summary>
        /// Plays a sound at a specific position
        /// </summary>
        /// <param name="soundEffect">Sound effect to play</param>
        /// <param name="position">Global position</param>
        public void PlaySoundAt(SoundEffect soundEffect, Vector2 position)
        {
            GlobalPosition = position;
            PlaySound(soundEffect);
        }

        /// <summary>
        /// Stops sound effect playback
        /// </summary>
        public void Stop() => Player?.Stop();
    }
}
