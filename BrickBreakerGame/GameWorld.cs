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

        private List<GameObject> gameObjects;

        public static int Width { get; private set; }
        public static int Height { get; private set; }

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Initialsier liste med alle game objects
            gameObjects = new List<GameObject>();

            // Initialiser skærmstørrelse
            Width = _graphics.PreferredBackBufferWidth;
            Height = _graphics.PreferredBackBufferHeight;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Indlæs debug-texutre i GameObject-klassen
            GameObject.LoadDebugContent(GraphicsDevice);

            //Opret instans af Paddle og tilføj til gameObjects listen
            Paddle paddle = new Paddle();
            paddle.LoadContent(Content);
            AddGameObject(paddle);

            Ball ball = new Ball();
            ball.LoadContent(Content);

            // Nu hvor boldens sprite er indlæst, kan vi sætte startpositionen
            ball.Position = new Vector2(paddle.Position.X, paddle.Position.Y - paddle.Sprite.Height / 2 - ball.Sprite.Height / 2 - 5);

            AddGameObject(ball);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }

            for (int i = 0; i < gameObjects.Count; i++)
            {
                for (int j = 0; j < gameObjects.Count; j++)
                {
                    if (gameObjects[i].CheckCollision(gameObjects[j]))
                    {
                        gameObjects[i].OnCollision(gameObjects[j]);
                        gameObjects[j].OnCollision(gameObjects[i]);
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
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

        public static float DeltaTime(GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
