using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mousegame;
using System;
using System.Collections.Generic;

namespace mousegame
{
    /*TO-DO:
        - Randomize enemy attributes (speed, spawn time, spawn position)
            - Make enemies all spawn far away from Player
        - Powerups:
            - Alter time
            - Ice
            - (Knife) explosion
            - Invulnerable
        - Randomize powerup attributes (spawn time, spawn position)
            - Make powerups all spawn far away from Player
        - HUD
        - Menu
    */


    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        private bool devMode = false;
        private int score;
        Random random;
        private string gameState, oldGameState;
        Texture2D pauseTexture;

        private Player player;
        private MouseState mouseState;
        Texture2D playerTexture;

        Texture2D enemyTexture;
        List<Enemy> enemies;
        TimeSpan enemySpawnTime;
        TimeSpan prevEnemySpawnTime;

        Texture2D slowTimeTexture;
        Texture2D freezeTexture;
        Texture2D explosionTexture;
        Texture2D invulnerableTexture;
        Texture2D slowTimeTexturePlayer;
        Texture2D freezeTexturePlayer;
        Texture2D explosionTexturePlayer;
        Texture2D invulnerableTexturePlayer;
        int slowTimeDuration;
        int freezeDuration;
        int explosionDuration;
        int invulnerableDuration;
        /*TimeSpan prevSlowTimeSpan;
        TimeSpan prevFreezeSpan;
        TimeSpan prevExplosionSpan;
        TimeSpan prevInvulnerableSpan;*/
        bool slowTime = false;
        bool freeze = false;
        bool invulnerable = false;
        bool explosion = false;
        List<Powerup> powerups;
        TimeSpan powerupSpawnTime;
        TimeSpan prevPowerupSpawnTime;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width-500;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height-200;
        }

        protected override void Initialize()
        {
            gameState = "paused";
            oldGameState = "";
            random = new Random();
            score = 0;

            player = new Player();
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("Graphics\\blueDot"), playerPosition);

            enemies = new List<Enemy>();
            prevEnemySpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(random.Next(2, 5));

            powerups = new List<Powerup>();
            prevPowerupSpawnTime = TimeSpan.Zero;
            powerupSpawnTime = TimeSpan.FromSeconds(random.Next(4, 8));

            slowTimeDuration = 0;
            freezeDuration = 0;
            explosionDuration = 0;
            invulnerableDuration = 0;

            /*slowTimeDuration = TimeSpan.FromSeconds(5.0f);
            freezeDuration = TimeSpan.FromSeconds(5.0f);
            explosionDuration = TimeSpan.FromSeconds(0f);
            invulnerableDuration = TimeSpan.FromSeconds(5.0f);
            prevSlowTimeSpan = TimeSpan.Zero;
            prevFreezeSpan = TimeSpan.Zero;
            prevExplosionSpan = TimeSpan.Zero;
            prevInvulnerableSpan = TimeSpan.Zero;*/

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Graphics\\gameFont");
            pauseTexture = Content.Load<Texture2D>("Graphics\\pause");

            playerTexture = Content.Load<Texture2D>("Graphics\\blueDot");

            enemyTexture = Content.Load<Texture2D>("Graphics\\redDot");

            slowTimeTexture = Content.Load<Texture2D>("Graphics\\blackDot");
            freezeTexture = Content.Load<Texture2D>("Graphics\\cyanDot");
            explosionTexture = Content.Load<Texture2D>("Graphics\\orangeDot");
            invulnerableTexture = Content.Load<Texture2D>("Graphics\\greenDot");

            slowTimeTexturePlayer = Content.Load<Texture2D>("Graphics\\blackUp");
            freezeTexturePlayer = Content.Load<Texture2D>("Graphics\\cyanUp");
            explosionTexturePlayer = Content.Load<Texture2D>("Graphics\\orangeUp");
            invulnerableTexturePlayer = Content.Load<Texture2D>("Graphics\\greenUp");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if(gameState == "paused")
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    gameState = "game";
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Environment.Exit(0);
                }
            }
            if (gameState == "game")
            {
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    devMode = true;
                }
                if (devMode == true)
                {
                    enemySpawnTime = TimeSpan.FromSeconds(1.0f);
                    powerupSpawnTime = TimeSpan.FromSeconds(1000.0f);
                }
                UpdatePlayer(gameTime);
                UpdateEnemies(gameTime);
                UpdatePowerups(gameTime);
                UpdateCollisions();
                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BlanchedAlmond);
            spriteBatch.Begin();
            if(gameState == "paused")
            {
                spriteBatch.Draw(pauseTexture, new Vector2(50,-60), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            if (gameState == "game")
            {
                if (player.isActive == true)
                {
                    player.Draw(spriteBatch);
                }

                foreach (Enemy enemy in enemies.ToArray())
                {
                    if (enemy.isActive)
                    {
                        enemy.Draw(spriteBatch);
                    }
                }

                foreach (Powerup powerup in powerups.ToArray())
                {
                    if (powerup.isActive)
                    {
                        powerup.Draw(spriteBatch);
                    }
                }

                spriteBatch.DrawString(font, slowTimeDuration.ToString(), new Vector2(20, 20), Color.Black);
                spriteBatch.DrawString(font, freezeDuration.ToString(), new Vector2(120, 20), Color.Black);
                spriteBatch.DrawString(font, invulnerableDuration.ToString(), new Vector2(220, 20), Color.Black);
                spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(420, 20), Color.Black);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            player.position = mousePosition;
        }

        private void AddEnemy()
        {
            int whichWall = random.Next(1, 5);
            Vector2 position;
            if(whichWall == 1) //north
            {
                position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + random.Next(0, GraphicsDevice.Viewport.Width), GraphicsDevice.Viewport.TitleSafeArea.Y + random.Next(0, 100));
            }
            else if (whichWall == 2) //east
            {
                position = new Vector2(GraphicsDevice.Viewport.Width - random.Next(0, 100), GraphicsDevice.Viewport.TitleSafeArea.Y + random.Next(0, GraphicsDevice.Viewport.Height));
            }
            else if (whichWall == 3) //south
            {
                position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + random.Next(0, GraphicsDevice.Viewport.Width), GraphicsDevice.Viewport.Height - random.Next(0, 100));
            }
            else //west
            {
                position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + random.Next(0, 100), GraphicsDevice.Viewport.TitleSafeArea.Y + random.Next(0, GraphicsDevice.Viewport.Height));
            }
            Enemy enemy = new Enemy();
            enemy.Initialize(enemyTexture, position);
            if(slowTime == true)
            {
                enemy.enemyMoveSpeed /= 2;
            }
            enemies.Add(enemy);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - prevEnemySpawnTime > enemySpawnTime)
            {
                prevEnemySpawnTime = gameTime.TotalGameTime;
                AddEnemy();
            }
            foreach (Enemy enemy in enemies.ToArray())
            {
                if(freeze == false)
                {
                    enemy.Update(gameTime, player);
                }
                if (enemy.isActive == false)
                {
                    enemies.Remove(enemy);
                    score += 100;
                }
            }
        }

        private void AddPowerup()
        {
            Vector2 position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + random.Next(0, GraphicsDevice.Viewport.Width - slowTimeTexture.Width), GraphicsDevice.Viewport.TitleSafeArea.Y + random.Next(0, GraphicsDevice.Viewport.Height - slowTimeTexture.Width));
            Powerup powerup = new Powerup();
            int powerupChoice = random.Next(1, 5);
            if (powerupChoice == 1)
            {
                powerup.Initialize(slowTimeTexture, position);
            }
            else if(powerupChoice == 2)
            {
                powerup.Initialize(freezeTexture, position);
            }
            else if (powerupChoice == 3)
            {
                powerup.Initialize(explosionTexture, position);
            }
            else if (powerupChoice == 4)
            {
                powerup.Initialize(invulnerableTexture, position);
            }
            powerups.Add(powerup);
        }

        private void UpdatePowerups(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - prevPowerupSpawnTime > powerupSpawnTime)
            {
                prevPowerupSpawnTime = gameTime.TotalGameTime;
                AddPowerup();
            }
            if (slowTimeDuration == 0 && freezeDuration == 0 && explosionDuration == 0 && invulnerableDuration == 0)
            {
                player.playerTexture = playerTexture;
            }
            if(slowTimeDuration != 0)
            {
                slowTimeDuration--;
                if(slowTimeDuration == 0)
                {
                    slowTime = false;
                }
            }
            if (freezeDuration != 0)
            {
                freezeDuration--;
                if (freezeDuration == 0)
                {
                    freeze = false;
                }
            }
            if (invulnerableDuration != 0)
            {
                invulnerableDuration--;
                if (invulnerableDuration == 0)
                {
                    invulnerable = false;
                }
            }
            foreach (Powerup powerup in powerups.ToArray())
            {
                if (powerup.isActive == false)
                {
                    powerups.Remove(powerup);
                }
            }
        }

        private void UpdateCollisions()
        {
            Rectangle rectangle1;
            Rectangle rectangle2;
            rectangle1 = new Rectangle((int)player.position.X, (int)player.position.Y, player.Width, player.Height);
            foreach (Enemy enemy in enemies.ToArray())
            {
                rectangle2 = new Rectangle((int)enemy.position.X, (int)enemy.position.Y, enemy.Width, enemy.Height);
                if(rectangle1.Intersects(rectangle2))       
                {
                    enemy.isActive = false;
                    if (devMode == false && invulnerable == false && freeze == false)
                    {
                        reset();
                    }
                }
            }
            foreach (Powerup powerup in powerups.ToArray())
            {
                rectangle2 = new Rectangle((int)powerup.position.X, (int)powerup.position.Y, powerup.Width, powerup.Height);
                if (rectangle1.Intersects(rectangle2))
                {
                    if(powerup.powerupTexture == slowTimeTexture)
                    {
                        slowTimeDuration = 200;
                        slowTime = true;
                        player.playerTexture = slowTimeTexturePlayer;
                        foreach (Enemy enemy in enemies.ToArray())
                        {
                            enemy.enemyMoveSpeed /= 2;
                        }
                    }
                    else if (powerup.powerupTexture == freezeTexture)
                    {
                        freezeDuration = 200;
                        freeze = true;
                        player.playerTexture = freezeTexturePlayer;
                    }
                    else if (powerup.powerupTexture == explosionTexture)
                    {
                        foreach (Enemy enemy in enemies.ToArray())
                        {
                            enemy.isActive = false;
                        }
                    }
                    else if (powerup.powerupTexture == invulnerableTexture)
                    {
                        invulnerableDuration = 200;
                        invulnerable = true;
                        player.playerTexture = invulnerableTexturePlayer;
                    }
                    powerup.isActive = false;
                }
            }
        }

        private void reset()
        {
            gameState = "paused";
            score = 0;
            enemies.Clear();
            powerups.Clear();
            slowTimeDuration = 0;
            freezeDuration = 0;
            invulnerableDuration = 0;
        }
    }
}
