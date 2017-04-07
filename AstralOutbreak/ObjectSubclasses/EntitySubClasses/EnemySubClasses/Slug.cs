using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak.ObjectSubclasses.EntitySubClasses.EnemySubClasses
{
    public enum SlugState { MovingRight, MovingLeft, Falling }

    class Slug : Enemy
    {
        private SlugState currentState;

        //prevY is a float used to store the previous Y value of the position, letting us know if the object is falling
        private float prevY;

        public SlugState CurrentState { get { return currentState; } }

        public Slug(Vector2 pos, float width, float height, float health, float damage = 1f, bool mobile = true) : base(pos, width, height, health, damage, mobile)
        {
            prevY = pos.Y;
        }

        public override void Step(float deltaTime)
        {
            //Checks if the player is to the left of the Slug, and will move towards the player
            if (RoomManager.Active.PlayerOne.Position.X < Position.X)
            {
                currentState = SlugState.MovingLeft;
                FaceRight = false;
                Position.X -= 1;
            }
            //Checks if the player is to the right of the Slug, and will move towards the player
            else if (RoomManager.Active.PlayerOne.Position.X > Position.X)
            {
                currentState = SlugState.MovingRight;
                FaceRight = true;
                Position.X += 1;
            }
            //If the the previous y position is larger than the current, sets the Slug to falling, move this to the top of the if statements to give this state priority
            else if (prevY > Position.Y)
            {
                currentState = SlugState.Falling;
            }
            base.Step(deltaTime);
            prevY = Position.Y;
        }
    }
}
