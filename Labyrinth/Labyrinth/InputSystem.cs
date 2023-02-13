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
    public class InputSystem
    {
        private InputList list;
        public static Player player;

        private Keys[] prevKeys;

        private bool isHoldingBox;
        public void SetInputList(InputList _list)
        {
            list = _list;
        }

        public InputList GetInputList() => list;

        public void ParseAndSendInputs(float dt)
        {
            if (list == null) return;

            var keyboardState = Keyboard.GetState();

            foreach (var input in list.inputs)
            {
                string choosenInput = input.GetInput();

                if (input.input == "UP") choosenInput = "DOWN";
                else if (input.input == "DOWN") choosenInput = "UP";

                else
                {
                    foreach (var key in input.GetKeys())
                    {
                        if (keyboardState.IsKeyDown(key) && key == Keys.Space)
                        {
                            isHoldingBox = true;
                            Send(choosenInput, isHoldingBox);
                        }
                        else { isHoldingBox = false; }

                        if (prevKeys == null)
                        {
                            Send(choosenInput, isHoldingBox);
                        }
                        else if (prevKeys.Contains(key) == false)
                        {
                            Send(choosenInput, isHoldingBox);
                        }
                    }
                }

                foreach (var key in input.GetKeys())
                {
                    if (keyboardState.IsKeyDown(key) && prevKeys.Contains(key))
                    {
                        break;
                    }
                }
            }

            prevKeys = keyboardState.GetPressedKeys();
        }

        public void Send(string input, bool isHolding)
        {
            player.RecieveInput(input, isHolding);
        }
    }
}
