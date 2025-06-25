using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using UnityEngine;

namespace Notes
{
    public abstract class BaseNote : MonoBehaviour
    {
        protected List<TriggerEntry> triggers = new();
        protected float lifeSpan;
        protected int laneNum;

        public float LifeSpan => lifeSpan;
        public int LaneNum => laneNum;

        public virtual void Initialize(float lifeSpan, int laneNum)
        {
            this.lifeSpan = lifeSpan;
            this.laneNum = laneNum;
            triggers.Clear();
        }
        protected virtual void Move(float BPM)
        {
            transform.position += Vector3.down * (BPM/60f) * Time.deltaTime;
        }
        public event Action OnClearEvent;

        protected virtual void Clear()
        {
            OnClearEvent?.Invoke();
        }

        public virtual void ManualUpdate(float BPM)
        {
            foreach (var trigger in triggers)
            {
                trigger.TryTrigger();
            }
            // ノーツの位置をBPMに基づいて更新
            Move(BPM);
        }

        protected void AddTrigger(Func<bool> condition, Action action)
        {
            triggers.Add(new TriggerEntry(condition, action));
        }
    }
}
