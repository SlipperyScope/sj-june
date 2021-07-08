using Godot;
using System;

namespace Remaster.Characters
{
    public class Movement : Node
    {
        /// <summary>
        /// Relative path to the character this node controls
        /// </summary>
        [Export]
        public NodePath CharacterPath { get; private set; }

        [Export]
        public Single MoveSpeed { get; private set; } = 100f;

        /// <summary>
        /// Character being controlled
        /// </summary>
        public Character Character { get; private set; }


        private Vector2 TargetLocation = Vector2.Zero;

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            Character = GetNode<Character>(CharacterPath);
        }

        /// <summary>
        /// Physics Process
        /// </summary>
        public override void _PhysicsProcess(Single delta)
        {
            if (Character.Position != TargetLocation)
            {
                Character.MoveAndSlide((TargetLocation - Character.Position).Normalized() * MoveSpeed);
            }
        }

        /// <summary>
        /// Attempts to move the character to the location
        /// </summary>
        /// <param name="location">Location to move to</param>
        public void MoveTo(Vector2 location) => TargetLocation = location;


    }
}
