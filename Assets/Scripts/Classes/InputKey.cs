using System;
using System.Collections.Generic;
using Classes;
using UnityEngine;

namespace InputSystem
{
    public class InputKey
    {
        public KeyCode keyCode;
        public event Action action;
        protected List<ITriggerEntry> triggers = new();
        public InputKey(KeyCode keyCode)
        {
            this.keyCode = keyCode;
        }
        public void AddTrigger(Func<bool> condition, Action action)
        {
            triggers.Add(new TriggerEntry(condition, action));
        }
        public void ResetTriggers()
        {
            foreach (var trigger in triggers)
                trigger.Reset();
        }
        public void Fire()
        {
            Debug.Log($"Action for {keyCode} invoked.");
            foreach (var trigger in triggers)
                trigger.TryTrigger();
            action?.Invoke();
        }
    }
}
