using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BrickBreakerGame
{
    public class Paddle : GameObject
    {
        private float speed;

        public Paddle()
        {
            speed = 400f;
            // Startposition for paddlen i bunden af skærmen
            position = new Vector2(GameWorld.Width / 2, GameWorld.Height - 50);
        }

        public override void LoadContent(ContentManager content)
        {
            // Indlæs paddle-sprite
            sprite = content.Load<Texture2D>("black_paddle");
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        public override void OnCollision(GameObject other)
        {
            // Implementeres senere, men dette er collider event kode
        }

        private void Move(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            // Flyt paddle til venstre
            if (keyState.IsKeyDown(Keys.A))
            {
                position.X -= speed * GameWorld.DeltaTime(gameTime);
            }
            // Flyt paddle til højre
            if (keyState.IsKeyDown(Keys.D))
            {
                position.X += speed * GameWorld.DeltaTime(gameTime);
            }

            // Begræns paddle til skærmens bredde
            if (position.X - origin.X < 0)
            {
                position.X = origin.X;
            }
            if (position.X + origin.X > GameWorld.Width)
            {
                position.X = GameWorld.Width - origin.X;
            }
        }
    }
}
