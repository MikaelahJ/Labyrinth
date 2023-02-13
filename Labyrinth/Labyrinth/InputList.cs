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
    public class InputList
    {

        public List<Input> inputs;

        const string UP = "UP";
        const string DOWN = "DOWN";
        const string LEFT = "LEFT";
        const string RIGHT = "RIGHT";
        const string SPACE = "SPACE";
        const string PAUSE = "PAUSE";

        public class Input
        {
            public List<Keys> keys;
            public string input;
            public bool repeatable;
            public float repeaterTime;
            private float timer;

            public Input(List<Keys> keys, string input, bool repeatable = false, float repeaterTime = 0.2f, float onHeldTimerReductionPerIncrement = 0.015f, int heldIncrementsLimit = 10)
            {
                this.input = input;
                this.keys = keys;
                this.repeatable = repeatable;
                this.repeaterTime = repeaterTime;
            }

            public bool AdvanceTimer(float time)
            {
                timer -= time;

                return true;
            }

           

            public List<Keys> GetKeys() => keys;
            public string GetInput() => input;
            public bool GetIsRepeatable() => repeatable;
            public float GetRepeaterTime() => repeaterTime;

            private void OnValidate()
            {
                input = input.ToUpper();
            }
        }

        public List<Input> GetAllInputs() => inputs;

        public void AddInput(Input input)
        {
            inputs ??= new List<Input>();

            if (inputs.Contains(input))
            {
                Console.WriteLine("key + input pair already exists");
                return;
            }
            foreach (var item in inputs)
            {
                if (item.GetKeys().SequenceEqual(input.GetKeys()))
                {
                    Console.WriteLine("Keys already exists");
                    return;
                }

            }

            inputs.Add(input);

        }

        public void AddInput(Keys key, string input, bool repeatable = false, float repeatTimer = 0.2f)
        {
            List<Keys> singleList = new() { key };
            AddInput(singleList, input, repeatable, repeatTimer);
        }

        public void AddInput(List<Keys> key, string input, bool repeatable = false, float repeatTimer = 0.2f)
        {
            input = input.ToUpper();

            inputs ??= new List<Input>();

            Input newInput = new(key, input, repeatable, repeatTimer);

            AddInput(newInput);
        }

        public void AddBaseInputs()
        {
            inputs ??= new List<Input>();

            Input baseLeft = new(new List<Keys> { Keys.Left, Keys.A }, LEFT, true, 0.2f, 0.015f, 10);
            if (inputs.Contains(baseLeft) == false) AddInput(baseLeft);

            Input baseRight = new(new List<Keys> { Keys.Right, Keys.D }, RIGHT, true, 0.2f, 0.015f, 10);
            if (inputs.Contains(baseRight) == false) AddInput(baseRight);

            Input baseUp = new(new List<Keys> { Keys.Up, Keys.W }, UP, true, 0.2f, 0.015f, 10);
            if (inputs.Contains(baseUp) == false) AddInput(baseUp);

            Input baseDown = new(new List<Keys> { Keys.Down, Keys.S }, DOWN, true, 0.2f, 0.015f, 10);
            if (inputs.Contains(baseDown) == false) AddInput(baseDown);

            Input baseSelect = new(new List<Keys> { Keys.Enter, Keys.Space }, SPACE, false);
            if (inputs.Contains(baseSelect) == false) AddInput(baseSelect);

            //Input baseBack = new Input(new List<Keys> { Keys.Z }, BACK, false);
            //if (inputs.Contains(baseBack) == false) AddInput(baseBack);

            Input basePause = new(new List<Keys> { Keys.Escape, Keys.P }, PAUSE, false);
            if (inputs.Contains(basePause) == false) AddInput(basePause);
        }
    }
}
