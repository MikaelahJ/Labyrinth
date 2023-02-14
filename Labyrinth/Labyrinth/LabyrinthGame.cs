using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

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

        private InputList gameInput;

        public const int GAME_WIDTH = 512;
        public const int GAME_HEIGHT = 288;
        public const int GAME_UPSCALE = 2;
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
            _graphics.PreferredBackBufferHeight = GAME_HEIGHT * GAME_UPSCALE;
            _graphics.PreferredBackBufferWidth = GAME_WIDTH * GAME_UPSCALE;
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

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}