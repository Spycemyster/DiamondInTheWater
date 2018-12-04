using DiamondInTheWater;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DiamondInTheWater
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public const int WIDTH = 1600;
        public const int HEIGHT = 900;

        /// <summary>
        /// The width in pixels of the game window.
        /// </summary>
        public int Width
        {
            get { return graphics.PreferredBackBufferWidth; }
        }

        /// <summary>
        /// The height in pixels of the game window.
        /// </summary>
        public int Height
        {
            get { return graphics.PreferredBackBufferHeight; }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //Content = new ResourceContentManager(Services, Resources.ResourceManager);

            double ratio = 16.0 / 9.0;

            int height = 900;
            int width = (int)(ratio * height);

            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;

            Window.Title = "A Diamond in the Water - Spencer Chang Period 3";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Tile.Init(Content);
            GameManager.GetInstance().Load(this);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Delete))
                Exit();
            
            if (IsActive)
            {
                MediaPlayer.Resume();
                InputManager.Instance.Update();
                GameManager.GetInstance().Update(gameTime);
            }
            else
                MediaPlayer.Pause();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            GameManager.GetInstance().Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
