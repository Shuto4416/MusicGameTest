using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{
    public class DebugSelectInputProvider : ISelectInputProvider
    {
        public bool DifficultyUp()
        {
            return Input.GetKeyDown(KeyCode.E);
        }

        public bool DifficultyDown()
        {
            return Input.GetKeyDown(KeyCode.Q);
        }

        public bool Up()
        {
            return Input.GetKeyDown(KeyCode.W);
        }

        public bool Down()
        {
            return Input.GetKeyDown(KeyCode.S);
        }

        public bool Right()
        {
            return Input.GetKeyDown(KeyCode.A);
        }

        public bool Left()
        {
            return Input.GetKeyDown(KeyCode.D);
        }

        public bool Enter()
        {
            return Input.GetKeyDown(KeyCode.Return);
        }

        public void ManualUpdate()
        {
            return;
        }
    }
}

