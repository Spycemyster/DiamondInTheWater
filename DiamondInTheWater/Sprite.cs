using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondInTheWater
{
    public abstract class Sprite
    {
        public const int SPRITE_SCALE = 3;
        public Texture2D Texture
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public abstract void Initialize(ContentManager Content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
