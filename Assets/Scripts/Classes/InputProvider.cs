using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InputSystem
{
    public class InputProvider : IInputProvider
    {
        List<KeyCode> keyCodes = new List<KeyCode>()
        {
            KeyCode.A, // 1st lane
            KeyCode.S, // 2nd lane
            KeyCode.D, // 3rd lane
            KeyCode.K, // 4th lane
            KeyCode.L, // 5th lane
            KeyCode.Semicolon  // 6th lane
        };
        public List<bool> IsPressedAllLanes()
        {
            List<bool> pressedStates = keyCodes.Select(k => Input.GetKeyDown(k)).ToList();
            return pressedStates;
        }
        public List<bool> IsGetKeyAllLanes()
        {
            List<bool> pressedStates = keyCodes.Select(k => Input.GetKey(k)).ToList();
            return pressedStates;
        }
    }
}

