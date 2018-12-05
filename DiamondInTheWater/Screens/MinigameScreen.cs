using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiamondInTheWater.Entities.Minigame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DiamondInTheWater.Screens
{
    public class MinigameScreen : Screen
    {
        public double GameTimer
        {
            get { return timer / 1000.0; }
        }
        private Game1 game;
        private double timer, spawnTimer, rSpawnTimer;
        private ScrollingWater water;
        private List<Projectile> projectiles;
        private Texture2D blank;
        private Random rand;
        private Song background;
        private SpriteFont font;
        private MinigameState state;
        private Player player;
        private enum MinigameState
        {
            NONE,
            BEGIN,
            AGGREGATE_DEMAND_SUPPLY,
            BUSINESS_CYCLE,
            PHILLIPS,
            KEYNESIAN_CROSS,
            LAFFERS_CURVE,
            SUPPLY_DEMAND,
            RECESSION_DEPRESSION,
            INFLATION,
            STAGFLATION,
            ECONOMIC_BOOM,
            END,
        }

        /// <summary>
        /// Creates a new instance of the <c>MinigameScreen</c>.
        /// </summary>
        /// <param name="game"></param>
        public MinigameScreen(Game1 game)
        {
            rand = new Random();
            projectiles = new List<Projectile>();
            water = new ScrollingWater(game);
            this.game = game;
            state = MinigameState.NONE;
        }

        public override void Initialize(ContentManager Content)
        {
            player = new Player(projectiles);
            player.Initialize(Content);
            font = Content.Load<SpriteFont>("largeFont");
            blank = Content.Load<Texture2D>("blank");
            background = Content.Load<Song>("M2");
            MediaPlayer.Play(background);
        }

        public override void Unload()
        {
        }

        public RandomProjectile GenerateProjectile(int x, int y, int size)
        {
            Color c = new Color(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
            RandomProjectile rp = new RandomProjectile(blank, new Rectangle(x, y, size, size), 10000)
            {
                Speed = 1.5f,
                Rotation = (float)(rand.NextDouble() * Math.PI),
                Color = c,
            };

            return rp;
        }

        public void SpawnRandom(int safeDistance)
        {
            int size = 10 + rand.Next(0, 10);
            int x = rand.Next(-size, game.Width);
            int y = rand.Next(-size, game.Height);

            while (Vector2.Distance(player.Position, new Vector2(x, y)) <= safeDistance)
            {
                x = rand.Next(-size, game.Width);
                y = rand.Next(-size, game.Height);
            }
            RandomProjectile rp = new RandomProjectile(blank, new Rectangle(x, y, size, size), 10000)
            {
                Speed = 2.5f,
                Rotation = (float)(rand.NextDouble() * Math.PI * 2),
                Color = new Color(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256))
            };
            projectiles.Add(rp);
        }

        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            rSpawnTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            spawnTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            water.Update(gameTime);
            Tile.Update(gameTime);
            player.Update(gameTime);

            
            if (timer > 8000 && rSpawnTimer > 200)
            {
                rSpawnTimer = 0;
                SpawnRandom(350);
                RandomProjectile p = GenerateProjectile(game.Width / 5, 0, 10);
                RandomProjectile p2 = GenerateProjectile(game.Width / 5 * 2, 0, 10);
                RandomProjectile p3 = GenerateProjectile(game.Width / 5 * 3, 0, 10);
                RandomProjectile p4 = GenerateProjectile(game.Width / 5 * 4, 0, 10);

                projectiles.Add(p);
                projectiles.Add(p2);
                projectiles.Add(p3);
                projectiles.Add(p4);
            }

            int initialCount = projectiles.Count;

            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update(gameTime);
                if (projectiles[i].TTL <= 0)
                    projectiles.RemoveAt(i--);

                i += Math.Max(Math.Min(0, projectiles.Count - initialCount), 0);
            }

            if (timer < 8000)
            {
                state = MinigameState.BEGIN;
            }
            else if (timer <= 8000)
            {
                state = MinigameState.NONE;
            }
            else if (timer > 8000 && timer < 14000)
            {
                state = MinigameState.KEYNESIAN_CROSS;
                if (spawnTimer > 100)
                {
                    spawnTimer = 0;
                    projectiles.Add(new FortyFiveProjectile(blank, new Rectangle(0, game.Height, 10, 10), 9000));
                    projectiles.Add(new KeynesianCross(blank, new Rectangle(0, game.Height / 3 * 2, 10, 10), 9000));
                }
            }
            else if (timer > 15000 && timer < 28500)
            {
                state = MinigameState.AGGREGATE_DEMAND_SUPPLY;
                if (spawnTimer > 100)
                {
                    spawnTimer = 0;
                    projectiles.Add(new LRASProjectile(blank, new Rectangle(
                        game.Width / 2, 0, 10, 10), 5000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(0, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(game.Width, game.Height * 3 / 4, 10, 10), 10000));
                }
            }
            else if (timer > 32000 && timer < 46500)
            {
                state = MinigameState.BUSINESS_CYCLE;
                if (spawnTimer > 100)
                {
                    spawnTimer = 0;
                    projectiles.Add(new BusinessCycleProjectile(blank, new Rectangle(
                        10, game.Height / 2, 10, 10), 10000));
                }
            }
            else if (timer > 50000 && timer < 53000)
            {
                state = MinigameState.PHILLIPS;

                if (spawnTimer > 32)
                {
                    spawnTimer = 0;
                    projectiles.Add(new PhillipsProjectile(blank, new Rectangle(50, 0, 10, 10), 18000));
                }

            }
            else if (timer > 56000 && timer < 60000)
            {
                state = MinigameState.SUPPLY_DEMAND;

                if (spawnTimer > 100)
                {
                    spawnTimer = 0;
                    projectiles.Add(new ASProjectile(blank, new Rectangle(0, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(game.Width, game.Height * 3 / 4, 10, 10), 10000));
                }
            }
            else if (timer > 63000 && timer < 68000)
            {
                state = MinigameState.RECESSION_DEPRESSION;

                if (spawnTimer > 100)
                {
                    spawnTimer = 0;
                    // recession/depression
                    projectiles.Add(new LRASProjectile(blank, new Rectangle(
                        game.Width / 2, 0, 10, 10), 5000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(0, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(game.Width, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(game.Width - 300, game.Height * 3 / 4 + 300, 10, 10), 10000));
                }
            }
            else if (timer >= 68000 && timer < 72000)
            {
                state = MinigameState.INFLATION;

                if (spawnTimer > 100)
                {
                    spawnTimer = 0;
                    projectiles.Add(new LRASProjectile(blank, new Rectangle(
                        game.Width / 2, 0, 10, 10), 5000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(0, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(game.Width, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(game.Width + 200, game.Height * 3 / 4 - 100, 10, 10), 10000));
                }
            }
            else if (timer >= 72000 && timer < 90000)
            {
                state = MinigameState.STAGFLATION;
                if (spawnTimer > 100)
                {
                    spawnTimer = 0;
                    projectiles.Add(new LRASProjectile(blank, new Rectangle(
                        game.Width / 2, 0, 10, 10), 5000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(0, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(-200, game.Height * 3 / 4 - 200, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(game.Width, game.Height * 3 / 4, 10, 10), 10000));
                }
            }
            else if (timer > 90000 && timer < 95000)
            {
                state = MinigameState.ECONOMIC_BOOM;
                if (spawnTimer > 100)
                {
                    spawnTimer = 0;
                    projectiles.Add(new LRASProjectile(blank, new Rectangle(
                        game.Width / 2, 0, 10, 10), 5000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(0, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(200, game.Height * 3 / 4 + 200, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(game.Width, game.Height * 3 / 4, 10, 10), 10000));
                }
            }
            else if (spawnTimer > 98000 && spawnTimer < 101000)
            {
                state = MinigameState.END;
            }
            else if (spawnTimer > 101000)
            {
                GameManager.GetInstance().ChangeScreen(ScreenState.MENU);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.PointWrap, null, null, null, null);
            water.Draw(spriteBatch);
            foreach (Projectile p in projectiles)
                p.Draw(spriteBatch);

            player.Draw(spriteBatch);

            string text = "";

            if (!state.Equals(MinigameState.END) && !state.Equals(MinigameState.BEGIN))
            {
                switch (state)
                {
                    case MinigameState.AGGREGATE_DEMAND_SUPPLY:
                        text = "Aggregate Demand, Aggregate Supply, and Aggregate Demand Curve";
                        break;
                    case MinigameState.BUSINESS_CYCLE:
                        text = "The Business Cycle";
                        break;
                    case MinigameState.PHILLIPS:
                        text = "The Phillips Curve";
                        break;
                    case MinigameState.KEYNESIAN_CROSS:
                        text = "The Keynesian Cross";
                        break;
                    case MinigameState.LAFFERS_CURVE:
                        text = "The Laffer's Curve";
                        break;
                    case MinigameState.SUPPLY_DEMAND:
                        text = "The Supply and Demand Curve";
                        break;
                    case MinigameState.RECESSION_DEPRESSION:
                        text = "During a Recession/Depression";
                        break;
                    case MinigameState.INFLATION:
                        text = "During Inflation";
                        break;
                    case MinigameState.STAGFLATION:
                        text = "During Stagflation";
                        break;
                    case MinigameState.ECONOMIC_BOOM:
                        text = "During Economic Boom";
                        break;
                }
                Vector2 position = new Vector2(game.Width - font.MeasureString(text).X - 32, game.Height - font.MeasureString(text).Y - 32);
                spriteBatch.DrawString(font, text, position + new Vector2(-2, 2), Color.Black * 0.6f);
                spriteBatch.DrawString(font, text, position, Color.White);
            }
            else if (state == MinigameState.BEGIN)
            {
                text = "Bonus Minigame!";
                Vector2 position = new Vector2(game.Width / 2 - font.MeasureString(text).X / 2, game.Height / 2 - font.MeasureString(text).Y / 2);

                spriteBatch.DrawString(font, text, position + new Vector2(-3, 3), Color.Black * 0.6f);
                spriteBatch.DrawString(font, text, position, Color.White);
            }
            else if (state == MinigameState.END)
            {
                text = "Thanks for playing!";
                Vector2 position = new Vector2(game.Width / 2 - font.MeasureString(text).X / 2, game.Height / 2 - font.MeasureString(text).Y / 2);

                spriteBatch.DrawString(font, text, position + new Vector2(-3, 3), Color.Black * 0.6f);
                spriteBatch.DrawString(font, text, position, Color.White);
            }

            spriteBatch.DrawString(font, "Made by Spencer Chang", new Vector2(8, 8), Color.White * 0.3f, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Meglovania - Undertale", new Vector2(8, game.Height - 40), Color.White * 0.9f, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }
}
