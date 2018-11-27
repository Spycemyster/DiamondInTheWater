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
    public class Map
    {
        private int[][] tiles;
        public const int TILE_SCALE = 2;

        public Map(int width, int height)
        {
            tiles = new int[height][];

            for (int i = 0; i < height; i++)
            {
                tiles[i] = new int[width];
            }

            InitAllTiles(1);
        }

        public void InitAllTiles(int id)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    tiles[i][j] = id;
                }
            }

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int tileSize = 32 * TILE_SCALE;
            for (int y = 0; y < tiles.Length; y++)
            {
                for (int x = 0; x < tiles[y].Length; x++)
                {
                    int id = tiles[y][x];
                    if (id == 0)
                        continue;
                    else
                        Tile.tiles[tiles[y][x]].Draw(spriteBatch,
                            x * tileSize, y * tileSize, tileSize, tileSize);
                }
            }
        }
    }

    public class Tile
    {
        public static Tile[] tiles = new Tile[2];

        public static void Init(ContentManager Content)
        {
            tiles[1] = new Tile(Content.Load<Texture2D>("WaterSheet"), 250f, 6);

        }

        public static void Update(GameTime gameTime)
        {
            foreach (Tile t in tiles)
                t?.UpdateTile(gameTime);
        }

        private Texture2D texture;
        private float timer, frameTime;
        private int frames, currentFrame;

        public Tile(Texture2D texture, float frameTime = 0f, int frames = 1)
        {
            this.frames = frames;
            this.frameTime = frameTime;
            timer = 0f;
            currentFrame = 0;
            this.texture = texture;
        }

        private void UpdateTile(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer >= frameTime)
            {
                timer = 0;
                currentFrame++;
                if (currentFrame >= frames)
                    currentFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y, int width, int height)
        {
            spriteBatch.Draw(texture, new Rectangle(x, y, width, height), 
                new Rectangle(32 * currentFrame, 0, 32, 32), Color.White);
        }
    }
}
