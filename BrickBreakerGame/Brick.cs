using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BrickBreakerGame
{
    /// <summary>
    /// Repræsentere en mursten i Brick breaker Spillet.
    /// Murstene kan ødelægges af bolden og de kan indeholde PowerUps eller PowerDowns.
    /// </summary>
    public class Brick : GameObject
    {
        //Fields
        private int hitPoints;
        private bool isIndestructible;
        private int points;

        /// <summary>
        /// Angiver om murstenen er market til fjernelse.
        /// </summary>
        public bool IsMarkedForRemoval { get; private set; }

        /// <summary>
        /// Angiver om murstenen kan ødelægges.
        /// </summary>
        public bool IsIndestructible { get => isIndestructible; private set => isIndestructible = value; }

        /// <summary>
        /// Angiver hvilken power murstenen indeholder, som falder ned når murstenen ødelægges.
        /// </summary>
        public Power PowerContained { get; set; }

        /// <summary>
        /// Initialiserer en ny instans af "Brick" med de specificerede egenskaber.
        /// </summary>
        /// <param name="initialHitPoints"></param> Antallet af hits murstene skal tage før den ødelægges
        /// <param name="indestructible"></param> Angiver om murstenen kan ødelægges
        /// <param name="power"></param> Den power som murstenen indeholder
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

        /// <summary>
        /// Indlæser murstenes sprite baseret på hitpoints.
        /// </summary>
        /// <param name="content"></param> ContentManager bruges til at indlæse murstenes sprite
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

        /// <summary>
        /// Opdaterer murstenes status. I denne klasse er der ingen handling under opdatering.
        /// </summary>
        /// <param name="gameTime"></param> Spillets tidsinformation.
        public override void Update(GameTime gameTime)
        {
            // Ingen opdatering løbende spillet
        }

        /// <summary>
        /// Håndtere kollisioner med andre spilobjekter som bolden.
        /// </summary>
        /// <param name="other"></param> Det andet "GameObject" som murstenen kolliderer med
        public override void OnCollision(GameObject other)
        {
            if (IsIndestructible) return;

            if (other is Ball)
            {
                hitPoints--;

                // Opdater sprite, hvis murstenen ikke ødelægges ved kollisionen.
                if (hitPoints > 0)
                {
                    UpdateSprite(GameWorld.Instance.Content);
                }

                // Hvis murstenen har mindre eller lig 0 hitpoints, så fjernes murstenen.
                if (hitPoints <= 0)
                {
                    IsMarkedForRemoval = true;
                    GameWorld.Instance.RemoveGameObject(this);
                    GameWorld.Instance.AddPointsToScore(points); // Tilføj specifikke point baseret på murstenens farve
                    GameWorld.Instance.soundManager.PlaySound("brick_break"); // Afspil lyd, når mursten ødelægges

                    // Hvis murstenen har en Power, så skal den falde ned når den murstenen ødelægges.
                    if (PowerContained != null)
                    {
                        PowerContained.Position = new Vector2(position.X, position.Y);
                        GameWorld.Instance.AddGameObject(PowerContained);
                    }
                }
            }
        }

        /// <summary>
        /// Opdaterer murstenes sprite baseret på de resternde hitpoints.
        /// </summary>
        /// <param name="content"></param> Contentmanager bruges til at indlæse den nye sprite
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