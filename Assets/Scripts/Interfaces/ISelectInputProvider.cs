using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InputSystem
{
    public interface ISelectInputProvider
    {
        public bool DifficultyUp();
        public bool DifficultyDown();
        public bool Up();
        public bool Down();
        public bool Right();
        public bool Left();
        public bool Enter();
        public void ManualUpdate();
    }
}

