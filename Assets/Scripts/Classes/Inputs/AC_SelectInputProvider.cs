using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{
    public class AC_SelectInputProvider : ISelectInputProvider
    {
        public bool DifficultyUp()
        {
            return Input.GetKeyDown(KeyCode.D);
        }

        public bool DifficultyDown()
        {
            return Input.GetKeyDown(KeyCode.S);
        }

        public bool Up()
        {
            return Input.GetKeyDown(KeyCode.F);
        }

        public bool Down()
        {
            return Input.GetKeyDown(KeyCode.W);
        }

        public bool Right()
        {
            return Input.GetKeyDown(KeyCode.K);
        }

        public bool Left()
        {
            return Input.GetKeyDown(KeyCode.J);
        }

        public bool Enter()
        {
            return Input.GetKeyDown(KeyCode.L);
        }

        public void ManualUpdate()
        {
            return;
        }
    }
}

