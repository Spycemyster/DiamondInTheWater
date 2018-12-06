using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DiamondInTheWater.Entities.Minigame
{
    public class Boss : Sprite
    {
        private Texture2D texture, blank;
        private float timer, fireTimer, reloadTimer, fireT;
        private int health, maxhealth;
        private List<Projectile> projectiles;

        public int Health
        {
            get { return health; }
        }

        public Boss(List<Projectile> projectiles)
        {
            this.projectiles = projectiles;
            maxhealth = 210;
            health = 210;
            reloadTimer = 1500;
            fireTimer = 3000;
        }

        public override void Initialize(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Cocacola");
            blank = Content.Load<Texture2D>("blank");
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timer += dt;
            fireT += dt;

            reloadTimer -= dt;
            if (reloadTimer <= 0)
            {
                fireTimer -= dt;
                if (fireTimer <= 0)
                {
                    reloadTimer = 1500;
                    fireTimer = 3000;
                }

                if (fireT > 100)
                {
                    fireT = 0;
                    Rectangle dr = GetCollisionRectangle();
                    int size = 10;
                    projectiles.Add(new LRASProjectile(blank, new Rectangle(dr.X +
                        dr.Width / 2 - size / 2, dr.Y + dr.Height + 2, size, size * 2), 10000));
                }
            }
            
            float x = (float)(-Math.Cos(timer / 2000) + 1f) * (Game1.WIDTH / 2 - texture.Width / 4);
            float y = (float)(Math.Sin(timer / 2000) + 1) * Game1.HEIGHT / 10;

            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i] is FriendlyProjectile && projectiles[i].GetCollisionRectangle().Intersects(GetCollisionRectangle()))
                {
                    projectiles.RemoveAt(i--);
                    health--;
                }
            }

            Position = new Vector2(x, y);
        }

        public Rectangle GetCollisionRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, texture.Width / 2, texture.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float opacity = timer / 2500f;
            Rectangle cRect = GetCollisionRectangle();
            spriteBatch.Draw(blank, new Rectangle(cRect.X - 4, cRect.Y - 4, cRect.Width + 8, cRect.Height + 8), Color.White * opacity);
            spriteBatch.Draw(texture, cRect, Color.White * opacity);
            float factor = (float)health / maxhealth;
            int width = (int)(1000 * factor);
            spriteBatch.Draw(blank, new Rectangle(Game1.WIDTH / 2 - width / 2, 30, width, 32), Color.Green * opacity);
        }
    }
}
