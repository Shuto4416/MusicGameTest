using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InputSystem
{
    public class LatencyAdjustInputProvider : ILatencyAdjustInputProvider
    {
        public bool Space()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }
}

