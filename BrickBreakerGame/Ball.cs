using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BrickBreakerGame
{
    public class Ball : GameObject
    {
        private float speed;
        private Vector2 direction;

        public Ball()
        {
            speed = 200f; // Juster efter spillets ønskede sværhedsgrad

            // Tilføj lidt tilfældighed til boldens startretning
            Random random = new Random();
            float randomDirectionX = (float)(random.NextDouble() * 2 - 1); // Generer mellem -1 og 1

            direction = new Vector2(randomDirectionX, -1); // Start retning
            direction.Normalize(); // Sikrer, at retningen er enhedslængde
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("black_ball");
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            position += direction * speed * GameWorld.DeltaTime(gameTime);

            // Håndter skærmkollisioner
            if (position.X - origin.X < 0 || position.X + origin.X > GameWorld.Width)
            {
                direction.X *= -1; // Skift vandret retning
            }
            if (position.Y - origin.Y < 0)
            {
                direction.Y *= -1; // Skift lodret retning
            }
            if (position.Y + origin.Y > GameWorld.Height)
            {
                // Bolden er faldet ud af bunden
                // Håndter livstab eller genstart bolden
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Paddle paddle)
            {
                //Differencen mellem boldens position og paddlens position
                float offset = position.X - paddle.Position.X;

                //Normaliser offset for at få en værdi mellem -1 og 1
                float normalizedOffset = offset / (paddle.Sprite.Width / 2);

                //Bestem en ny vinkel baseret på offset
                direction = new Vector2(normalizedOffset, -1);

                //Normaliser retningen så hastigheden forbliver konstant
                direction.Normalize();
            }
            else if (other is Brick)
            {
                direction.Y *= -1; // Vend den lodrette retning
                // Yderligere logik kan tilføjes for præcise kollisioner
            }
        }
    }
}
