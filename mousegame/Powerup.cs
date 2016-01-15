using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mousegame
{
    class Powerup
    {
        private Texture2D powerupTexture;
        private Vector2 position;
        private bool isActive;

        public int Width
        {
            get { return powerupTexture.Width; }
        }

        public int Height
        {
            get { return powerupTexture.Height; }
        }

        public Texture2D getTexture { get { return powerupTexture; } }
        public Vector2 getPosition { get { return position; } }
        public bool getActive { get { return isActive; } }
        public void setActive(bool inActive) { isActive = inActive; }

        public void Initialize(Texture2D inTexture, Vector2 inPosition)
        {
            powerupTexture = inTexture;
            position = inPosition;
            isActive = true;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(powerupTexture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
