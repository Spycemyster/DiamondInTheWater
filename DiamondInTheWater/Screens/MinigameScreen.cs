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
        private float endTimer;
        private double timer, spawnTimer, rSpawnTimer;
        private ScrollingWater water;
        private List<Projectile> projectiles;
        private Texture2D blank, money;
        private Random rand;
        private Song background, background2;
        private SpriteFont font;
        private MinigameState state;
        private Player player;
        private Boss boss;
        string songName;
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
            BOSS,
            END,
        }
        private bool isPlayFlag;

        /// <summary>
        /// Creates a new instance of the <c>MinigameScreen</c>.
        /// </summary>
        /// <param name="game"></param>
        public MinigameScreen(Game1 game)
        {
            isPlayFlag = false;
            rand = new Random();
            projectiles = new List<Projectile>();
            water = new ScrollingWater(game);
            this.game = game;
            state = MinigameState.NONE;
            timer = 0;
        }

        public override void Initialize(ContentManager Content)
        {
            player = new Player(projectiles);
            boss = new Boss(projectiles);
            boss.Initialize(Content);
            player.Initialize(Content);
            font = Content.Load<SpriteFont>("largeFont");
            blank = Content.Load<Texture2D>("blank");
            money = Content.Load<Texture2D>("Money");
            background = Content.Load<Song>("M2");
            background2 = Content.Load<Song>("M1");
            songName = "Megalovania - Undertale";
            MediaPlayer.Play(background);
        }

        public override void Unload()
        {
        }

        public RandomProjectile GenerateProjectile(int x, int y, int size, Color co)
        {
            Color c = (co.Equals(Color.Transparent)) ? new Color(rand.Next(0, 256), 
                rand.Next(0, 256), rand.Next(0, 256)) : co;
            RandomProjectile rp = new RandomProjectile(blank, new Rectangle(x, y, size, size), 10000)
            {
                Speed = 3.5f,
                Rotation = (float)(rand.NextDouble() * Math.PI),
                Color = c,
            };

            return rp;
        }

        public void SpawnRandom(int safeDistance)
        {
            int size = 5 + rand.Next(5, 12);
            int x = rand.Next(-size, game.Width + size);
            int y = rand.Next(-size, game.Height + size);

            while (Vector2.Distance(player.Position, new Vector2(x, y)) <= safeDistance)
            {
                x = rand.Next(-size, game.Width + size);
                y = rand.Next(-size, game.Height + size);
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
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            float dv = 0.0004f;
            water.Update(gameTime);
            Tile.Update(gameTime);
            player.Update(gameTime);
            float bossFightTimer = (float)(timer - (124000 + 4 / dv + 64));

            if (player.Health <= 0)
            {
                GameManager.GetInstance().ChangeScreen(ScreenState.MENU);
            }

            int t = (int)(2 / dv + 16);
            int rSpawnTimerThresh = (state.Equals(MinigameState.BOSS)) ? 300 : 200;

            if (timer > 8000 && rSpawnTimer > rSpawnTimerThresh && boss.Health > 0)
            {
                rSpawnTimer = 0;
                if (!state.Equals(MinigameState.BOSS))
                    SpawnRandom(350);
                Color c = (state.Equals(MinigameState.BOSS)) ? Color.Red : Color.Transparent;
                RandomProjectile p = GenerateProjectile(game.Width / 5, 0, 10, c);
                RandomProjectile p2 = GenerateProjectile(game.Width / 5 * 2, 0, 10, c);
                RandomProjectile p3 = GenerateProjectile(game.Width / 5 * 3, 0, 10, c);
                RandomProjectile p4 = GenerateProjectile(game.Width / 5 * 4, 0, 10, c);

                projectiles.Add(p);
                projectiles.Add(p2);
                projectiles.Add(p3);
                projectiles.Add(p4);
            }

            int initialCount = projectiles.Count;

            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update(gameTime);
                i += Math.Max(Math.Min(0, projectiles.Count - initialCount), 0);
                if (projectiles[Math.Min(i, projectiles.Count - 1)].TTL <= 0)
                    projectiles.RemoveAt(i--);
                i += Math.Max(Math.Min(0, projectiles.Count - initialCount), 0);

            }

            if (timer < 8000)
            {
                state = MinigameState.BEGIN;
            }
            else if (timer > 8000 && timer < 14000)
            {
                state = MinigameState.KEYNESIAN_CROSS;
                if (spawnTimer > 200)
                {
                    spawnTimer = 0;
                    projectiles.Add(new FortyFiveProjectile(blank, new Rectangle(
                        0, game.Height, 10, 10), 9000));
                    projectiles.Add(new KeynesianCross(blank, new Rectangle(
                        0, game.Height / 3 * 2, 10, 10), 9000));
                }
            }
            else if (timer > 15000 && timer < 28500)
            {
                state = MinigameState.AGGREGATE_DEMAND_SUPPLY;
                if (spawnTimer > 200)
                {
                    spawnTimer = 0;
                    projectiles.Add(new LRASProjectile(blank, new Rectangle(
                        game.Width / 2, 0, 10, 10), 5000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(
                        0, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(
                        game.Width, game.Height * 3 / 4, 10, 10), 10000));
                }
            }
            else if (timer > 32000 && timer < 46500)
            {
                state = MinigameState.BUSINESS_CYCLE;
                if (spawnTimer > 200)
                {
                    spawnTimer = 0;
                    projectiles.Add(new BusinessCycleProjectile(blank, new Rectangle(
                        0, game.Height / 4 * 3, 10, 10), 10000));
                }
            }
            else if (timer > 50000 && timer < 62000)
            {
                state = MinigameState.PHILLIPS;

                if (spawnTimer > 200)
                {
                    spawnTimer = 0;
                    projectiles.Add(new PhillipsProjectile(blank, new Rectangle(50, 0, 10, 10), 18000));
                }

            }
            else if (timer > 64000 && timer < 78000)
            {
                state = MinigameState.SUPPLY_DEMAND;

                if (spawnTimer > 200)
                {
                    spawnTimer = 0;
                    projectiles.Add(new ASProjectile(blank, new Rectangle(
                        0, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(
                        game.Width, game.Height * 3 / 4, 10, 10), 10000));
                }
            }
            else if (timer > 82000 && timer < 88500)
            {
                state = MinigameState.RECESSION_DEPRESSION;

                if (spawnTimer > 200)
                {
                    spawnTimer = 0;
                    // recession/depression
                    projectiles.Add(new LRASProjectile(blank, new Rectangle(
                        game.Width / 2, 0, 10, 10), 5000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(
                        0, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(
                        game.Width, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(
                        game.Width - 300, game.Height * 3 / 4 + 300, 10, 10), 10000));
                }
            }
            else if (timer >= 88500 && timer < 96000)
            {
                state = MinigameState.INFLATION;

                if (spawnTimer > 200)
                {
                    spawnTimer = 0;
                    projectiles.Add(new LRASProjectile(blank, new Rectangle(
                        game.Width / 2, 0, 10, 10), 5000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(
                        0, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(
                        game.Width, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(
                        game.Width + 200, game.Height * 3 / 4 - 100, 10, 10), 10000));
                }
            }
            else if (timer >= 96000 && timer < 103000)
            {
                state = MinigameState.STAGFLATION;
                if (spawnTimer > 200)
                {
                    spawnTimer = 0;
                    projectiles.Add(new LRASProjectile(blank, new Rectangle(
                        game.Width / 2, 0, 10, 10), 5000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(
                        0, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(
                        -200, game.Height * 3 / 4 - 200, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(
                        game.Width, game.Height * 3 / 4, 10, 10), 10000));
                }
            }
            else if (timer > 103000 && timer < 112000)
            {
                state = MinigameState.ECONOMIC_BOOM;
                if (spawnTimer > 200)
                {
                    spawnTimer = 0;
                    projectiles.Add(new LRASProjectile(blank, new Rectangle(
                        game.Width / 2, 0, 10, 10), 5000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(
                        0, game.Height * 3 / 4, 10, 10), 10000));
                    projectiles.Add(new ASProjectile(blank, new Rectangle(
                        200, game.Height * 3 / 4 + 200, 10, 10), 10000));
                    projectiles.Add(new ADProjectile(blank, new Rectangle(
                        game.Width, game.Height * 3 / 4, 10, 10), 10000));
                }
            }
            else if (timer > 114000 && timer < 124000)
            {
                if (spawnTimer > 70)
                {
                    spawnTimer = 0;
                    projectiles.Add(new LafferProjectile(blank, new Rectangle(0, game.Height, 10, 10), 10000));
                }
                state = MinigameState.LAFFERS_CURVE;
            }
            else if (timer > 124000 && timer < 124000 + t)
            {
                state = MinigameState.NONE;
                MediaPlayer.Volume = Math.Max(MediaPlayer.Volume - dv * dt, 0);
                //GameManager.GetInstance().ChangeScreen(ScreenState.MENU);
            }
            else if (timer > 124000 + t && timer < 124000 + 2 * t)
            {
                if (!isPlayFlag)
                {
                    isPlayFlag = true;
                    MediaPlayer.Play(background2);
                }
                MediaPlayer.Volume = Math.Min(MediaPlayer.Volume + dv * dt, 1);
            }
            else if (timer > 124000 + 4 / dv + 64 && boss.Health > 0)
            {
                state = MinigameState.BOSS;
                boss.Update(gameTime);
                songName = "Spear of Justice! - Undertale";
            }
            else
            {
                state = MinigameState.NONE;
            }

            if (boss.Health <= 0)
            {
                endTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                state = MinigameState.END;

                if (endTimer > 10000)
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
                    case MinigameState.BOSS:
                        text = "Boss Fight";
                        boss.Draw(spriteBatch);
                        break;
                }
                Vector2 position = new Vector2(game.Width - font.MeasureString(text).X - 32, 
                    game.Height - font.MeasureString(text).Y - 32);
                spriteBatch.DrawString(font, text, position + new Vector2(-2, 2), Color.Black * 0.6f);
                spriteBatch.DrawString(font, text, position, Color.White);
            }
            else if (state == MinigameState.BEGIN)
            {
                text = "Bonus Minigame!";
                Vector2 position = new Vector2(game.Width / 2 - font.MeasureString(text).X / 2, 
                    game.Height / 2 - font.MeasureString(text).Y / 2);

                spriteBatch.DrawString(font, text, position + new Vector2(-3, 3), Color.Black * 0.6f);
                spriteBatch.DrawString(font, text, position, Color.White);
            }
            else if (state == MinigameState.END)
            {
                text = "You win! Thanks for playing!";
                Vector2 position = new Vector2(game.Width / 2 - font.MeasureString(text).X / 2,
                    game.Height / 2 - font.MeasureString(text).Y / 2);

                spriteBatch.DrawString(font, text, position + new Vector2(-3, 3), Color.Black * 0.6f);
                spriteBatch.DrawString(font, text, position, Color.White);
            }

            int width = money.Width * 2;
            int height = money.Height * 2;
            for (int i = 0; i < player.Health; i++)
            {
                spriteBatch.Draw(money, new Rectangle(16 + i * (width + 16),
                    game.Height - height - 16, width, height), Color.White);
            }

            float snW = font.MeasureString(songName).X;
            spriteBatch.DrawString(font, "Made by Spencer Chang", new Vector2(8, 8), 
                Color.White * 0.3f, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, songName, new Vector2(game.Width - snW *
                0.8f - 10, 10), Color.Black* 0.3f, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, songName, new Vector2(game.Width - snW * 
                0.8f - 8, 8), Color.White * 0.9f, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }
}
