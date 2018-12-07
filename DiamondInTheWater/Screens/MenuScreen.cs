using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiamondInTheWater.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DiamondInTheWater.Screens
{
    public class MenuScreen : Screen
    {
        private UIButton start, load, minigame;
        private Texture2D texture, blank;
        private Game1 game;

        /// <summary>
        /// Creates a new instance of the <c>MenuScreen</c>.
        /// </summary>
        /// <param name="game"></param>
        public MenuScreen(Game1 game)
        {
            this.game = game;
        }

        /// <summary>
        /// Draws the <c>MenuScreen</c> to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(0, 0, game.Width, game.Height), Color.White);
            start.Draw(spriteBatch);
            load.Draw(spriteBatch);
            minigame.Draw(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Initializes the <c>MenuScreen</c>.
        /// </summary>
        /// <param name="Content"></param>
        public override void Initialize(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Menu");
            blank = Content.Load<Texture2D>("blank");
            start = new UIButton
            {
                Font = Content.Load<SpriteFont>("largeFont"),
                Texture = blank,
                Text = "Start",
                Size = new Point(240, 70),
                Position = new Point(game.Width / 2 - 120, game.Height / 2 + 72),
                Background = Color.DimGray * 0.95f
            };
            start.OnClick += onClick;
            load = new UIButton
            {
                Font = Content.Load<SpriteFont>("largeFont"),
                Size = new Point(240, 70),
                Position = new Point(game.Width / 2 - 120, game.Height / 2 + 172),
                Background = Color.DimGray * 0.95f
            };
            load.OnClick += onClick;
            load.Text = "Load";
            load.Texture = blank;
            minigame = new UIButton
            {
                Font = Content.Load<SpriteFont>("largeFont"),
                Size = new Point(240, 70),
                Position = new Point(game.Width / 2 - 120, game.Height / 2 + 272),
                Background = Color.DimGray * 0.95f,
                Foreground = Color.Yellow * 0.95f,
            };
            minigame.OnClick += onClick;
            minigame.Text = "Bonus";
            minigame.Texture = blank;
        }

        private void onClick(UIEventArg arg)
        {
            if (arg.Component == load)
            {
                GameManager.GetInstance().ChangeScreen(ScreenState.GAME);

                if (File.Exists("Save/Load.sav"))
                {
                    GameScreen g = (GameScreen)(GameManager.GetInstance().Screen);
                    g.LoadGame();
                }
            }
            else if (arg.Component == start)
            {
                GameManager.GetInstance().ChangeScreen(ScreenState.GAME);
            }
            else if (arg.Component == minigame)
            {
                GameManager.GetInstance().ChangeScreen(ScreenState.MINIGAME);
            }
        }

        public override void Unload()
        {
        }

        public override void Update(GameTime gameTime)
        {
            MediaPlayer.Stop();
            start.Update(gameTime);
            load.Update(gameTime);
            minigame.Update(gameTime);
        }
    }
}
