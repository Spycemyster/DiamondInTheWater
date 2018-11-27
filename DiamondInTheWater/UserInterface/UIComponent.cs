using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondInTheWater.UserInterface
{
    public class UIComponent
    {
        public Color Background
        {
            get;
            set;
        }

        public Color Foreground
        {
            get;
            set;
        }

        public Point Position
        {
            get;
            set;
        }

        public Point Size
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public Vector2 BackgroundOrigin
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }

        public delegate void UIEvent(UIEventArg arg);

        /// <summary>
        /// When the user clicks on the component.
        /// </summary>
        public UIEvent OnClick;

        public UIEvent OnMouseEnter;

        public UIEvent OnMouseLeave;

        public bool IsHovering;

        /// <summary>
        /// Creates a new instance of the <c>UIComponent</c>.
        /// </summary>
        public UIComponent()
        {

        }

        public virtual void Initialize(ContentManager Content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            if (GetDrawRectangle(new Point(0, 0)).Contains(Mouse.GetState().Position))
            {
                if (InputManager.Instance.IsMouseClicked(MouseButton.LEFT))
                    OnClick?.Invoke(new UIEventArg(this));
                if (!IsHovering)
                {
                    IsHovering = true;
                    OnMouseEnter?.Invoke(new UIEventArg(this));
                }
            }
            else
            {
                if (IsHovering)
                {
                    IsHovering = false;
                    OnMouseLeave?.Invoke(new UIEventArg(this));
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual Rectangle GetDrawRectangle(Point offset)
        {
            return Rectangle.Empty;
        }
    }

    public class UIEventArg
    {
        public readonly UIComponent Component;

        /// <summary>
        /// Creates a new instance of the <c>UIEventArg</c>.
        /// </summary>
        /// <param name="component"></param>
        public UIEventArg(UIComponent component)
        {
            Component = component;
        }
    }
}
