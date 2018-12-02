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

namespace DiamondInTheWater.Screens
{
    public class MenuScreen : Screen
    {
        private UIButton start, load;
        private Texture2D texture, blank;
        private Game1 game;
        public MenuScreen(Game1 game)
        {
            this.game = game;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(0, 0, game.Width, game.Height), Color.White);
            start.Draw(spriteBatch);
            load.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Initialize(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Menu");
            blank = Content.Load<Texture2D>("blank");
            start = new UIButton();
            start.Font = Content.Load<SpriteFont>("largeFont");
            start.Texture = blank;
            start.Text = "Start";
            start.Size = new Point(240, 70);
            start.Position = new Point(game.Width / 2 - 120, game.Height / 2 + 72);
            start.Background = Color.DimGray * 0.95f;
            start.OnClick += onClick;
            load = new UIButton();
            load.Font = Content.Load<SpriteFont>("largeFont");
            load.Size = new Point(240, 70);
            load.Position = new Point(game.Width / 2 - 120, game.Height / 2 + 172);
            load.Background = Color.DimGray * 0.95f;
            load.OnClick += onClick;
            load.Text = "Load";
            load.Texture = blank;
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
        }

        public override void Unload()
        {
        }

        public override void Update(GameTime gameTime)
        {
            start.Update(gameTime);
            load.Update(gameTime);
        }
    }
}
