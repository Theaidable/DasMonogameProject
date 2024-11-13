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

        public void IncreaseSize()
        {
            // Forøg paddlens størrelse med en bestemt faktor (f.eks. 1.5 gange)
            if (sprite != null)
            {
                Vector2 newSize = new Vector2(sprite.Width * 1.5f, sprite.Height); // Forøger bredden med 50%
                sprite = ResizeTexture(sprite, (int)newSize.X, (int)newSize.Y);
                origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            }
        }

        public void DecreaseSize()
        {
            // Reducer paddlens størrelse med en bestemt faktor (f.eks. 0.75 gange)
            if (sprite != null)
            {
                Vector2 newSize = new Vector2(sprite.Width * 0.75f, sprite.Height); // Reducerer bredden med 25%
                sprite = ResizeTexture(sprite, (int)newSize.X, (int)newSize.Y);
                origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            }
        }

        // Metode til at ændre størrelsen på paddlens sprite
        private Texture2D ResizeTexture(Texture2D texture, int width, int height)
        {
            RenderTarget2D renderTarget = new RenderTarget2D(GameWorld.Instance.GraphicsDevice, width, height);
            GameWorld.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
            GameWorld.Instance.GraphicsDevice.Clear(Color.Transparent);

            SpriteBatch spriteBatch = new SpriteBatch(GameWorld.Instance.GraphicsDevice);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
            spriteBatch.End();
            GameWorld.Instance.GraphicsDevice.SetRenderTarget(null);

            return (Texture2D)renderTarget;
        }
    }
}
