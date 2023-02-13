using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Labyrinth
{
    public class LabyrinthGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public const int WIDTH = 512;
        public const int HEIGHT = 288;
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
            // TODO: Add your initialization logic here
            board = new(Content);

            base.Initialize();
            board = new(Content);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Matrix matrix = Matrix.CreateScale(GAME_UPSCALE);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: matrix);

            // TODO: Add your drawing code here

            _spriteBatch.End();
            base.Draw(gameTime);
            
        }
    }
}