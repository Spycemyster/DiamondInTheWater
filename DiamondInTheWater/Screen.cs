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
    public abstract class Screen
    {
        /// <summary>
        /// Initializes the <c>Screen</c> instance of a <c>ContentManager</c>.
        /// </summary>
        /// <param name="Content"></param>
        public abstract void Initialize(ContentManager Content);

        /// <summary>
        /// Called when exiting or switching screens.
        /// </summary>
        public abstract void Unload();

        /// <summary>
        /// Updates the logic and conditional checking for the <c>Screen</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draws the contents of the <c>Screen</c>.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
