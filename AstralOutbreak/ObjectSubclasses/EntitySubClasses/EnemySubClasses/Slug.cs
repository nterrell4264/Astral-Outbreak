using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public enum SlugState { Moving, Falling, Idle }

    public class Slug : Enemy
    {
        private SlugState currentState;
        private bool awake;

        //prevY is a float used to store the previous Y value of the position, letting us know if the object is falling
        private float prevY;

        public SlugState CurrentSlugState
        {
            get { return currentState; }
            set
            {
                currentState = value;
                CurrentActionTime = 0;
            }
        }

        public Slug(Vector2 pos, float width, float height, float health, float damage = 1f, bool mobile = true) : base(pos, width, height, health, damage, mobile)
        {
            prevY = Velocity.Y;
            awake = false;
            Gravity = true;
        }

        public override void Step(float deltaTime)
        {
            if (awake)
            {
                switch (currentState)
                {
                    case SlugState.Moving:
                        if (Position.X >= RoomManager.Active.PlayerOne.Position.X - 10 && Position.X <= RoomManager.Active.PlayerOne.Position.X + 10 && Position.Y != RoomManager.Active.PlayerOne.Position.Y)
                        {
                            CurrentSlugState = SlugState.Idle;
                            Velocity.X = 0;
                            break;
                        }

                        if (FaceRight)
                        {
                            if (prevY > Velocity.Y)
                            {
                                CurrentSlugState = SlugState.Falling;
                                break;
                            }

                            if (RoomManager.Active.PlayerOne.Position.X + 10 < Position.X)
                            {
                                FaceRight = false;
                                break;
                            }
                            Velocity.X = 50;
                        }
                        else
                        {
                            if (prevY > Velocity.Y)
                            {
                                CurrentSlugState = SlugState.Falling;
                                break;
                            }

                            if (RoomManager.Active.PlayerOne.Position.X - 10 > Position.X)
                            {
                                FaceRight = true;
                                break;
                            }
                            Velocity.X = -50;
                        }

                        break;

                    case SlugState.Falling:
                        if (prevY == Velocity.Y)
                        {
                            if (RoomManager.Active.PlayerOne.Position.X + 10 < Position.X)
                            {
                                CurrentSlugState = SlugState.Moving;
                                FaceRight = false;
                                break;
                            }
                            else if (RoomManager.Active.PlayerOne.Position.X - 10 > Position.X)
                            {
                                CurrentSlugState = SlugState.Moving;
                                FaceRight = true;
                                break;
                            }

                            if(Velocity.X == 0)
                            {
                                CurrentSlugState = SlugState.Idle;
                                break;
                            }
                        }
                        else
                        {
                            if (RoomManager.Active.PlayerOne.Position.X + 10 < Position.X)
                            {
                                FaceRight = false;
                                break;
                            }
                            else if (RoomManager.Active.PlayerOne.Position.X - 10 > Position.X)
                            {
                                FaceRight = true;
                                break;
                            }
                        }
                        break;

                    case SlugState.Idle:
                        if (RoomManager.Active.PlayerOne.Position.X + 10 < Position.X)
                        {
                            CurrentSlugState = SlugState.Moving;
                            FaceRight = false;
                            break;
                        }
                        else if (RoomManager.Active.PlayerOne.Position.X - 10 > Position.X)
                        {
                            CurrentSlugState = SlugState.Moving;
                            FaceRight = true;
                            break;
                        }

                        if (prevY > Velocity.Y)
                        {
                            CurrentSlugState = SlugState.Falling;
                        }
                        break;

                    default:
                        CurrentSlugState = SlugState.Idle;
                        break;
                }
            }

            else
            {
                awake = CheckLineOfSight(RoomManager.MapData);
            }

            ////Checks if the player is to the left of the Slug, and will move towards the player
            //if (RoomManager.Active.PlayerOne.Position.X < Position.X)
            //{
            //    currentState = SlugState.MovingLeft;
            //    FaceRight = false;
            //    Velocity.X = -50;
            //}
            ////Checks if the player is to the right of the Slug, and will move towards the player
            //else if (RoomManager.Active.PlayerOne.Position.X > Position.X)
            //{
            //    currentState = SlugState.MovingRight;
            //    FaceRight = true;
            //    Velocity.X = 50;
            //}
            ////If the the previous y position is larger than the current, sets the Slug to falling, move this to the top of the if statements to give this state priority
            //else if (prevY > Position.Y)
            //{
            //    currentState = SlugState.Falling;
            //}
            base.Step(deltaTime);
            prevY = Velocity.Y;
        }
    }
}
