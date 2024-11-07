using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BrickBreakerGame
{
    public class Ball : GameObject
    {
        private float speed;
        private Vector2 direction;

        public Ball()
        {
            speed = 400f; // Juster efter spillets ønskede sværhedsgrad
            direction = new Vector2(1, -1); // Start retning
            direction.Normalize(); // Sikrer, at retningen er enhedslængde
            position = new Vector2(GameWorld.Width / 2, GameWorld.Height / 2); // Startposition i midten
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("ball_texture"); // Erstat "ball_texture" med din faktiske sprite
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
            // Eksempel på kollision med paddle eller mursten
            if (other is Paddle || other is Brick)
            {
                direction.Y *= -1; // Vend den lodrette retning
                // Yderligere logik kan tilføjes for præcise kollisioner
            }
        }
    }
}
