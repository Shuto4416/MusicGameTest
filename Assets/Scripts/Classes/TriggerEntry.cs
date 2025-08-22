using System;
using System.Collections;
using System.Collections.Generic;

namespace Classes
{
    public class TriggerEntry : ITriggerEntry
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
        public void Reset() => HasFired = false;
    }

    public class TriggerEntry<T> : ITriggerEntry
    {
        public bool HasFired { get; private set; } = false;
        public Func<bool> Condition;
        public Action<T> Action;
        private readonly T _param;

        public TriggerEntry(Func<bool> condition, Action<T> action, T param)
        {
            Condition = condition;
            Action = action;
            _param = param;
        }

        public void TryTrigger()
        {
            if (!HasFired && Condition())
            {
                HasFired = true;
                Action?.Invoke(_param);
            }
        }

        public void Reset() => HasFired = false;
    }
}

