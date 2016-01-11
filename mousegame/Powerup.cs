using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mousegame
{
    class Powerup
    {
        public Texture2D powerupTexture;
        public Vector2 position;
        public bool isActive;

        public int Width
        {
            get { return powerupTexture.Width; }
        }

        public int Height
        {
            get { return powerupTexture.Height; }
        }

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
