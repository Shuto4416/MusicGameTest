using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{
    public interface IInputProvider
    {
        public List<bool> IsPressedAllLanes();
        public List<bool> IsGetKeyAllLanes();
    }
}

