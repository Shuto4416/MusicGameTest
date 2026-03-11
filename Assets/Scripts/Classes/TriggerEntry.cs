using System;
using System.Collections;
using System.Collections.Generic;

namespace Classes
{
    public class TriggerEntry : ITriggerEntry
    {
        public bool isFired { get; private set; } = false;
        public Func<bool> condition;
        public Action action;

        public TriggerEntry(Func<bool> condition, Action action)
        {
            this.condition = condition;
            this.action = action;
        }

        public void TryTrigger()
        {
            if(!isFired && condition())
            {
                isFired = true;
                action?.Invoke();
            }
        }
        public void Reset() => isFired = false;
    }

    public class TriggerEntry<T> : ITriggerEntry
    {
        public bool isFired { get; private set; } = false;
        public Func<bool> condition;
        public Action<T> action;
        private readonly T _param;

        public TriggerEntry(Func<bool> condition, Action<T> action, T param)
        {
            this.condition = condition;
            this.action = action;
            this._param = param;
        }

        public void TryTrigger()
        {
            if (!isFired && condition())
            {
                isFired = true;
                action?.Invoke(_param);
            }
        }

        public void Reset() => isFired = false;
    }
}

