using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Labyrinth
{
    public abstract class BoardObject
    {
        protected Vector2 drawPosition;
        private Vector2 truePosition;
        private float moveTimer;
        protected Texture2D sprite;
        protected int x;
        protected int y;
        protected int speed = 2;

        public BoardObject(int x, int y)
        {
            this.x = x;
            this.y = y;
            drawPosition = new Vector2(x * LabyrinthGame.CELL_SIZE, y * LabyrinthGame.CELL_SIZE);
            truePosition = drawPosition;
        }

        public int GetX() => x;
        public int GetY() => y;

        protected void DoMove(int xMove, int yMove)
        {
            x += xMove;
            y += yMove;

            moveTimer = 0;
            truePosition = new Vector2(x * LabyrinthGame.CELL_SIZE, y * LabyrinthGame.CELL_SIZE);
        }

        public void Update(float dt)
        {
            moveTimer += dt * speed;
            moveTimer = moveTimer > 1 ? 1 : moveTimer;
            drawPosition = Vector2.Lerp(drawPosition, truePosition, moveTimer);
        }

        public abstract bool AttemptMove(int xMove, int yMove, int depth);

        public virtual void Draw(SpriteBatch batch, Vector2 offset)
        {
            batch.Draw(sprite, drawPosition + offset, Color.White);
        }
    }
}
