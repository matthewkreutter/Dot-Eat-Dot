using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mousegame;
using System;
using System.Collections.Generic;

namespace mousegame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        private bool devMode = false;
        private int score;
        private Random random;
        private string gameState;
        private string choiceState;
        private Texture2D mouseTexture;
        private string pauseText;
        private string normalText;
        private Color normalTextColor;
        private string bombText;
        private Color bombTextColor;
        private bool bombMode;
        private int elapsedTime;
        private int level;

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private KeyboardState clearKeyboardState;

        private Player player;
        private MouseState mouseState;
        private Texture2D playerTexture;

        private Texture2D enemyTexture;
        private List<Enemy> enemies;
        private TimeSpan enemySpawnTime;
        private TimeSpan prevEnemySpawnTime;
        private int spawnLower, spawnUpper;
        private int enemySpeed;

        private Texture2D slowTimeTexture;
        private Texture2D freezeTexture;
        private Texture2D explosionTexture;
        private Texture2D invulnerableTexture;
        private Texture2D slowTimeTexturePlayer;
        private Texture2D freezeTexturePlayer;
        private Texture2D explosionTexturePlayer;
        private Texture2D invulnerableTexturePlayer;
        private int slowTimeDuration;
        private int freezeDuration;
        private int explosionDuration;
        private int invulnerableDuration;
        /*TimeSpan prevSlowTimeSpan;
        TimeSpan prevFreezeSpan;
        TimeSpan prevExplosionSpan;
        TimeSpan prevInvulnerableSpan;*/
        private bool slowTime = false;
        private bool freeze = false;
        private bool invulnerable = false;
        //private bool explosion = false;
        private List<Powerup> powerups;
        private TimeSpan powerupSpawnTime;
        private TimeSpan prevPowerupSpawnTime;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            gameState = "paused";
            choiceState = "normal";
            pauseText = "You are a navy blue dot, controlled by the mouse. Select a mode using the arrow keys and then press enter to play.";
            normalText = "NORMAL MODE:\nEvil red dots spawn and charge at you until you die.\nYou have Slow Time (black dot), Freeze (cyan dot),\nExplosion (orange dot) and Invulnerability (green dot) at your disposal.";
            bombText = "BOMB MODE:\nRed dots spawn and move much faster, but die when they collide with each other.";
            normalTextColor = Color.Black;
            bombTextColor = Color.Black;
            bombMode = false;
            random = new Random();
            score = 0;
            elapsedTime = 0;
            level = 1;

            player = new Player();
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("Graphics\\blueDot"), playerPosition);

            enemies = new List<Enemy>();
            enemySpeed = 200;
            prevEnemySpawnTime = TimeSpan.Zero;
            spawnLower = 4;
            spawnUpper = 6;
            enemySpawnTime = TimeSpan.FromSeconds(random.Next(spawnLower, spawnUpper));

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
            mouseTexture = Content.Load<Texture2D>("Graphics\\mouse");

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
            UpdatePlayer(gameTime);
            if (gameState == "paused")
            {
                currentKeyboardState = Keyboard.GetState();
                if (choiceState == "normal")
                {
                    bombTextColor = Color.Black;
                    normalTextColor = Color.Red;
                    if ((currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down)) || (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up)))
                    {
                        choiceState = "bomb";
                    }
                    if(currentKeyboardState.IsKeyDown(Keys.Enter))
                    {
                        gameState = "game";
                        bombMode = false;
                    }
                }
                else if (choiceState == "bomb")
                {
                    bombTextColor = Color.Red;
                    normalTextColor = Color.Black;
                    if ((currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down)) || (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up)))
                    {
                        choiceState = "normal";
                    }
                    if (currentKeyboardState.IsKeyDown(Keys.Enter))
                    {
                        gameState = "game";
                        bombMode = true;
                        spawnLower = 2;
                        spawnUpper = 3;
                        enemySpawnTime = TimeSpan.FromSeconds(random.Next(spawnLower, spawnUpper));
                        enemySpeed += 300;
                    }
                }
                else
                {
                    normalTextColor = Color.Black;
                    bombTextColor = Color.Black;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
                {
                    Environment.Exit(0);
                }
                previousKeyboardState = currentKeyboardState;
            }
            if (gameState == "game")
            {
                currentKeyboardState = Keyboard.GetState();
                if (currentKeyboardState.IsKeyDown(Keys.D))
                {
                    devMode = true;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
                {
                    gameState = "paused";
                    reset();
                }
                if (devMode == true)
                {
                    enemySpawnTime = TimeSpan.FromSeconds(1.0f);
                    powerupSpawnTime = TimeSpan.FromSeconds(3.0f);
                }
                elapsedTime++;
                if(elapsedTime % 500 == 0)
                {
                    harder();
                }
                UpdateEnemies(gameTime);
                UpdatePowerups(gameTime);
                UpdateCollisions();
                if(bombMode == true)
                {
                    UpdateEnemyCollisions();
                }
                previousKeyboardState = currentKeyboardState;
                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BlanchedAlmond);
            spriteBatch.Begin();
            if(devMode == true)
            {
                spriteBatch.DrawString(font, "(X, Y):" + mouseState.X + ", " + mouseState.Y, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width - 300, 100), Color.Black);
                spriteBatch.DrawString(font, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width + " " + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width - 300, 200), Color.Black);
            }
            if (gameState == "paused")
            {//FIX RESOLUTION DEPENDENCY
                //spriteBatch.Draw(mouseTexture, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - mouseTexture.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Height / 8), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, pauseText, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 4, GraphicsDevice.Viewport.TitleSafeArea.Height / 4), Color.Black);
                spriteBatch.DrawString(font, normalText, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 4, GraphicsDevice.Viewport.TitleSafeArea.Height / 3), normalTextColor);
                spriteBatch.DrawString(font, bombText, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 4, GraphicsDevice.Viewport.TitleSafeArea.Height / 2), bombTextColor);
            }
            if (gameState == "game")
            {
                if (player.getActive == true)
                {
                    player.Draw(spriteBatch);
                }

                foreach (Enemy enemy in enemies.ToArray())
                {
                    if (enemy.getActive)
                    {
                        enemy.Draw(spriteBatch);
                    }
                }

                foreach (Powerup powerup in powerups.ToArray())
                {
                    if (powerup.getActive)
                    {
                        powerup.Draw(spriteBatch);
                    }
                }
                spriteBatch.DrawString(font, slowTimeDuration.ToString(), new Vector2(20, GraphicsDevice.Viewport.TitleSafeArea.Height - 50), Color.Black);
                spriteBatch.DrawString(font, freezeDuration.ToString(), new Vector2(120, GraphicsDevice.Viewport.TitleSafeArea.Height - 50), Color.Black);
                spriteBatch.DrawString(font, invulnerableDuration.ToString(), new Vector2(220, GraphicsDevice.Viewport.TitleSafeArea.Height - 50), Color.Black);
                spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(40, 20), Color.Black);
                spriteBatch.DrawString(font, "Elapsed Time: " + elapsedTime.ToString(), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width - 300, 20), Color.Black);
                spriteBatch.DrawString(font, "Level: " + level.ToString(), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width - 300, 60), Color.Black);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            player.setPosition(mousePosition);
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
            enemy.Initialize(enemyTexture, position, enemySpeed);
            if(slowTime == true)
            {
                enemy.setSpeed(enemy.getSpeed / 2);
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
                if (enemy.getActive == false)
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
                player.setTexture(playerTexture);
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
                if (powerup.getActive == false)
                {
                    powerups.Remove(powerup);
                }
            }
        }

        private void UpdateCollisions()
        {
            Rectangle rectangle1;
            Rectangle rectangle2;
            rectangle1 = new Rectangle((int)player.getPosition.X, (int)player.getPosition.Y, player.Width, player.Height);
            foreach (Enemy enemy in enemies.ToArray())
            {
                rectangle2 = new Rectangle((int)enemy.getPosition.X, (int)enemy.getPosition.Y, enemy.Width, enemy.Height);
                if(rectangle1.Intersects(rectangle2))       
                {
                    enemy.setActive(false);
                    if (devMode == false && invulnerable == false && freeze == false)
                    {
                        reset();
                    }
                }
            }
            foreach (Powerup powerup in powerups.ToArray())
            {
                rectangle2 = new Rectangle((int)powerup.getPosition.X, (int)powerup.getPosition.Y, powerup.Width, powerup.Height);
                if (rectangle1.Intersects(rectangle2))
                {
                    if(powerup.getTexture == slowTimeTexture)
                    {
                        slowTimeDuration = 200;
                        slowTime = true;
                        player.setTexture(slowTimeTexturePlayer);
                        foreach (Enemy enemy in enemies.ToArray())
                        {
                            enemy.setSpeed(enemy.getSpeed / 2);
                        }
                    }
                    else if (powerup.getTexture == freezeTexture)
                    {
                        freezeDuration = 200;
                        freeze = true;
                        player.setTexture(freezeTexturePlayer);
                    }
                    else if (powerup.getTexture == explosionTexture)
                    {
                        foreach (Enemy enemy in enemies.ToArray())
                        {
                            enemy.setActive(false);
                        }
                    }
                    else if (powerup.getTexture == invulnerableTexture)
                    {
                        invulnerableDuration = 200;
                        invulnerable = true;
                        player.setTexture(invulnerableTexturePlayer);
                    }
                    powerup.setActive(false);
                }
            }
        }

        private void UpdateEnemyCollisions()
        {
            Rectangle rectangle1;
            Rectangle rectangle2;
            foreach (Enemy enemy1 in enemies.ToArray())
            {
                foreach (Enemy enemy2 in enemies.ToArray())
                {
                    rectangle1 = new Rectangle((int)enemy1.getPosition.X, (int)enemy1.getPosition.Y, enemy1.Width, enemy1.Height);
                    rectangle2 = new Rectangle((int)enemy2.getPosition.X, (int)enemy2.getPosition.Y, enemy2.Width, enemy2.Height);
                    if (!rectangle1.Equals(rectangle2) && rectangle1.Intersects(rectangle2))
                    {
                        enemy1.setActive(false);
                        enemy2.setActive(false);
                    }
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
            level = 0;
            elapsedTime = 0;
            player.setTexture(playerTexture);
            spawnLower = 4;
            spawnUpper = 6;
            enemySpawnTime = TimeSpan.FromSeconds(random.Next(spawnLower, spawnUpper));
            enemySpeed = 200;
        }

        private void harder()
        {
            if(spawnLower > 1)
            {
                spawnLower--;
            }
            if(spawnUpper > 1)
            {
                spawnUpper--;
            }
            enemySpawnTime = TimeSpan.FromSeconds(random.Next(spawnLower, spawnUpper));
            enemySpeed += 25;
            level++;
        }
    }
}
