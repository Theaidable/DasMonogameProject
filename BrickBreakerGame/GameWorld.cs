using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BrickBreakerGame
{
    /// <summary>
    /// Repræsenterer GameWorld, hvor alle spilobjekter og logik defineres.
    /// Denne klasse indeholde alle spilobjekter, opdaterer deres tildstand, håndtere input, og tegner dem på skærmen.
    /// </summary>
    public class GameWorld : Game
    {
        //Fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont scoreFont;
        private bool isGameOver;
        private List<GameObject> gameObjects;
        private List<GameObject> removeObjects;
        private List<Power> activePowers;

        //Kalder til de andre klasser
        public Paddle paddle;
        public Ball ball;
        public SoundManager soundManager;
        private LevelManager levelManager;
        private ScoreManager scoreManager;

        // Public properties til at få værdier til bredde, højde og en instans af GameWorld.
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static GameWorld Instance { get; private set; }

        /// <summary>
        /// Initialiserer en ny instans af "GameWorld" klassen.
        /// </summary>
        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
            isGameOver = false;
        }

        /// <summary>
        /// Initialiserer spilverdenen
        /// </summary>
        protected override void Initialize()
        {
            gameObjects = new List<GameObject>();
            removeObjects = new List<GameObject>();
            activePowers = new List<Power>();
            scoreManager = new ScoreManager();

            Width = _graphics.PreferredBackBufferWidth;
            Height = _graphics.PreferredBackBufferHeight;

            base.Initialize();
        }

        /// <summary>
        /// Indlæser spillets indhold, som grafik og lyde.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            GameObject.LoadDebugContent(GraphicsDevice);
            
            // Indlæs font
            scoreFont = Content.Load<SpriteFont>("ScoreFont");

            // Opret Paddle
            paddle = new Paddle();
            paddle.LoadContent(Content);
            AddGameObject(paddle);

            // Opret en Ball
            ball = new Ball();
            ball.LoadContent(Content);
            ball.Position = new Vector2(paddle.Position.X, paddle.Position.Y - paddle.Sprite.Height / 2 - ball.Sprite.Height / 2 - 5);
            AddGameObject(ball);

            // Initialiser LevelManager og indlæs det første level
            levelManager = new LevelManager(Content);
            levelManager.CreateFirstLevel(gameObjects);

            // Initialiser SoundManager og indlæs lyde
            soundManager = new SoundManager();
            soundManager.LoadContent(Content);

            //soundManager.PlaySound("game_over");  // Test af lyd

            // Afspil baggrundsmusik
            //soundManager.PlayBackgroundMusic();
        }

        /// <summary>
        /// Opdatere spillets tilstand
        /// </summary>
        /// <param name="gameTime"></param> Spillets tidsinformation
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (isGameOver)
            {
                HandleGameOverInput(); // Håndter input for at restarte eller stoppe
                return;
            }

            // Opdater gameObjects som normalt
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }

            UpdateActivePowers(gameTime);
            HandleCollisions();

            // Fjern gameObjects der skal fjernes
            foreach (GameObject gameObject in removeObjects)
            {
                gameObjects.Remove(gameObject);
            }
            removeObjects.Clear();

            base.Update(gameTime);
        }

        /// <summary>
        /// Håndtere kollisioner mellem alle spilobjekter.
        /// </summary>
        private void HandleCollisions()
        {
            // Kollisionsdetektion mellem gameObjects
            for (int i = 0; i < gameObjects.Count; i++)
            {
                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    if (gameObjects[i].CheckCollision(gameObjects[j]))
                    {
                        gameObjects[i].OnCollision(gameObjects[j]);
                        gameObjects[j].OnCollision(gameObjects[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Håndterer input, hvis spillet er slut.
        /// </summary>
        private void HandleGameOverInput()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.R)) // Tryk 'R' for at genstarte spillet
            {
                RestartGame();
            }
            else if (keyState.IsKeyDown(Keys.Q)) // Tryk 'Q' for at afslutte spillet
            {
                Exit();
            }
        }

        /// <summary>
        /// Genstarter spillet ved at nulstille alle relevante komponenter.
        /// </summary>
        private void RestartGame()
        {
            isGameOver = false;
            scoreManager.ResetScore();

            // Nulstil boldens position
            ball.Position = new Vector2(paddle.Position.X, paddle.Position.Y - paddle.Sprite.Height / 2 - ball.Sprite.Height / 2 - 5);
            ball.ResetDirectionAndSpeed();

            // Genopret niveauet og paddle
            gameObjects.Clear();
            removeObjects.Clear();
            levelManager.CreateFirstLevel(gameObjects);
            AddGameObject(paddle);
            AddGameObject(ball);
        }

        /// <summary>
        /// Tegner alle spilobjekter samt score og spilstatus.
        /// </summary>
        /// <param name="gameTime"></param> Spillets tidsinformation.
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (isGameOver)
            {
                _spriteBatch.DrawString(scoreFont, "Game Over! Press 'R' to Restart or 'Q' to Quit", new Vector2(100, Height / 2), Color.Red);
            }
            else
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Draw(_spriteBatch);
                }
                // Tegn score og highscore på skærmen
                _spriteBatch.DrawString(scoreFont, $"Score: {scoreManager.Score}", new Vector2(10, 10), Color.Black);
                _spriteBatch.DrawString(scoreFont, $"High Score: {scoreManager.HighScore}", new Vector2(10, 30), Color.Black);

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Kald denne metode når bolden falder ud af bunden i Ball.cs
        /// </summary>
        public void OnBallLost()
        {
            isGameOver = true;
            soundManager.PlaySound("game_over");
        }

        /// <summary>
        /// Tilføjer point til spillerens score.
        /// </summary>
        /// <param name="points"></param> Antallet af points der skal tilføjes
        public void AddPointsToScore(int points)
        {
            scoreManager.AddPoints(points);
        }

        /// <summary>
        /// Tilføjer et spilobjekt til spillet.
        /// </summary>
        /// <param name="power"></param> Spilobjektet, der skal tilføjes.
        public void AddActivePower(Power power)
        {
            activePowers.Add(power);
        }

        /// <summary>
        /// Tilføjer et spilobjekt til spillet.
        /// </summary>
        /// <param name="newObject"></param> Spilobjektet, der skal tilføjes.
        public void AddGameObject(GameObject newObject)
        {
            gameObjects.Add(newObject);
        }

        /// <summary>
        /// Marker et spilobjekt til fjernelse fra spillet.
        /// </summary>
        /// <param name="removeObject"></param> Objektet der skal fjernes.
        public void RemoveGameObject(GameObject removeObject)
        {
            if (!removeObjects.Contains(removeObject))
            {
                removeObjects.Add(removeObject);
            }
        }

        /// <summary>
        /// Opdaterer status for alle aktive powers.
        /// </summary>
        /// <param name="gameTime"></param> Spillets tidsinformation.
        private void UpdateActivePowers(GameTime gameTime)
        {
            for (int i = activePowers.Count - 1; i >= 0; i--)
            {
                Power power = activePowers[i];
                power.UpdatePowerEffect(gameTime);

                // Hvis effekten er færdig, fjern den
                if (power.IsEffectCompleted())
                {
                    DeactivatePower(power);
                    activePowers.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Deaktiverer en power-effekt efter den varighed.
        /// </summary>
        /// <param name="power"></param> Den power, der skal deaktiveres
        private void DeactivatePower(Power power)
        {
            // Brug den aktuelle effekt for at deaktivere
            switch (power.Effect)
            {
                case "IncreasePaddleSize":
                    paddle.DecreasePaddleSize(); // Gør paddlen mindre igen
                    break;

                case "ReducePaddleSize":
                    paddle.IncreasePaddleSize(); // Gør paddlen normal igen
                    break;

                case "IncreaseBallSpeed":
                    ball.DecreaseBallSpeed(); // Reducer boldens hastighed til normal
                    break;

                case "ReduceBallSpeed":
                    ball.IncreaseBallSpeed(); // Øg boldens hastighed tilbage til normal
                    break;
            }
        }

        /// <summary>
        /// Beregner tid siden sidste opdatering
        /// </summary>
        /// <param name="gameTime"></param> Spillets tidsinformaton.
        /// <returns> Returnerer tiden siden sidste opdatering som float</returns>
        public static float DeltaTime(GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
