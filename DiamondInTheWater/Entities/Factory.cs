using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DiamondInTheWater.Entities
{
    public class Factory : Sprite
    {
        private Rectangle drawRect;
        public Factory(Vector2 position)
        {
            Position = position;
        }

        public override void Initialize(ContentManager Content)
        {
            Texture = Content.Load<Texture2D>("Factory");
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            drawRect = new Rectangle(Position.ToPoint(), new Point(Texture.Width * SPRITE_SCALE, Texture.Height * SPRITE_SCALE));
            spriteBatch.Draw(Texture, drawRect, Color.White);
        }
    }
}
