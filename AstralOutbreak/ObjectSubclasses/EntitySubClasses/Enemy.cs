using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public enum EnemyType { Slug, }

    public class Enemy : Entity
    {
        public int OriginX { get; set; }
        public int OriginY { get; set; }

        public Enemy(Vector2 pos, float width, float height, float health, bool mobile = true) : base(pos, width, height, health, mobile)
        {
        }

        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);

        }
    }
}
