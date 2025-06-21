using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{
    public class InputProvider : IInputProvider
    {
        public bool IsPressedFirstLane()
        {
            return Input.GetKey(KeyCode.A);
        }

        public bool IsPressedSecondLane()
        {
            return Input.GetKey(KeyCode.S);
        }

        public bool IsPressedThirdLane()
        {
            return Input.GetKey(KeyCode.D);
        }

        public bool IsPressedFourthLane()
        {
            return Input.GetKey(KeyCode.K);
        }

        public bool IsPressedFifthLane()
        {
            return Input.GetKey(KeyCode.L);
        }

        public bool IsPressedSixthLane()
        {
            return Input.GetKey(KeyCode.Semicolon);
        }
        public bool[] IsPressedAllLanes()
        {
            return new bool[]
            {
                IsPressedFirstLane(),
                IsPressedSecondLane(),
                IsPressedThirdLane(),
                IsPressedFourthLane(),
                IsPressedFifthLane(),
                IsPressedSixthLane()
            };
        }
    }
}

