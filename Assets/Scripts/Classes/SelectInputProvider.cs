using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{
    public class SelectInputProvider : ISelectInputProvider
    {
        public bool DifficultyUp()
        {
            return Input.GetKeyDown(KeyCode.S);
        }

        public bool DifficultyDown()
        {
            return Input.GetKeyDown(KeyCode.A);
        }

        public bool Up()
        {
            return Input.GetKeyDown(KeyCode.D);
        }

        public bool Down()
        {
            return Input.GetKeyDown(KeyCode.W);
        }

        public bool Right()
        {
            return Input.GetKeyDown(KeyCode.L);
        }

        public bool Left()
        {
            return Input.GetKeyDown(KeyCode.K);
        }

        public bool Enter()
        {
            return Input.GetKeyDown(KeyCode.Semicolon);
        }

        public void ManualUpdate()
        {
            return;
        }
    }
}

