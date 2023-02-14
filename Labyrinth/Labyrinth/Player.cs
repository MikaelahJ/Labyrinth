using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Labyrinth
{
    public class Player : BoardObject
    {
        private Board board;
        public Player(int x, int y, ContentManager contentManager, Board board) : base(x, y)
        {
            sprite = contentManager.Load<Texture2D>("Char");
            drawPosition = new Vector2(0 * 32, 0 * 32);
            this.board = board;
            InputSystem.player = this;
        }

        public void RecieveInput(string input, bool isHoldingBox)
        {
            switch (input)
            {
                case "UP":
                    AttemptMove(0, 1, 0, isHoldingBox);
                    break;

                case "DOWN":
                    AttemptMove(0, -1, 0, isHoldingBox);
                    break;

                case "LEFT":
                    AttemptMove(-1, 0, 0, isHoldingBox);
                    break;

                case "RIGHT":
                    AttemptMove(1, 0, 0, isHoldingBox);
                    break;
                case "SWITCH":
                    if(CheckSquares())
                        board.cyanActive = !board.cyanActive;
                    break;
            }
        }

        private bool CheckSquares()
        {
            for (int _x = 0; _x < board.worldWidth; _x++)
            {
                for (int _y = 0; _y < board.worldHeight; _y++)
                {
                    if (board.mapArray[_x, _y] == 'c' || board.mapArray[_x, _y] == 'm')
                    {
                        BoardObject searched = board.GetObjectAtPosition(_x, _y);

                        if (searched is Box box || searched is Player player)
                            return false;
                    }

                }
            }
            return true;
        }

        public override bool AttemptMove(int xMove, int yMove, int depth, bool isHolding)
        {
            if (board.IsSpaceWalkable(x + xMove, y + yMove) == false) return false;
            if (board.IsPositionOutsideOfBoard(x + xMove, y + yMove)) return false;

            BoardObject searched = board.GetObjectAtPosition(x + xMove, y + yMove);
            if (searched != null)
            {
                if (searched is Box box)
                {
                    if (box.AttemptMove(xMove, yMove, depth) == false)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            if (isHolding)
            {
                searched = board.GetObjectAtPosition(x - xMove, y - yMove);

                if (searched != null)
                {
                    Debug.WriteLine("den är ju fan inte null då asså");
                    if (searched is Box box)
                    {
                        box.AttemptMove(xMove, yMove, depth);
                    }
                }
            }

            DoMove(xMove, yMove);

            return true;
        }
    }
}
