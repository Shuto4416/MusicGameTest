using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using UnityEngine;

namespace Notes
{
    public abstract class BaseNote : MonoBehaviour
    {
        protected List<ITriggerEntry> triggers = new();
        protected int laneNum;
        protected int noteType;
        protected int noteSoftLanding;
        protected float lifeSpan;
        protected int isCritical;

        public int LaneNum => laneNum;
        public int NoteType => noteType;
        public int NoteSoftLanding => noteSoftLanding;
        public float LifeSpan => lifeSpan;
        public int IsCritical => isCritical;


        public virtual void Initialize(int laneNum, int noteType, int noteSoftLanding, float lifeSpan, int isCritical)
        {
            this.laneNum = laneNum;
            this.noteType = noteType;
            this.noteSoftLanding = noteSoftLanding;
            this.lifeSpan = lifeSpan;
            this.isCritical = isCritical;
            triggers.Clear();
        }
        protected virtual void Move(float BPM)
        {
            transform.position += Vector3.down * (BPM/60f) * Time.deltaTime;
        }
        public event Action OnClearEvent;
        public virtual event Action<int> OnSofLanEvent;
        
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

        protected void AddTrigger<T>(Func<bool> condition, Action<T> action, T param)
        {
            triggers.Add(new TriggerEntry<T>(condition, action, param));
        }
    }
}
