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
            speed = 300f; // Juster efter spillets ønskede sværhedsgrad

            // Tilføj lidt tilfældighed til boldens startretning
            Random random = new Random();
            float randomDirectionX = (float)(random.NextDouble() * 2 - 1); // Generer mellem -1 og 1

            direction = new Vector2(randomDirectionX, -1); // Start retning
            direction.Normalize(); // Sikrer, at retningen er enhedslængde
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("black_ball"); // Erstat "ball_texture" med din faktiske sprite
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
                // Afspil paddle hit-lyd
                GameWorld.Instance.soundManager.PlaySound("paddle_hit");

                // Differencen mellem boldens position og paddlens position
                float offset = position.X - paddle.Position.X;

                // Normaliser offset for at få en værdi mellem -1 og 1
                float normalizedOffset = offset / (paddle.Sprite.Width / 2);

                // Bestem en ny vinkel baseret på offset
                direction = new Vector2(normalizedOffset, -1);

                // Normaliser retningen så hastigheden forbliver konstant
                direction.Normalize();
            }
            else if (other is Brick brick)
            {
                // Afspil brick hit-lyd
                GameWorld.Instance.soundManager.PlaySound("brick_hit");

                // Bestem kollisionens retning baseret på boldens og murstenens position
                float ballLeft = position.X - origin.X;
                float ballRight = position.X + origin.X;
                float ballTop = position.Y - origin.Y;
                float ballBottom = position.Y + origin.Y;

                float brickLeft = brick.Position.X - brick.Sprite.Width / 2;
                float brickRight = brick.Position.X + brick.Sprite.Width / 2;
                float brickTop = brick.Position.Y - brick.Sprite.Height / 2;
                float brickBottom = brick.Position.Y + brick.Sprite.Height / 2;

                // Tjek hvilken side af murstenen, bolden rammer
                bool hitFromTop = ballBottom > brickTop && ballTop < brickTop && direction.Y > 0;
                bool hitFromBottom = ballTop < brickBottom && ballBottom > brickBottom && direction.Y < 0;
                bool hitFromLeft = ballRight > brickLeft && ballLeft < brickLeft && direction.X > 0;
                bool hitFromRight = ballLeft < brickRight && ballRight > brickRight && direction.X < 0;

                if (hitFromTop || hitFromBottom)
                {
                    direction.Y *= -1; // Skift lodret retning
                }
                else if (hitFromLeft || hitFromRight)
                {
                    direction.X *= -1; // Skift vandret retning
                }

                // Normaliser retningen så hastigheden forbliver konstant
                direction.Normalize();

                // Undgå at mursten mister flere liv på en gang ved at sikre, at kun én kollision opdateres
                if (brick.IsMarkedForRemoval == true)
                {
                    brick.OnCollision(this);
                }
            }
        }

        public void IncreaseBallSpeed()
        {
            speed += 200f;
        }

        public void DecreaseBallSpeed()
        {
            speed -= 100f;
        }
    }
}
