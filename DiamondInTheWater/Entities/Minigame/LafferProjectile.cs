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
            int time = 10;
            float timeC = (timer / time);
            position = new Vector2(timeC, (float)((3 * Math.Pow(Game1.WIDTH, 2) - 4 * Math.Pow(timeC, 2) + 4 * timeC * Game1.WIDTH) / (4 * Math.Pow(Game1.WIDTH, 2))));
            //position = new Vector2((timer ) / time + Game1.WIDTH / 2, 
            //    -(float)Math.Sqrt(Math.Max(Math.Pow(Game1.HEIGHT, 2) - 
            //    Math.Pow((timer - Game1.WIDTH / 2) / time, 2), 0)) + Game1.HEIGHT);
            //velocity = new Vector2(5, (float)((2 * timer + Game1.WIDTH) / (2 * Math.Sqrt(Math.Pow(Game1.HEIGHT, 2) - Math.Pow(timer, 2) + timer * Game1.WIDTH - Math.Pow(Game1.WIDTH, 2) / 4))));
        }
    }
}
