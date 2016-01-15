using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mousegame
{
    class Player
    {
        private Texture2D playerTexture;
        private Vector2 position;
        private bool isActive;
        //public int Health;

        public int Width
        {
            get { return playerTexture.Width; }
        }

        public int Height
        {
            get { return playerTexture.Height; }
        }

        public Texture2D getTexture { get { return playerTexture; }}
        public void setTexture(Texture2D inTexture) { playerTexture = inTexture; }
        public Vector2 getPosition { get { return position; } }
        public void setPosition(Vector2 inPosition) {  position = inPosition;  }
        public bool getActive { get { return isActive; } }
        public void setActive(bool inActive) { isActive = inActive; }

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
