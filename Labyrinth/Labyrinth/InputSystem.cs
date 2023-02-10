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

        private Keys[] prevKeys;

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
                        if (keyboardState.IsKeyDown(key))
                        {
                            if (prevKeys == null)
                            {
                                //Send(choosenInput);
                            }
                            else if (prevKeys.Contains(key) == false)
                            {
                                //Send(choosenInput);
                            }
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

        //public void Send(string input)
        //{
        //    SendFinalInput(input);

        //    if (firstHandler == null)
        //    {
        //        SendFinalInput(input);
        //    }
        //    else
        //    {
        //        SendFinalInput(firstHandler.RecieveInput(input));
        //    }
        //}

        //public void AddHandler(InputHandler newHandler)
        //{
        //    Debug.Log("adding input handler: " + newHandler);
        //    if (firstHandler == null) firstHandler = newHandler;

        //    else if (IsInputHandlerPresentInChain(newHandler, firstHandler))
        //    {
        //        Debug.LogWarning("handler: " + newHandler + " already present in chain");
        //    }

        //    else GetLastHandler().SetNextHandler(newHandler);
        //}

        //public void RemoveHandler(InputHandler remove)
        //{

        //    if (firstHandler == null) return;

        //    if (firstHandler == remove)
        //    {
        //        firstHandler = firstHandler.GetNextHandler();
        //        return;
        //    }
        //    if (IsInputHandlerPresentInChain(remove, firstHandler) == false) return;

        //    InputHandler parent = GetParentHandlerFor(remove, firstHandler);

        //    // we're not removing The Last handler
        //    if (GetLastHandler() != remove)
        //    {
        //        InputHandler child = remove.GetNextHandler();

        //        parent.SetNextHandler(child);
        //    }
        //    else
        //    {
        //        parent.SetNextHandler(null);
        //    }

        //}

        //private InputHandler GetParentHandlerFor(InputHandler h, InputHandler evaluatedParent)
        //{
        //    if (firstHandler == h) return h;

        //    if (evaluatedParent.GetNextHandler() == h) return evaluatedParent;

        //    return GetParentHandlerFor(h, evaluatedParent.GetNextHandler());
        //}

        //private bool IsInputHandlerPresentInChain(InputHandler h, InputHandler evaluatedParent)
        //{
        //    if (evaluatedParent.GetNextHandler() == null) return false;
        //    if (firstHandler == null) return false;
        //    if (firstHandler == h) return true;

        //    if (evaluatedParent.GetNextHandler() == h) return true;

        //    return IsInputHandlerPresentInChain(h, evaluatedParent.GetNextHandler());
        //}

        //public InputHandler GetLastHandler()
        //{
        //    return firstHandler.GetLastHandler();
        //}

        //private void SendFinalInput(string input)
        //{
        //    if (objectsToService == null || objectsToService.Count == 0) return;

        //    foreach (var obj in objectsToService)
        //    {
        //        obj.RecieveInput(input);
        //    }
        //}
    }
}
