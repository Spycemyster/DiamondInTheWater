using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondInTheWater.Entities.Minigame
{
    public class FriendlyProjectile : Projectile
    {
        public enum FriendlyProjectileType
        {
            NSINUISOID,
            PSINUISOID,
            VERTICAL,
            NLINEAR,
            PLINEAR,
        }
        private FriendlyProjectileType type;
        private List<Projectile> projectiles;
        private float timer;
        public FriendlyProjectile(Texture2D texture, Rectangle initialRectangle, int ttl,
            FriendlyProjectileType type, List<Projectile> projectiles) : base(texture, 
                initialRectangle, ttl)
        {
            this.type = type;
            this.projectiles = projectiles;
        }
        

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            for (int i = 0; i < projectiles.Count; i++)
            {
                Projectile p = projectiles[i];

                if (p is FriendlyProjectile)
                {
                    continue;
                }

                if (p.GetCollisionRectangle().Intersects(GetCollisionRectangle()))
                {
                    projectiles.Remove(p);
                    projectiles.Remove(this);
                }
            }
            base.Update(gameTime);

            switch (type)
            {
                case FriendlyProjectileType.PSINUISOID:
                    velocity = new Vector2(-(float)Math.Cos(timer / 200) * 2.5f, -2);
                    break;
                case FriendlyProjectileType.NSINUISOID:
                    velocity = new Vector2((float)Math.Cos(timer / 200) * 2.5f, -2);
                    break;
                case FriendlyProjectileType.VERTICAL:
                    velocity = new Vector2(0, -2);
                    break;
                case FriendlyProjectileType.NLINEAR:
                    velocity = new Vector2(2, -2);
                    break;
                case FriendlyProjectileType.PLINEAR:
                    velocity = new Vector2(-2, -2);
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle cRect = GetCollisionRectangle();
            Rectangle borRect = new Rectangle(cRect.X - 3, cRect.Y - 3, cRect.Width + 6, cRect.Height + 6);
            spriteBatch.Draw(Texture, borRect, Color.Black);
            spriteBatch.Draw(Texture, cRect, Color.Green);
        }

    }
}
