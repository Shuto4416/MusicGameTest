using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{
    public interface IInputProvider
    {
        public bool IsPressedFirstLane();
        public bool IsPressedSecondLane();
        public bool IsPressedThirdLane();
        public bool IsPressedFourthLane();
        public bool IsPressedFifthLane();
        public bool IsPressedSixthLane();
        public bool[] IsPressedAllLanes();
    }
}

