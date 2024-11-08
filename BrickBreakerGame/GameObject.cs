using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BrickBreakerGame
{
    public abstract class GameObject
    {
        protected Vector2 position;
        protected Texture2D sprite;
        protected Vector2 origin;
        private static Texture2D collisionTexture;

        // Offentlig egenskab til at få adgang til position
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        // Offentlig egenskab til at få adgang til sprite
        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        // Egenskab der giver en Rectangle for kollisionsboksen
        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), sprite.Width, sprite.Height);
            }
        }

        // Abstrakte metoder, som alle nedarvede klasser skal implementere
        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void OnCollision(GameObject other);

        // Standard LoadContent-metode til at indlæse collisionTexture
        public static void LoadDebugContent(GraphicsDevice graphicsDevice)
        {
            if (collisionTexture == null)
            {
                collisionTexture = new Texture2D(graphicsDevice, 1, 1);
                collisionTexture.SetData(new[] { Color.Red });
            }
        }

        // Standard Draw-metode til at tegne objektet
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);

            // Tegn collisionbox for debugging
            if (collisionTexture != null)
            {
                Rectangle collisionBox = CollisionBox;
                spriteBatch.Draw(collisionTexture, new Rectangle(collisionBox.Left, collisionBox.Top, collisionBox.Width, 1), Color.Red); // Top
                spriteBatch.Draw(collisionTexture, new Rectangle(collisionBox.Left, collisionBox.Bottom, collisionBox.Width, 1), Color.Red); // Bottom
                spriteBatch.Draw(collisionTexture, new Rectangle(collisionBox.Left, collisionBox.Top, 1, collisionBox.Height), Color.Red); // Left
                spriteBatch.Draw(collisionTexture, new Rectangle(collisionBox.Right, collisionBox.Top, 1, collisionBox.Height), Color.Red); // Right
            }
        }

        // Metode til at tjekke kollision med et andet GameObject
        public bool CheckCollision(GameObject other)
        {
            return this.CollisionBox.Intersects(other.CollisionBox);
        }
    }
}
