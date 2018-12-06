using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiamondInTheWater.Entities.Minigame
{
    public class LafferProjectile : Projectile
    {
        private float timer;
        public LafferProjectile(Texture2D texture, Rectangle initialRectangle, int TTL) : base(texture, initialRectangle, TTL)
        {
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            base.Update(gameTime);
            velocity = new Vector2(6, -Game1.HEIGHT * 10 * (float)( Math.PI / Game1.WIDTH / 2.8f * Math.Cos(timer * Math.PI / Game1.WIDTH / 2.8f)));
        }
    }
}
