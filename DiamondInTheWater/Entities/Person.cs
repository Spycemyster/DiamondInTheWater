using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DiamondInTheWater.Entities
{
    public class Person : Sprite
    {
        private Texture2D left, right;
        private Rectangle bounds;
        private float timer;
        private double frameTimer;
        private Random rand;
        private PersonState state;
        private int WALKTIME, WAITTIME;
        public const float SPEED = 1.5f;
        private Vector2 direction;

        public Person(Rectangle bounds, Random rand)
        {
            direction = Vector2.Zero;
            WALKTIME = rand.Next(300, 3000);
            WAITTIME = rand.Next(0, 4000);
            this.rand = rand;
            state = PersonState.WAITING;
            this.bounds = bounds;
        }

        public override void Initialize(ContentManager Content)
        {
            right = Content.Load<Texture2D>("PersonRight");
            left = Content.Load<Texture2D>("PersonLeft");
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            frameTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            switch (state)
            {
                case PersonState.WAITING:
                    if (timer >= WAITTIME)
                    {
                        timer = 0;
                        state = PersonState.DIRECTION;
                    }
                    break;
                case PersonState.WALKING:
                    if (timer < WALKTIME)
                    {
                        Position += SPEED * direction;

                        if (!bounds.Contains(Position.ToPoint()))
                        {
                            if (Position.X > bounds.X + bounds.Width)
                                Position = new Vector2(bounds.X + bounds.Width, Position.Y);
                            else if (Position.X < bounds.X)
                                Position = new Vector2(bounds.X, Position.Y);
                            if (Position.Y > bounds.Y + bounds.Height)
                                Position = new Vector2(Position.X, bounds.Y + bounds.Height);
                            else if (Position.Y < bounds.Y)
                                Position = new Vector2(Position.X, bounds.Y);
                            timer = 0;
                            state = PersonState.WAITING;
                        }
                    }
                    else
                    {
                        timer = 0;
                        state = PersonState.WAITING;
                    }
                    break;
                case PersonState.DIRECTION:
                    state = PersonState.WALKING;
                    direction = new Vector2((float)rand.NextDouble() - 0.5f, (float)rand.NextDouble() - 0.5f);
                    direction.Normalize();
                    WALKTIME = rand.Next(300, 3000);
                    WAITTIME = rand.Next(0, 4000);

                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = (direction.X > 0) ? right : left;
            int frame = (state.Equals(PersonState.WALKING) && Math.Sin(frameTimer / 100) > 0) ? 1 : 0;

            spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, 15, 15),
                new Rectangle(3 * frame, 0, 3, 3), Color.White);
        }

        private enum PersonState
        {
            WALKING,
            DIRECTION,
            WAITING,
        }

    }
}
