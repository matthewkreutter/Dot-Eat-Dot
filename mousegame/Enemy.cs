using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mousegame
{
    class Enemy
    {
        public Texture2D enemyTexture;
        public Vector2 position;
        public bool isActive;
        public float enemyMoveSpeed;
        //public int health;

        public int Width
        {
            get { return enemyTexture.Width; }
        }

        public int Height
        {
            get { return enemyTexture.Height; }
        }

        public void Initialize(Texture2D inTexture, Vector2 inPosition, int speed)
        {
            Random random = new Random();
            enemyTexture = inTexture;
            position = inPosition;
            isActive = true;
            enemyMoveSpeed = random.Next(speed, speed + 100);
        }

        public void Update(GameTime gameTime, Player inPlayer)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 Move_Towards = -(position - inPlayer.position);
            Move_Towards.Normalize();
            position = position + (Move_Towards * enemyMoveSpeed * elapsedTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemyTexture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
