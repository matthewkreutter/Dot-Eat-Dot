using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mousegame
{
    class Player
    {
        public Texture2D playerTexture;
        public Vector2 position;
        public bool isActive;
        //public int Health;

        public int Width
        {
            get { return playerTexture.Width; }
        }

        public int Height
        {
            get { return playerTexture.Height; }
        }

        public void Initialize(Texture2D inTexture, Vector2 inPosition)
        {
            playerTexture = inTexture;
            position = inPosition;
            isActive = true;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
