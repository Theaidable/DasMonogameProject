using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreakerGame
{
    public class Brick : GameObject
    {
        private int hitPoints;
        private bool isIndestructable;
        private int points;

        public Brick(int initialHitPoints, bool indestructiable = false)
        {
            hitPoints = initialHitPoints;
            isIndestructable = indestructiable;
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
            if (isIndestructable)
            {
                sprite = content.Load<Texture2D>("black_brick");
            }
            else if (hitPoints == 1)
            {
                sprite = content.Load<Texture2D>("green_brick");
            }
            else if (hitPoints == 2)
            {
                sprite = content.Load<Texture2D>("yellow_brick");
            }
            else if (hitPoints == 3)
            {
                sprite = content.Load<Texture2D>("red_brick");
            }

            origin = new Microsoft.Xna.Framework.Vector2(sprite.Width / 2, sprite.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            //Ingen opdatering løbende spillet
        }

        public override void OnCollision(GameObject other)
        {
            if (isIndestructable) return;

            if (other is Ball)
            {
                hitPoints--;

                if (hitPoints <= 0)
                {
                    GameWorld.Instance.RemoveGameObject(this);
                }
            }
        }
    }
}
