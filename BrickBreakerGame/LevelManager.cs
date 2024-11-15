using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace BrickBreakerGame
{
    /// <summary>
    /// LevelManager klassen er ansvarlig for håndtere og skabe niveauer i spillet.
    /// Den opretter mursten med forskellige egenskaber og placerer dem i niveauet.
    /// </summary>
    public class LevelManager
    {
        // Fields
        private ContentManager content;
        private int hitPoints;
        private Random random = new Random();

        /// <summary>
        /// Constructor for LevelManager klassen.
        /// </summary>
        /// <param name="content"></param> ContentMAnager er for at indlæse spilindholdet.
        public LevelManager(ContentManager content)
        {
            this.content = content;
        }

        /// <summary>
        /// Opretter det første niveau i spillet med mursten og placerer dem på skærmen
        /// </summary>
        /// <param name="gameObjects"></param> Liste over spilobjekter som mursten, der skal tilføjes til spillet.
        public void CreateFirstLevel(List<GameObject> gameObjects)
        {
            int topMargin = 50; // Margin fra toppen af skærmen
            int sideMargin = 50; // Margin fra siderne af skærmen
            int spacing = 5; // Mellemrum mellem mursten

            // Venstre side mursten med lille mellemrum
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (row == 0) hitPoints = 1; // Grøn
                    else if (row == 1 || row == 2) hitPoints = 2; // Gul
                    else if (row == 3) hitPoints = 3; // Rød

                    Brick brick = new Brick(hitPoints);
                    brick.LoadContent(content);

                    int brickWidth = brick.Sprite.Width;
                    int brickHeight = brick.Sprite.Height;

                    int xPosition = sideMargin + col * (brickWidth + spacing);
                    int yPosition = topMargin + row * (brickHeight + spacing);

                    brick.Position = new Vector2(xPosition, yPosition);
                    gameObjects.Add(brick);
                }
            }

            // Højre side mursten (spejlvendt) med lille mellemrum
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (row == 0) hitPoints = 1; // Grøn
                    else if (row == 1 || row == 2) hitPoints = 2; // Gul
                    else if (row == 3) hitPoints = 3; // Rød

                    Brick brick = new Brick(hitPoints);
                    brick.LoadContent(content);

                    int brickWidth = brick.Sprite.Width;
                    int brickHeight = brick.Sprite.Height;

                    int xPosition = GameWorld.Width - sideMargin - (5 - col) * (brickWidth + spacing);
                    int yPosition = topMargin + row * (brickHeight + spacing);

                    brick.Position = new Vector2(xPosition, yPosition);
                    gameObjects.Add(brick);
                }
            }

            // Midterste uødelæggelige mursten
            for (int row = 0; row < 2; row++)
            {
                Brick indestructibleBrick = new Brick(1, true);
                indestructibleBrick.LoadContent(content);

                int brickWidth = indestructibleBrick.Sprite.Width;
                int brickHeight = indestructibleBrick.Sprite.Height;

                int xPosition = GameWorld.Width / 2 - brickWidth / 2;
                int yPosition = GameWorld.Height / 4 + row * (brickHeight + spacing);

                indestructibleBrick.Position = new Vector2(xPosition, yPosition);
                gameObjects.Add(indestructibleBrick);
            }

            // Tilføj PowerUps og PowerDowns til nogle af murstene
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] is Brick brick && !brick.IsIndestructible)
                {
                    int randomChance = random.Next(100); // Generer et tal mellem 0 og 99
                    if (randomChance < 40) // 40% chance for en Power
                    {
                        Power.PowerType powerType = randomChance < 20 ? Power.PowerType.PowerUp : Power.PowerType.PowerDown;

                        // Vælg en tilfældig effekt afhængigt af PowerType
                        string effect = "";
                        if (powerType == Power.PowerType.PowerUp)
                        {
                            string[] possibleEffects = { "IncreasePaddleSize", "DecreaseBallSpeed"};
                            effect = possibleEffects[random.Next(possibleEffects.Length)];
                        }
                        else if (powerType == Power.PowerType.PowerDown)
                        {
                            string[] possibleEffects = { "ReducePaddleSize", "IncreaseBallSpeed"};
                            effect = possibleEffects[random.Next(possibleEffects.Length)];
                        }

                        Power power = new Power(powerType, effect);
                        power.LoadContent(content);
                        brick.PowerContained = power;
                    }
                }
            }
        }
    }
}
