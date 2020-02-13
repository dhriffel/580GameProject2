using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace _580GameProject2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Random random;
        TimeSpan moveTimer;
        State currentMove;
        SpriteFont font, bigFont;
        Song song;
        SoundEffect goodMove, badMove;
        bool scoreCheck, fadeCheck;
        int score, highScore, countdown, timeBetweenMoves, r, b;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 1000;
            graphics.ApplyChanges();
            fadeCheck = true;

            timeBetweenMoves = 6000;

            scoreCheck = true;
            score = 0;
            highScore = 0;
            countdown = 5;
            moveTimer = new TimeSpan(0);
            random = new Random(DateTime.Now.Second);
            player = new Player(this, (graphics.PreferredBackBufferWidth/2 -100), (graphics.PreferredBackBufferHeight / 2 -100));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            font = Content.Load<SpriteFont>("azonixFont");
            bigFont = Content.Load<SpriteFont>("azonixFontBig");
            song = Content.Load<Song>("backgroundMusic");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
            goodMove = Content.Load<SoundEffect>("goodMove");
            badMove = Content.Load<SoundEffect>("badMove");


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);

            moveTimer += gameTime.ElapsedGameTime;

            countdown = (5 - (int)Math.Floor(moveTimer.TotalSeconds));

            if (moveTimer.TotalMilliseconds >= 5000) {
                if (moveTimer.TotalMilliseconds > timeBetweenMoves) {
                    if (!scoreCheck)
                    {
                        scoreCheck = true;
                        badMove.Play();
                        moveTimer = new TimeSpan(0);
                        if (score > highScore)
                            highScore = score;
                        score = 0;
                        currentMove = 0;
                        timeBetweenMoves = 6000;
                    }
                    else
                    {
                        scoreCheck = false;
                        var values = Enum.GetValues(typeof(State));
                        do{
                            currentMove = (State)values.GetValue(random.Next(values.Length));
                        } while (currentMove == State.Idle);
                        moveTimer = new TimeSpan(0, 0, 5);
                    }
                }
                if (player.state == currentMove && !scoreCheck)
                {
                    goodMove.Play();
                    scoreCheck = true;
                    score++;
                    switch (score)
                    {
                        case 20:
                            timeBetweenMoves -= 100;
                            break;
                        case 40:
                            timeBetweenMoves -= 100;
                            break;
                        case 60:
                            timeBetweenMoves -= 100;
                            break;
                    }
                }

                
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (fadeCheck)
            {
                if (r < 256)
                    r += 1;
                else if (b < 256)
                    b += 1;
                else
                    fadeCheck = false;
            }
            else
            {
                if (r > 0)
                    r -= 1;
                else if (b > 0)
                    b -= 1;
                else
                    fadeCheck = true;
            }

            GraphicsDevice.Clear(new Color(r,0,b));

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            Vector2 scoreStringSize = font.MeasureString($"Score: {score}");
            spriteBatch.DrawString(font, $"Score: {score}", new Vector2(graphics.PreferredBackBufferWidth / 2 - scoreStringSize.X / 2, 10), Color.White);
            spriteBatch.DrawString(font, $"Highscore: {highScore}", new Vector2(0, 10), Color.White);

            if (countdown > 0)
            {
                Vector2 countdownStringSize = font.MeasureString($"{countdown}");
                spriteBatch.DrawString(bigFont, $"{countdown}", new Vector2(graphics.PreferredBackBufferWidth / 2 - countdownStringSize.X/2, 300), Color.White);
            }
            if(moveTimer.TotalMilliseconds < 5700 && currentMove != 0)
            {
                Vector2 moveStringSize = font.MeasureString($"{currentMove}");
                spriteBatch.DrawString(bigFont, $"{currentMove}", new Vector2(graphics.PreferredBackBufferWidth / 2 - moveStringSize.X/2, 300), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
