using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BrickBreakerGame
{
    public class Power : GameObject
    {
        public enum PowerType { PowerUp, PowerDown }

        public PowerType Type { get; set; }

        public string Effect { get; set; }

        private float speed = 150f;
        private float duration = 5f;
        private bool isEffectActive;
        private float effectTimer;

        public Power(PowerType type, string effect)
        {
            Type = type;
            Effect = effect;
        }

        public override void LoadContent(ContentManager content)
        {
            if (Type == PowerType.PowerUp)
            {
                sprite = content.Load<Texture2D>("LifeSprites/Life007"); // Brug en sprite for PowerUp
            }
            else if (Type == PowerType.PowerDown)
            {
                sprite = content.Load<Texture2D>("LifeSprites/Life000"); // Brug en sprite for PowerDown
            }
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            if (!isEffectActive)
            {
                // Power falder nedad
                position.Y += speed * GameWorld.DeltaTime(gameTime);

                // Hvis den når bunden af skærmen, fjernes den fra gameObjects-listen
                if (position.Y > GameWorld.Height)
                {
                    GameWorld.Instance.RemoveGameObject(this);
                }
            }
            else
            {
                // Hvis effekten er aktiv, hold styr på timeren
                effectTimer += GameWorld.DeltaTime(gameTime);

                if (effectTimer >= duration)
                {
                    isEffectActive = false; // Markér effekten som færdig
                }
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Paddle)
            {
                ActivateEffect();
                GameWorld.Instance.soundManager.PlaySound("pick_up"); // Afspil lyd, når paddle samler power-up op

                // Tilføj til aktive effekter og fjern kun power-ikonet fra visningen
                GameWorld.Instance.AddActivePower(this);
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
                    paddle.IncreasePaddleSize();
                    break;

                case "ReducePaddleSize":
                    paddle.DecreasePaddleSize();
                    break;

                case "IncreaseBallSpeed":
                    ball.IncreaseBallSpeed();
                    break;

                case "ReduceBallSpeed":
                    ball.DecreaseBallSpeed();
                    break;
            }

            isEffectActive = true;
            effectTimer = 0f;
        }

        public void UpdatePowerEffect(GameTime gameTime)
        {
            if (isEffectActive)
            {
                effectTimer += GameWorld.DeltaTime(gameTime);

                if (effectTimer >= duration)
                {
                    isEffectActive = false;
                }
            }
        }

        public bool IsEffectCompleted()
        {
            return !isEffectActive; // Returnér true, hvis effekten er færdig
        }
    }
}
