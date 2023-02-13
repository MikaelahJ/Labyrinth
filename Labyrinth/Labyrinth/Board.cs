using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SharpDX.Direct3D9;
using SharpDX.MediaFoundation;

namespace Labyrinth
{
    public class Board
    {
        private int cellSize = 32;
        private int worldWidth = 16;
        private int worldHeight = 9;
        private char[,] board;

        char[,] mapArray = new char[16, 9];

        private ContentManager content;
        private List<BoardObject> objectsOnBoard;

        private Texture2D floorSprite;
        private Texture2D wallSprite;
        private Texture2D goalSprite;
        private Bitmap mainMap;
        private Bitmap layer1;
        private Bitmap layer2;

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
            mainMap = GetBitmap("Main");
            layer1 = GetBitmap("Layer1");
            layer2 = GetBitmap("Layer2");

            //var test = GetBitmap("Test");

            BitmapToArray(mainMap, 'w');
            BitmapToArray(layer1, 'c');
            BitmapToArray(layer2, 'm');

            mapArray[12, 7] = 'g';

            SetBoard(mapArray);
            
            floorSprite = content.Load<Texture2D>("Floor");
            wallSprite = content.Load<Texture2D>("Wall");
            goalSprite = content.Load<Texture2D>("Goal");
        }

        private Bitmap GetBitmap(string textureName)
        {
            MemoryStream memoryStream = new MemoryStream();
            Texture2D texture = content.Load<Texture2D>(textureName);
            texture.SaveAsPng(memoryStream, texture.Width, texture.Height);

            return new Bitmap(memoryStream);
        }

        private void BitmapToArray(Bitmap map, char type)
        {
            int width = map.Width;
            int height = map.Height;
            char[,] result = new char[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (mapArray[x, y] != 0 && mapArray[x, y] != 'f') continue;

                    mapArray[x, y] = map.GetPixel(x, y).R < 200 ? type : 'f';

                    Debug.WriteLine(mapArray[x, y]);
                }
            }
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
            if (x > worldWidth - 1 || x < 0) return true;
            if (y > worldHeight - 1 || y < 0) return true;
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
            objectsOnBoard = new List<BoardObject>();
            int width = worldWidth;
            int height = worldHeight;
            board = new char[width, height];

            objectsOnBoard.Add(new Player(0, 0, content, this));
            objectsOnBoard.Add(new Box(0, 2, content, this));
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


        public void Update(float dt)
        {
            foreach (var obj in objectsOnBoard)
            {
                obj.Update(dt);
            }
        }

        public void Draw(SpriteBatch batch, Vector2 offset)
        {
            if (mapArray == null) return;
            for (int x = 0; x < worldWidth; x++)
            {
                for (int y = 0; y < worldHeight; y++)
                {
                    Vector2 position = new Vector2(x * 32, y * 32);

                    switch (mapArray[x, y])
                    {
                        case floor:
                            batch.Draw(floorSprite, position, new Microsoft.Xna.Framework.Color(20, 21, 46));
                            break;
                        case wall:
                            batch.Draw(wallSprite, position, new Microsoft.Xna.Framework.Color(2, 56, 46));
                            break;
                        case cyan:
                            batch.Draw(wallSprite, position, new Microsoft.Xna.Framework.Color(0, 247, 255));
                            break;
                        case magenta:
                            batch.Draw(wallSprite, position, new Microsoft.Xna.Framework.Color(247, 0, 255));
                            break;
                        case goal:
                            batch.Draw(goalSprite, position, Microsoft.Xna.Framework.Color.White);
                            break;
                    }
                }
            }

            foreach (var obj in objectsOnBoard)
            {
                obj.Draw(batch, offset);
            }

        }

        public bool GetIsLevelBound()
        {
            return false;
        }
    }
}
