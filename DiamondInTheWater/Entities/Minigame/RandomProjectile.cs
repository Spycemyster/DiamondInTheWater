using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiamondInTheWater.Entities.Minigame
{
    public class RandomProjectile : Projectile
    {
        public float Rotation;
        public float Speed;

        public Color Color;

        public RandomProjectile(Texture2D texture, Rectangle initialRectangle, int TTL) 
            : base(texture, initialRectangle, TTL)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            velocity = new Vector2((float)Math.Cos(Rotation) * Speed, (float)Math.Sin(Rotation) * Speed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Color);
        }
    }
}
