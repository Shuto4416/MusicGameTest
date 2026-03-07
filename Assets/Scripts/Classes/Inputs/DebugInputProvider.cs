using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using UnityEngine;

namespace InputSystem
{
    public class DebugInputProvider : IInputProvider
    {
        List<KeyCode> keyCodes = new List<KeyCode>()
        {
            KeyCode.S, // 1st lane
            KeyCode.D, // 2nd lane
            KeyCode.F, // 3rd lane
            KeyCode.J, // 4th lane
            KeyCode.K, // 5th lane
            KeyCode.L  // 6th lane
        };
        List<InputKey> InputKeys = new List<InputKey>(Enumerable.Repeat<InputKey>(null, 6));

        public int KeyCount() => keyCodes.Count;

        public void ManualUpdate()
        {
            GetKeyDownActionAllLanes();
        }

        public List<bool> IsPushAllLanes()
        {
            return InputKeys.Select(inputKey => Input.GetKey(inputKey.keyCode)).ToList();
        }

        public void Initialize()
        {
            InputKeys = keyCodes.Select(k => new InputKey(k)).ToList();
        }
        private void GetKeyDownActionAllLanes()
        {
            foreach (var inputKey in InputKeys)
            {
                if (Input.GetKey(inputKey.keyCode))
                {
                    Debug.Log($"Key {inputKey.keyCode} pressed");
                    inputKey.Fire();
                }
                else inputKey.ResetTriggers();
            }
        }

        public void SubscriptForKeyDownAction(int lane, params Action[] actions)
        {
            foreach (var action in actions)
            {
                InputKeys[lane].action += action;
            }
        }

        public void SubscriptForKeyDownOnceAction(int lane, params Action[] actions)
        {
            foreach (var action in actions)
            {
                InputKeys[lane].AddTrigger(()=>true, action);
            }
        }
    }
}

