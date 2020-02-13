using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _580GameProject2
{
    public enum State
    {
        Down = 6,
        Right = 7,
        Left = 3,
        Up = 2,
        Clap = 4,
        Airfist = 5,
        Idle = 1,
    }
    public class Player
    {
        Game1 game;
        Texture2D texture;
        KeyboardState oldKeyboardState = Keyboard.GetState();
        public State state;
        TimeSpan timer;
        int frame;
        Rectangle position;
        //Vector2 position;
        SpriteFont font;
        

        /// <summary>
        /// How quickly the animation should advance frames (1/8 second as milliseconds)
        /// </summary>
        const int ANIMATION_FRAME_RATE = 124;

        /// <summary>
        /// The width of the animation frames
        /// </summary>
        const int FRAME_WIDTH = 110;

        /// <summary>
        /// The hieght of the animation frames
        /// </summary>
        const int FRAME_HEIGHT = 128;

        public Player(Game1 game, int x, int y)
        {
            this.game = game;
            position = new Rectangle(x, y, 200, 200);
            timer = new TimeSpan(0);
            state = State.Idle;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("dancer");
            font = game.Content.Load<SpriteFont>("defaultFont");
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update the player state based on input
            if (keyboard.IsKeyDown(Keys.Up))
            {
                state = State.Up;
            }
            else if (keyboard.IsKeyDown(Keys.Left))
            {
                state = State.Left;
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                state = State.Right;
            }
            else if (keyboard.IsKeyDown(Keys.Down))
            {
                state = State.Down;
            }
            else if (keyboard.IsKeyDown(Keys.C))
            {
                state = State.Clap;
            }
            else if (keyboard.IsKeyDown(Keys.F))
            {
                state = State.Airfist;
            }
            else state = State.Idle;

            // Update the player animation timer when the player is moving
            //if (state != State.Idle)
            timer += gameTime.ElapsedGameTime;

            // Determine the frame should increase.  Using a while 
            // loop will accomodate the possiblity the animation should 
            // advance more than one frame.
            while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                // increase by one frame
                frame++;
                // reduce the timer by one frame duration
                timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }

            // Keep the frame within bounds (there are four frames)
            frame %= 8;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(
                frame * FRAME_WIDTH, // X value 
                (int)state * FRAME_HEIGHT, // Y value
                FRAME_WIDTH, // Width 
                FRAME_HEIGHT // Height
                );
            spriteBatch.Draw(texture, position, source, Color.White);
            //spriteBatch.DrawString(font, $"State: {state}", Vector2.Zero, Color.White);

        }
    }
}
