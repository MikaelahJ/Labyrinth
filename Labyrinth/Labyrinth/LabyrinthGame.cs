using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Labyrinth
{
    public class LabyrinthGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont spriteFont;
        private SpriteFont spriteFont2;
        public static string text = "Press ENTER to switch walls";
        public static bool hasWon;
        int secondsPassed;
        int minutesPassed;
        int hoursPassed;
        int frame;

        private InputList gameInput;

        public const int GAME_WIDTH = 1920;
        public const int GAME_HEIGHT = 1080;
        public const float GAME_UPSCALE = 3.75f;
        public const int CELL_SIZE = 32;
        private Board board;

        public LabyrinthGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = (int)(GAME_HEIGHT * GAME_UPSCALE);
            _graphics.PreferredBackBufferWidth = (int)(GAME_WIDTH * GAME_UPSCALE);
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            // TODO: Add your initialization logic here
            gameInput = new InputList();
            gameInput.AddBaseInputs();
            InputSystem.INSTANCE.SetInputList(gameInput);
            board = new(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("File");
            spriteFont2 = Content.Load<SpriteFont>("File2");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            board.Update(0.016f);
            base.Update(gameTime);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            InputSystem.INSTANCE.ParseAndSendInputs(dt);

            frame++;
            if (frame % Math.Pow(60, 3) == 0 && !hasWon)
            {
                hoursPassed++;
                minutesPassed = 0;
                secondsPassed = 0;
            }
            else if (frame % Math.Pow(60, 2) == 0 && !hasWon)
            {
                minutesPassed++;
                secondsPassed = 0;
            }
            else if (frame % Math.Pow(60, 1) == 0 && !hasWon)
            {
                secondsPassed++;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(20, 21, 46));
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Matrix matrix = Matrix.CreateScale(GAME_UPSCALE);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: matrix);

            board.Draw(_spriteBatch, Vector2.Zero);
            if (hasWon)
                _spriteBatch.DrawString(spriteFont2, "YOU WON WOOHOO!", new Vector2(100, 150), Color.White);
            else
                _spriteBatch.DrawString(spriteFont, text, new Vector2(35, 1), Color.White);
            if (hoursPassed == 0 && minutesPassed == 0)
            {
                _spriteBatch.DrawString(spriteFont, secondsPassed.ToString() + "s", new Vector2(480, 258), Color.White);
            }
            else if (hoursPassed == 0)
            {
                _spriteBatch.DrawString(spriteFont, minutesPassed.ToString() + "min " + secondsPassed.ToString() + "s", new Vector2(435, 258), Color.White);
            }
            else
            {
                _spriteBatch.DrawString(spriteFont, hoursPassed.ToString() + "hrs" + minutesPassed.ToString() + "min " + secondsPassed.ToString() + "s", new Vector2(390, 258), Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}