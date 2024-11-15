using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BrickBreakerGame
{
    /// <summary>
    /// Repræsentere en PowerUp eller en PowerDown der har en effekt for spillet når den samles op af Paddle
    /// </summary>
    public class Power : GameObject
    {
        // Fields
        private float speed = 150f;
        private float duration = 5f;
        private bool isEffectActive;
        private float effectTimer;
        
        /// <summary>
        /// Enumurering for hvilken type af power (PowerUp eller PowerDown)
        /// </summary>
        public enum PowerType { PowerUp, PowerDown }

        /// <summary>
        /// Public property til at hente og definere typen af power (up eller down)
        /// </summary>
        public PowerType Type { get; set; }

        /// <summary>
        /// Public property til at hente og definere effekten af power
        /// </summary>
        public string Effect { get; set; }

        /// <summary>
        /// Initialisere en ny instance af "Power" klassen med en specefik type og effekt
        /// </summary>
        /// <param name="type"></param> Hvilken type af power (PowerUp eller PowerDown).
        /// <param name="effect"></param> Hvilken effekt poweren har
        public Power(PowerType type, string effect)
        {
            Type = type;
            Effect = effect;
        }

        /// <summary>
        /// Indlæser indholdet for power objektet
        /// </summary>
        /// <param name="content"></param> ContentManger til at indlæse spritesene
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

        /// <summary>
        /// Opdatere powerens postion og håndterer effekttimeren.
        /// </summary>
        /// <param name="gameTime"></param> Spillets tidsinformation.
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

        /// <summary>
        /// Håndterer kollision med andre objekter.
        /// </summary>
        /// <param name="other"></param> Det andet gameobject som Power kolliderer med.
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

        /// <summary>
        /// Aktiverer effekten afhængig af hvilken type det er.
        /// </summary>
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

        /// <summary>
        /// Opdatere den aktive effekt af poweren
        /// </summary>
        /// <param name="gameTime"></param> Spillets tidsinformation.
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

        /// <summary>
        /// Tjekker om effekten er færdig.
        /// </summary>
        /// <returns>Returnerer true, hvis effekten er færdig</returns>
        public bool IsEffectCompleted()
        {
            return !isEffectActive; // Returnér true, hvis effekten er færdig
        }
    }
}
