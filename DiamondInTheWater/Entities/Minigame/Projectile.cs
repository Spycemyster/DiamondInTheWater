using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DiamondInTheWater.Entities.Minigame
{
    public class Projectile : Sprite
    {
        public int TTL
        {
            get { return ttl; }
        }
        private Texture2D texture;
        protected Vector2 velocity, position;
        private Rectangle drawRectangle;
        private int ttl;

        public Projectile(Texture2D texture, Rectangle initialRectangle, int TTL)
        {
            ttl = TTL;
            this.texture = texture;
            drawRectangle = initialRectangle;
            position = drawRectangle.Location.ToVector2();
        }

        public Rectangle GetCollisionRectangle()
        {
            return drawRectangle;
        }

        public override void Initialize(ContentManager Content)
        {
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            ttl -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            drawRectangle = new Rectangle((int)position.X, (int)position.Y, drawRectangle.Width, drawRectangle.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Color.Blue);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(texture, drawRectangle, color);
        }

    }
}
