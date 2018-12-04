using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondInTheWater.Entities.Minigame
{
    public class ScrollingWater
    {
        private int timer;
        private Game1 game;
        private const int TIME = 16;

        public ScrollingWater(Game1 game)
        {
            this.game = game;
        }

        public void Update(GameTime gameTime)
        {
            timer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > 64 * TIME)
            {
                timer -= 64 * TIME;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int size = 64;
            for (int y = -1; y < game.Height / size + 1; y++)
            {
                for (int x = -1; x < game.Width / size + 1; x++)
                {
                    Tile.tiles[1].Draw(spriteBatch, x * size, y * size + timer / TIME, size, size);
                }
            }
        }
    }
}
