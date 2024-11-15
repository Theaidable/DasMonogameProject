using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BrickBreakerGame
{
    /// <summary>
    /// Abstrakt baseklasse, som repræsentere et spilobjekt i brick Breaker spillet.
    /// Alle objekter i spillet arver fra denne klasse
    /// </summary>
    public abstract class GameObject
    {
        //Fields
        protected Vector2 position;
        protected Texture2D sprite;
        protected Vector2 origin;
        private static Texture2D collisionTexture;

        /// <summary>
        /// Offentlig property til at få objektes position.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Offentlig property til at få objektes sprite.
        /// </summary>
        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        /// <summary>
        /// Offentlig property der returnere en Rectangle for objektets kollisionsboks
        /// </summary>
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

        /// <summary>
        /// Indlæser debug-tekstur til at vise kollsionsboksen for spilobjekter.
        /// </summary>
        /// <param name="graphicsDevice"></param> GraphicsDevice bruges til at skabe debug-teksturen.
        public static void LoadDebugContent(GraphicsDevice graphicsDevice)
        {
            if (collisionTexture == null)
            {
                collisionTexture = new Texture2D(graphicsDevice, 1, 1);
                collisionTexture.SetData(new[] { Color.Red });
            }
        }

        /// <summary>
        /// Tegner spilobjektet på skærmen
        /// </summary>
        /// <param name="spriteBatch"></param> SpriteBatch bruges til at tegne spilobjektet
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

        /// <summary>
        /// Tjekker om GameObject kolliderer med et andet GameObject
        /// </summary>
        /// <param name="other"></param> Det andet GameObject der tjekkes for kollision med.
        /// <returns> Returnerer true, hvis der er en kollision, ellers returnerer den false</returns>
        public bool CheckCollision(GameObject other)
        {
            return this.CollisionBox.Intersects(other.CollisionBox);
        }
    }
}
