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
        protected int laneNum;
        protected int noteType;
        protected int noteSoftLanding;
        protected float lifeSpan;

        public int LaneNum => laneNum;
        public int NoteType => noteType;
        public int NoteSoftLanding => noteSoftLanding;
        public float LifeSpan => lifeSpan;


        public virtual void Initialize(int laneNum, int noteType, int noteSoftLanding, float lifeSpan)
        {
            this.laneNum = laneNum;
            this.noteType = noteType;
            this.noteSoftLanding = noteSoftLanding;
            this.lifeSpan = lifeSpan;
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
