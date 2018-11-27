using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondInTheWater.UserInterface
{
    public class UIButton : UIComponent
    {

        public float Opacity
        {
            get;
            set;
        }

        public SpriteFont Font
        {
            get;
            set;
        }
        private float timer;

        /// <summary>
        /// Creates a new instance of the <c>UIButton</c>.
        /// </summary>
        public UIButton()
        {
            timer = 0f;
            Foreground = Color.Black;
            Background = Color.White;
            Opacity = 1;
            Text = "";
        }

        public override void Initialize(ContentManager Content)
        {
            base.Initialize(Content);

            Texture = Content.Load<Texture2D>("Blank");
            Font = Content.Load<SpriteFont>("GameFont");
        }

        /// <summary>
        /// Updates the logic of the <c>UIButton</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (IsHovering)
            {
                Opacity = (float)Math.Cos(timer / 200f) * 0.4f + 0.6f;
            }
            else
            {
                timer = 0f;
                Opacity = 1f;
            }
        }

        /// <summary>
        /// The draw rectangle for the <c>UIButton</c>.
        /// </summary>
        /// <returns></returns>
        public override Rectangle GetDrawRectangle(Point offset)
        {
            return new Rectangle(new Vector2(Position.X + offset.X, Position.Y - offset.Y).ToPoint(), Size);
        }

        /// <summary>
        /// Draws the contents of the <c>UIButton</c> to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle drawRect = GetDrawRectangle(new Point(0, 0));
            //Vector2 position = new Vector2(drawRect.X, drawRect.Y);
            //spriteBatch.Draw(Texture, drawRect, null, Background * Opacity, 0f,
            //    new Vector2(Texture.Width / 2f, Texture.Height / 2f), SpriteEffects.None, 0f);
            //spriteBatch.DrawString(font, Text, position, Foreground * Opacity, 0f, new Vector2(
            //    textSize.X / 2, textSize.Y / 2), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(Texture, drawRect, Background * Opacity);

            if (!Text.Equals(""))
            {
                Vector2 textSize = Font.MeasureString(Text);
                Vector2 position = new Vector2(drawRect.X + drawRect.Width / 2 - textSize.X / 2,
                    drawRect.Y + drawRect.Height / 2 - textSize.Y / 2);
                spriteBatch.DrawString(Font, Text, position, Foreground * Opacity);
            }
        }
    }
}
