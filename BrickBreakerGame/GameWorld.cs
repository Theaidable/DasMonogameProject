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

        private List<GameObject> gameObjects;
        private List<GameObject> removeObjects;

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

            Width = _graphics.PreferredBackBufferWidth;
            Height = _graphics.PreferredBackBufferHeight;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            GameObject.LoadDebugContent(GraphicsDevice);

            // Opret Paddle og Ball
            Paddle paddle = new Paddle();
            paddle.LoadContent(Content);
            AddGameObject(paddle);

            Ball ball = new Ball();
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

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }

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

            _spriteBatch.End();

            base.Draw(gameTime);
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

        public static float DeltaTime(GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
