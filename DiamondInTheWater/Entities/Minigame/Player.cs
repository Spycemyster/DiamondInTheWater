using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static DiamondInTheWater.Entities.Minigame.FriendlyProjectile;

namespace DiamondInTheWater.Entities.Minigame
{
    public class Player : Sprite
    {
        private const float SCALE = 0.4f;
        private const float MOVE_SPEED = 6;
        private Texture2D texture0, texture1, blank;
        public int Health
        {
            get { return (int)Math.Ceiling(health / 2); }
        }
        private float health;
        private float fireTimer;
        private List<Projectile> projectiles;
        private float hurtTimer;

        public Player(List<Projectile> projectiles)
        {
            health = 20;
            this.projectiles = projectiles;
            Position = new Vector2(Game1.WIDTH / 2, Game1.HEIGHT - 200);
        }

        public Rectangle GetCollisionRectangle(int safe)
        {
            Rectangle dr = GetDrawRectangle();

            return new Rectangle(dr.X + safe, dr.Y + safe, dr.Width - 2 * safe, dr.Height - 2 * safe);
        }

        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)(texture0.Width * SCALE), (int)(texture0.Height * SCALE));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color r = (hurtTimer >= 0) ? Color.Red : Color.White;
            spriteBatch.Draw(Texture, GetDrawRectangle(), r);
        }

        public override void Initialize(ContentManager Content)
        {
            texture0 = Content.Load<Texture2D>("PelkeySmile");
            texture1 = Content.Load<Texture2D>("PelkeyFrown");
            blank = Content.Load<Texture2D>("Pepsi");
            Texture = texture0;
        }

        private void Hurt()
        {
            hurtTimer = 225f;
            health--;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 velocity = Vector2.Zero;
            hurtTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            fireTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (InputManager.Instance.KeyDown(Keys.W) || InputManager.Instance.KeyDown(Keys.Up))
            {
                velocity.Y = -1;
            }
            else if (InputManager.Instance.KeyDown(Keys.S) || InputManager.Instance.KeyDown(Keys.Down))
            {
                velocity.Y = 1;
            }

            if (InputManager.Instance.KeyDown(Keys.A) || InputManager.Instance.KeyDown(Keys.Left))
            {
                velocity.X = -1;
            }
            else if (InputManager.Instance.KeyDown(Keys.D) || InputManager.Instance.KeyDown(Keys.Right))
            {
                velocity.X = 1;
            }


            Texture = (hurtTimer <= 0) ? texture0 : texture1;

            // shooting

            if (InputManager.Instance.KeyDown(Keys.Space))
            {
                if (fireTimer > 290)
                {
                    fireTimer = 0;
                    int size = 24;
                    int x = GetDrawRectangle().X + GetDrawRectangle().Width / 2;
                    int y = GetDrawRectangle().Y;
                    projectiles.Add(new FriendlyProjectile(blank, new Rectangle(x, y, size, size),
                        6000, FriendlyProjectileType.NLINEAR, projectiles));
                    projectiles.Add(new FriendlyProjectile(blank, new Rectangle(x, y, size, size),
                        6000, FriendlyProjectileType.PLINEAR, projectiles));
                    projectiles.Add(new FriendlyProjectile(blank, new Rectangle(x, y, size, size),
                        6000, FriendlyProjectileType.NSINUISOID, projectiles));
                    projectiles.Add(new FriendlyProjectile(blank, new Rectangle(x, y, size, size),
                        6000, FriendlyProjectileType.PSINUISOID, projectiles));
                    projectiles.Add(new FriendlyProjectile(blank, new Rectangle(x, y, size, size), 
                        6000, FriendlyProjectileType.VERTICAL, projectiles));
                }
            }

            if (velocity.Length() > 0)
                velocity.Normalize();
            Position += velocity * MOVE_SPEED;

            for (int i = 0; i < projectiles.Count; i++)
            {
                if (!(projectiles[i] is FriendlyProjectile)
                    && projectiles[i].GetCollisionRectangle().Intersects(GetCollisionRectangle(24)))
                {
                    Hurt();
                    projectiles.RemoveAt(i--);
                }
            }
        }
    }
}
