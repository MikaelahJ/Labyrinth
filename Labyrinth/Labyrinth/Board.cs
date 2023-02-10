using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;

namespace Labyrinth
{
    public class Board
    {
        private char[,] board;
        private ContentManager content;
        private List<BoardObject> objectsOnBoard;

        private Texture2D floorSprite;
        private Texture2D wallSprite;
        private Texture2D goalSprite;
        private Bitmap worldMap;

        public const char player = 'p';
        public const char floor = 'f';
        public const char wall = 'w';
        public const char box = 'b';
        public const char goal = 'g';

        // our custom board objects
        public const char cyan = 'c';
        public const char magenta = 'm';

        private Vector2 boardOffset;

        public Board(ContentManager _content)
        {
            content = _content;
            worldMap = new Bitmap("Main.png");
            floorSprite = content.Load<Texture2D>("Floor");
            wallSprite = content.Load<Texture2D>("Wall");
            goalSprite = content.Load<Texture2D>("Goal");
        }

        public int GetBoardWidth() => board.GetLength(0) - 1;
        public int GetBoardHeight() => board.GetLength(1) - 1;
        public Vector2 GetBoardOffset() => boardOffset;

        public bool IsSpaceWalkable(int x, int y)
        {
            if (IsPositionOutsideOfBoard(x, y)) return false;
            return board[x, y] != wall;
        }

        public void AddObject(char c, int x, int y)
        {
            var objOnSpot = GetObjectAtPosition(x, y);
            if (objOnSpot != null)
            {
                objectsOnBoard.Remove(objOnSpot);
            }

            switch (c)
            {
                case player:
                    objectsOnBoard.Add(new Player(x, y, content, this));
                    break;

                case box:
                    objectsOnBoard.Add(new Box(x, y, content, this));
                    break;
            }
        }

        public void RemoveObject(int x, int y)
        {
            var objToRemove = GetObjectAtPosition(x, y);
            if (objToRemove == null) return;

            objectsOnBoard.Remove(objToRemove);
        }

        public void SetGroundType(char type, int x, int y)
        {
            board[x, y] = type;
        }

        public bool IsPositionOutsideOfBoard(int x, int y)
        {
            if (x > board.GetLength(0) - 1 || x < 0) return true;
            if (y > board.GetLength(1) - 1 || y < 0) return true;
            return false;
        }

        public BoardObject GetObjectAtPosition(int x, int y)
        {
            if (IsPositionOutsideOfBoard(x, y)) return null;
            foreach (var item in objectsOnBoard)
            {
                if (item.GetX() == x && item.GetY() == y) return item;
            }

            return null;
        }

        private static char[,] RotateArrayClockwise(char[,] src)
        {
            int width;
            int height;
            char[,] dst;

            width = src.GetUpperBound(0) + 1;
            height = src.GetUpperBound(1) + 1;
            dst = new char[height, width];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int newRow;
                    int newCol;

                    newRow = col;
                    newCol = height - (row + 1);

                    dst[newCol, newRow] = src[col, row];
                }
            }

            return dst;
        }

        public void SetBoard(char[,] newBoard)
        {
            for (int i = 0; i < 3; i++)
            {
                newBoard = RotateArrayClockwise(newBoard);
            }

            objectsOnBoard = new List<BoardObject>();
            int width = newBoard.GetLength(0);
            int height = newBoard.GetLength(1);
            board = new char[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    board[x, y] = newBoard[x, height - 1 - y];

                    switch (board[x, y])
                    {
                        case player:
                            objectsOnBoard.Add(new Player(x, y, content, this));
                            board[x, y] = floor;
                            break;
                        case box:
                            objectsOnBoard.Add(new Box(x, y, content, this));
                            board[x, y] = floor;
                            break;

                    }
                }
            }

            boardOffset = new Vector2((board.GetLength(0) * 32) / 2, (board.GetLength(1) * 32) / 2);
        }


        public bool IsLevelFinished()
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] == goal)
                    {
                        var objAtPosition = GetObjectAtPosition(x, y);
                        if (objAtPosition == null)
                        {
                            return false;
                        }
                        else if (objAtPosition.GetType() != typeof(Box))
                        {
                            return false;
                        }
                    }
                }

            }
            return true;
        }

        public void Draw(SpriteBatch batch, Vector2 offset)
        {
            if (board == null) return;
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    Vector2 position = new Vector2(x * 32, y * 32);
                    position += offset;

                    switch (board[x, y])
                    {
                        case floor:
                            batch.Draw(floorSprite, position, Microsoft.Xna.Framework.Color.White);
                            break;
                        case wall:
                            batch.Draw(wallSprite, position, Microsoft.Xna.Framework.Color.White);
                            break;
                        case goal:
                            batch.Draw(goalSprite, position, Microsoft.Xna.Framework.Color.White);
                            break;
                    }
                }
            }
        }

        public bool GetIsLevelBound()
        {
            return false;
        }
    }
}
