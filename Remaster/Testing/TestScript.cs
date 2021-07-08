using Godot;
using Remaster.Audio;
using System;

namespace Remaster
{
    public class TestScript : Node
    {
        [Export]
        public SoundEffect SoundEffect { get; private set; }

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            GetNode<SoundEffectPlayer>("SFXPlayer")?.PlaySound(SoundEffect);
        }
    } 
}
