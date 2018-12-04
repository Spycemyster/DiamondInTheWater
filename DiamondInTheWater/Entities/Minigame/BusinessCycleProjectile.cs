using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiamondInTheWater.Entities.Minigame
{
    public class BusinessCycleProjectile : Projectile
    {
        private float timer;

        public BusinessCycleProjectile(Texture2D texture, Rectangle initialRectangle, int ttl)
            : base(texture, initialRectangle, ttl)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            float a = 700;
            velocity = new Vector2(2.75f, 3 * (float)(Math.Cos(timer / a)) - .6f);
        }
    }
}
