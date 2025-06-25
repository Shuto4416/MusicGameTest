using System;
using System.Collections;
using System.Collections.Generic;

namespace Classes
{
    public class TriggerEntry
    {
        public bool HasFired { get; private set; } = false;
        public Func<bool> Condition;
        public Action Action;

        public TriggerEntry(Func<bool> condition, Action action)
        {
            Condition = condition;
            Action = action;
        }

        public void TryTrigger()
        {
            if(!HasFired && Condition())
            {
                HasFired = true;
                Action?.Invoke();
            }
        }
    }
}

