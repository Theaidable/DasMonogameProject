using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BrickBreakerGame
{
    public class Brick : GameObject
    {
        private int hitPoints;
        private bool isIndestructible;
        private int points;

        public bool IsMarkedForRemoval { get; private set; }
        public bool IsIndestructible { get => isIndestructible; set => isIndestructible = value; }


        public Power PowerContained { get; set; }

        public Brick(int initialHitPoints, bool indestructible = false, Power power = null)
        {
            hitPoints = initialHitPoints;
            isIndestructible = indestructible;
            IsMarkedForRemoval = false;
            PowerContained = power;

            // Tildel point baseret på murstenens hitPoints
            switch (hitPoints)
            {
                case 1:
                    points = 100; // Grøn mursten
                    break;
                case 2:
                    points = 200; // Gul mursten
                    break;
                case 3:
                    points = 300; // Rød mursten
                    break;
                default:
                    points = 0;
                    break;
            }
        }

        public override void LoadContent(ContentManager content)
        {
            if (IsIndestructible)
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
            if (IsIndestructible) return;

            if (other is Ball)
            {
                hitPoints--;

                if (hitPoints > 0)
                {
                    UpdateSprite(GameWorld.Instance.Content);
                }

                if (hitPoints <= 0)
                {
                    IsMarkedForRemoval = true;
                    GameWorld.Instance.RemoveGameObject(this);
                    // Tilføj specifikke point baseret på murstenens farve
                    GameWorld.Instance.AddPointsToScore(points);
                    GameWorld.Instance.soundManager.PlaySound("brick_break"); // Afspil lyd, når mursten ødelægges

                    // Hvis murstenen har en Power, slip den
                    if (PowerContained != null)
                    {
                        PowerContained.Position = new Vector2(position.X, position.Y);
                        GameWorld.Instance.AddGameObject(PowerContained);
                    }
                }
            }
        }

        private void UpdateSprite(ContentManager content)
        {
            if (hitPoints == 1)
            {
                sprite = content.Load<Texture2D>("BricksSquareSprites/BrickSquare003");
            }
            else if (hitPoints == 2)
            {
                sprite = content.Load<Texture2D>("BricksSquareSprites/BrickSquare005");
            }
        }
    }
}