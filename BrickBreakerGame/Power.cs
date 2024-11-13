using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BrickBreakerGame
{
    public class Power : GameObject
    {
        public enum PowerType { PowerUp, PowerDown}

        public PowerType Type { get; set; }

        public string Effect { get; set; }

        private float speed = 150f;

        public Power(PowerType type,string effect)
        {
            Type = type;
            Effect = effect;
        }

        public override void LoadContent(ContentManager content)
        {
            if (Type == PowerType.PowerUp)
            {
                sprite = content.Load<Texture2D>("LifeSprites/Life002"); // Brug en sprite for PowerUp
            }
            else if (Type == PowerType.PowerDown)
            {
                sprite = content.Load<Texture2D>("LifeSprites/Life000"); // Brug en sprite for PowerDown
            }
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            // Power falder nedad
            position.Y += speed * GameWorld.DeltaTime(gameTime);

            // Hvis den når bunden af skærmen, fjernes den
            if (position.Y > GameWorld.Height)
            {
                GameWorld.Instance.RemoveGameObject(this);
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Paddle)
            {
                ActivateEffect();
                GameWorld.Instance.RemoveGameObject(this);
            }
        }

        private void ActivateEffect()
        {
            Paddle paddle = GameWorld.Instance.paddle;
            Ball ball = GameWorld.Instance.ball;

            switch (Effect)
            {
                case "IncreasePaddleSize":
                    paddle.IncreaseSize();
                    break;

                case "ReducePaddleSize":
                    paddle.DecreaseSize();
                    break;

                case "IncreaseBallSpeed":
                    ball.IncreaseSpeed();
                    break;

                case "ReduceBallSpeed":
                    ball.DecreaseSpeed();
                    break;
            }
        }
    }
}
