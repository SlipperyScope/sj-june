using Godot;
using System;

namespace Remaster.Characters
{
    public class Controller : Node
    {
        [Export]
        public NodePath CharacterPath { get; private set; }

        [Export]
        public NodePath MovementPath { get; private set; }

        public Character Character { get; private set; }
        public Movement Movement { get; private set; }

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            Character = GetNode<Character>(CharacterPath);
            Movement = GetNode<Movement>(MovementPath);
        }
    }
}
