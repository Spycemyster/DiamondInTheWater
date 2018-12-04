using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiamondInTheWater.Entities.Minigame
{
    public class PhillipsProjectile : Projectile
    {
        private float timer;
        public PhillipsProjectile(Texture2D texture, Rectangle initialRectangle, int TTL)
            : base(texture, initialRectangle, TTL)
        {
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timer += dt;
            base.Update(gameTime);

            velocity = new Vector2(2, (float)(Math.Pow((timer + 700)/ 10000000, -2)) / 10000000);
        }
    }
}
