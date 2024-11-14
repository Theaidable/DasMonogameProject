using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BrickBreakerGame
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private LevelManager levelManager;
        private ScoreManager scoreManager;
        private SpriteFont scoreFont;
        public Paddle paddle;
        public Ball ball;

        private List<GameObject> gameObjects;
        private List<GameObject> removeObjects;
        private List<Power> activePowers;

        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static GameWorld Instance { get; private set; }

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
        }

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

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            GameObject.LoadDebugContent(GraphicsDevice);
            // Indlæs font
            scoreFont = Content.Load<SpriteFont>("ScoreFont");

            // Opret Paddle og Ball
            paddle = new Paddle();
            paddle.LoadContent(Content);
            AddGameObject(paddle);

            ball = new Ball();
            ball.LoadContent(Content);
            ball.Position = new Vector2(paddle.Position.X, paddle.Position.Y - paddle.Sprite.Height / 2 - ball.Sprite.Height / 2 - 5);
            AddGameObject(ball);

            levelManager = new LevelManager(Content);
            levelManager.CreateFirstLevel(gameObjects);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Opdater gameObjects som normalt
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }

            // Opdater aktive Power-effekter
            UpdateActivePowers(gameTime);

            // Kollisionsdetektion
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

            // Fjern gameObjects der skal fjernes
            foreach (GameObject gameObject in removeObjects)
            {
                gameObjects.Remove(gameObject);
            }
            removeObjects.Clear();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(_spriteBatch);
            }
            // Tegn score og highscore på skærmen
            _spriteBatch.DrawString(scoreFont, $"Score: {scoreManager.Score}", new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(scoreFont, $"High Score: {scoreManager.HighScore}", new Vector2(10, 30), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // Kald AddPoints i OnCollision, når bolden rammer en mursten
        public void AddPointsToScore(int points)
        {
            scoreManager.AddPoints(points);
        }

        public void AddActivePower(Power power)
        {
            activePowers.Add(power);
        }

        public void AddGameObject(GameObject newObject)
        {
            gameObjects.Add(newObject);
        }

        public void RemoveGameObject(GameObject removeObject)
        {
            if (!removeObjects.Contains(removeObject))
            {
                removeObjects.Add(removeObject);
            }
        }

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

        public static float DeltaTime(GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
