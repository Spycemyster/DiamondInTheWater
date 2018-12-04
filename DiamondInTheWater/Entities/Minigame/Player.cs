using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DiamondInTheWater.Entities.Minigame
{
    public class Player : Sprite
    {
        private const float SCALE = 0.4f;
        private const float MOVE_SPEED = 6;
        private Vector2 position;
        private Texture2D texture;
        private int health;
        private List<Projectile> projectiles;

        public Player(List<Projectile> projectiles)
        {
            health = 20;
            this.projectiles = projectiles;
            position = new Vector2(Game1.WIDTH / 2, Game1.HEIGHT - 200);
        }

        public Rectangle GetCollisionRectangle(int safe)
        {
            Rectangle dr = GetDrawRectangle();

            return new Rectangle(dr.X + safe, dr.Y + safe, dr.Width - 2 * safe, dr.Height - 2 * safe);
        }

        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * SCALE), (int)(texture.Height * SCALE));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, GetDrawRectangle(), Color.White);
        }

        public override void Initialize(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("PelkeySmile");
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 velocity = Vector2.Zero;

            if (InputManager.Instance.KeyDown(Keys.W))
            {
                velocity.Y = -1;
            }
            else if (InputManager.Instance.KeyDown(Keys.S))
            {
                velocity.Y = 1;
            }

            if (InputManager.Instance.KeyDown(Keys.A))
            {
                velocity.X = -1;
            }
            else if (InputManager.Instance.KeyDown(Keys.D))
            {
                velocity.X = 1;
            }
            if (velocity.Length() > 0)
                velocity.Normalize();
            position += velocity * MOVE_SPEED;

            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].GetCollisionRectangle().Intersects(GetCollisionRectangle(24)))
                {
                    health--;
                    projectiles.RemoveAt(i--);
                }
            }
        }
    }
}
