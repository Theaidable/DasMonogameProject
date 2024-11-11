using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BrickBreakerGame
{
    public class Brick : GameObject
    {
        private int hitPoints;
        private bool isIndestructible;
        public bool IsMarkedForRemoval { get; private set; }

        public Brick(int initialHitPoints, bool indestructible = false)
        {
            hitPoints = initialHitPoints;
            isIndestructible = indestructible;
            IsMarkedForRemoval = false;
        }

        public override void LoadContent(ContentManager content)
        {
            if (isIndestructible)
            {
                sprite = content.Load<Texture2D>("BricksSquareSprites/BrickSquare000");
            }
            else if (hitPoints == 1)
            {
                sprite = content.Load<Texture2D>("BricksSquareSprites/BrickSquare002");
            }
            else if (hitPoints == 2)
            {
                sprite = content.Load<Texture2D>("BricksSquareSprites/BrickSquare004");
            }
            else if (hitPoints == 3)
            {
                sprite = content.Load<Texture2D>("BricksSquareSprites/BrickSquare008");
            }

            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            // Ingen opdatering løbende spillet
        }

        public override void OnCollision(GameObject other)
        {
            if (isIndestructible) return;

            if (other is Ball)
            {
                hitPoints--;

                if (hitPoints <= 0)
                {
                    IsMarkedForRemoval = true;
                    GameWorld.Instance.RemoveGameObject(this);
                }
            }
        }
    }
}