using DiamondInTheWater.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondInTheWater
{
    public class GameManager
    {
        #region Singleton Code
        /// <summary>
        /// The current instance of the <c>GameManager</c>.
        /// </summary>
        /// <returns></returns>
        public static GameManager GetInstance()
        {
            if (inst == null)
                inst = new GameManager();
            return inst;
        }
        private static GameManager inst;

        private GameManager()
        {

        }
        #endregion

        private Game1 game;
        private Screen screen;

        public Screen Screen
        {
            get { return screen; }
        }

        /// <summary>
        /// Loads and initializes the <c>GameManager</c>.
        /// </summary>
        /// <param name="game"></param>
        public void Load(Game1 game)
        {
            this.game = game;
            ChangeScreen(ScreenState.MENU);
        }

        public void ChangeScreen(ScreenState state)
        {
            screen?.Unload();
            switch (state)
            {
                case ScreenState.GAME:
                    screen = new GameScreen(game);
                    break;
                case ScreenState.MENU:
                    screen = new MenuScreen(game);
                    break;
            }

            screen.Initialize(game.Content);
        }

        /// <summary>
        /// Updates the logic and conditional checking for the 
        /// <c>GameManager</c> and the current instance of <c>Screen</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            screen?.Update(gameTime);
        }

        /// <summary>
        /// Draws the <c>GameManager</c> and the current <c>Screen</c>.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            screen?.Draw(spriteBatch);
        }
    }

    public enum ScreenState
    {
        GAME,
        MENU,
    }
}
