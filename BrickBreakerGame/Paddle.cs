using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BrickBreakerGame
{
    /// <summary>
    /// Paddle klassen styrer spillerens paddle
    /// Paddlen kan bevæge sig til højre eller venstre, og den ændrer størrelse basteret på power.
    /// </summary>
    public class Paddle : GameObject
    {
        // Fields
        private float speed;

        /// <summary>
        /// Constructor for Paddle klassen.
        /// Sætter standardhastigheden og position
        /// </summary>
        public Paddle()
        {
            speed = 400f;
            position = new Vector2(GameWorld.Width / 2, GameWorld.Height - 50); // Startposition for paddlen i bunden af skærmen
        }

        /// <summary>
        /// Indlæser indholdet for paddle, herunder paddle-sprite.
        /// </summary>
        /// <param name="content"></param> ContentManager til at indlæse paddle-sprite.
        public override void LoadContent(ContentManager content)
        {
            // Indlæs paddle-sprite
            sprite = content.Load<Texture2D>("PaddleSprites/Paddle002");
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }

        /// <summary>
        /// Opdatere paddlens position, sådan den bevæger sig
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        /// <summary>
        /// Håndtere kollision med andre objekter.
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(GameObject other)
        {
            //Collision med Paddle håndteres i andre klasser.
        }

        /// <summary>
        /// Flytter paddlen baseret på brugerens input.
        /// </summary>
        /// <param name="gameTime"></param>
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

        /// <summary>
        /// Øg størrelsen af paddlen når PowerUp aktiveres.
        /// </summary>
        public void IncreasePaddleSize()
        {
            // Forøg paddlens størrelse med en bestemt faktor
            if (sprite != null)
            {
                Vector2 newSize = new Vector2(sprite.Width * 2f, sprite.Height); // Fordobler bredden
                sprite = ResizeTexture(sprite, (int)newSize.X, (int)newSize.Y);
                origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            }
        }

        /// <summary>
        /// Reducerer størrelsen af paddlen når PowerDown aktiveres.
        /// </summary>
        public void DecreasePaddleSize()
        {
            // Reducer paddlens størrelse med en bestemt faktor
            if (sprite != null)
            {
                Vector2 newSize = new Vector2(sprite.Width * 0.5f, sprite.Height); // Halvere bredden
                sprite = ResizeTexture(sprite, (int)newSize.X, (int)newSize.Y);
                origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            }
        }

        /// <summary>
        /// Ændrer størrelsen på paddlens sprite.
        /// </summary>
        /// <param name="texture"></param> Den originale sprite, som skal ændres
        /// <param name="width"></param> Ny bredde for spriten.
        /// <param name="height"></param> Ny høhjde for spriten.
        /// <returns>Returnerer en Texture2D med ændrede størrelse</returns>
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
