using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mousegame
{
    class Enemy
    {
        private Texture2D enemyTexture;
        private Vector2 position;
        private bool isActive;
        private float enemyMoveSpeed;
        //public int health;

        public int Width
        {
            get { return enemyTexture.Width; }
        }

        public int Height
        {
            get { return enemyTexture.Height; }
        }

        public Texture2D getTexture { get { return enemyTexture; } }
        public Vector2 getPosition { get { return position; } }
        public bool getActive { get { return isActive; } }
        public void setActive(bool inActive) { isActive = inActive; }
        public float getSpeed {  get { return enemyMoveSpeed; } }
        public void setSpeed(float inSpeed) { enemyMoveSpeed = inSpeed; }

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
            Vector2 Move_Towards = -(position - inPlayer.getPosition);
            Move_Towards.Normalize();
            position = position + (Move_Towards * enemyMoveSpeed * elapsedTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemyTexture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
