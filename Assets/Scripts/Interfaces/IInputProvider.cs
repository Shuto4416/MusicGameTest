using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InputSystem
{
    public interface IInputProvider
    {
        public int KeyCount();
        public void ManualUpdate();
        public void Initialize();
        public List<bool> IsPushAllLanes();
        public void SubscriptForKeyDownAction(int lane, params Action[] action);
        public void SubscriptForKeyDownOnceAction(int lane, params Action[] action);
    }
}

