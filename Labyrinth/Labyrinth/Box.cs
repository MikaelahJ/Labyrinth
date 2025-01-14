﻿using System;
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
    public class Box : BoardObject
    {
        private Board board;

        public Box(int x, int y, ContentManager contentManager, Board board) : base(x, y)
        {
            sprite = contentManager.Load<Texture2D>("Box");
            this.board = board;
        }

        public override bool AttemptMove(int xMove, int yMove, int depth, bool isHolding = false)
        {
            if (!board.IsSpaceWalkable(x + xMove, y + yMove)) return false;
            if (board.IsPositionOutsideOfBoard(x + xMove, y + yMove)) return false;
            var searched = board.GetObjectAtPosition(x + xMove, y + yMove);
            if (searched != null)
            {
                if (searched.GetType() == typeof(Box) && !searched.AttemptMove(xMove, yMove, depth - 1, false))
                {
                    return false;
                }
            }

            DoMove(xMove, yMove);

            if (new Vector2(x, y) == board.goalPos)
                LabyrinthGame.hasWon = true;

            return true;
        }
    }
}
