using Godot;
using System;

namespace Remaster.Audio
{
    public class SoundEffectPlayer : Node
    {
        /// <summary>
        /// Player reference
        /// </summary>
        public AudioStreamPlayer Player { get; private set; } = new AudioStreamPlayer();

        /// <summary>
        /// True if player is playing
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
        /// <param name="soundEffect">Sound effect to play</param>
        public void PlaySound(SoundEffect soundEffect)
        {
            Player.VolumeDb = soundEffect.PlaybackVolume;
            Player.Stream = GD.Load<AudioStream>(soundEffect.File);
            Player.Play();
            GD.Print($"Playing {soundEffect.Name}");
        }

        /// <summary>
        /// Stops a sound effect
        /// </summary>
        public void Stop() => Player?.Stop();
    }
}
