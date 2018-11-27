using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiamondInTheWater;
using DiamondInTheWater.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SSBGame.UserInterface
{
    public class UISlider : UIComponent
    {
        public float Value
        {
            get { return Math.Max((float)(SliderX - Position.X) / Size.X, 0f); }
        }

        public int SliderY
        {
            get { return Position.Y - Size.Y / 2; }
        }
        public int SliderX
        {
            get;
            set;
        }
        private bool isDragging;
        public SpriteFont Font
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new instance of the <c>UISlider</c>.
        /// </summary>
        public UISlider()
        {
            isDragging = false;
            Foreground = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsHovering && InputManager.Instance.IsMouseDown(MouseButton.LEFT))
            {
                isDragging = true;
            }

            if (isDragging)
            {
                SliderX = Mouse.GetState().X;

                if (InputManager.Instance.IsMouseUp(MouseButton.LEFT))
                    isDragging = false;
            }

            // keeps the slider drag within the component
            if (SliderX > Position.X + Size.X)
                SliderX = Position.X + Size.X;
            else if (SliderX < Position.X)
                SliderX = Position.X;
        }

        public override Rectangle GetDrawRectangle(Point offset)
        {
            return new Rectangle(Position.X + offset.X, Position.Y + offset.Y, Size.X, Size.Y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            // left side
            //spriteBatch.Draw(Texture, new Rectangle(Position.X, Position.Y, SliderX - Position.X, Size.Y), Color.LightCyan);
            //// right side
            //spriteBatch.Draw(Texture, new Rectangle(Position.X + SliderX, Position.Y, Position.X + Size.X - SliderX, Size.Y), Color.White);
            spriteBatch.Draw(Texture, new Rectangle(Position.X, Position.Y, Size.X, Size.Y), Color.White);
            float v = Value;
            // slider
            int dim = Size.Y * 2;
            spriteBatch.Draw(Texture, new Rectangle(SliderX - dim / 2, SliderY, dim, dim), Color.Gray);
            Vector2 pos = new Vector2(Position.X - Font.MeasureString(Text).X - dim,
                Position.Y - Size.Y / 2 + Font.MeasureString(Text).Y / 2);
            spriteBatch.DrawString(Font, Text, pos, Foreground);
        }
    }
}
