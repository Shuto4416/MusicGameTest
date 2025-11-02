using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using UnityEngine;

namespace Notes
{
    public abstract class BaseNote : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameObject;
        protected List<ITriggerEntry> triggers = new();
        protected int laneNum;
        protected int noteType;
        protected int noteSoftLanding;
        protected float lifeSpan = 1000000f;
        protected int isCritical;
        protected bool isPushed = false;
        protected Vector3 initialPosition = new Vector3(0,100f,0f);

        public int LaneNum => laneNum;
        public int NoteType => noteType;
        public int NoteSoftLanding => noteSoftLanding;
        public float LifeSpan => lifeSpan;
        public int IsCritical => isCritical;

        public bool IsPushed => isPushed;
        public Vector3 InitialPosition => initialPosition;
        public static float JudgeLineY = 0f;
        protected float spawnAbsTime;
        protected float targetAbsTime;


        public virtual void Initialize(int laneNum, int noteType, int noteSoftLanding, float lifeSpan, int isCritical, Vector3 initialPosition/*, float spawnAbsTime, float targetAbsTime*/)
        {
            this.laneNum = laneNum;
            this.noteType = noteType;
            this.noteSoftLanding = noteSoftLanding;
            this.lifeSpan = lifeSpan;
            this.isCritical = isCritical;
            this.initialPosition = initialPosition;
            // this.spawnAbsTime = spawnAbsTime;
            // this.targetAbsTime = targetAbsTime;
            // transform.position = initialPosition;
            isPushed = false;
            triggers.Clear();
        }
        protected virtual void Move(float speed)
        {
            gameObject.transform.position = initialPosition + Vector3.down * speed;
        }
        // protected virtual void Move(float BPM)
        // {
        //     float currentAbsTime = TimeManager.instance.CurrentTime;
            
        //     // --- 絶対位置計算ロジックを挿入 ---
        //     float totalTime = targetAbsTime - spawnAbsTime;
        //     float softLandingRatio = noteSoftLanding / 100f;
        //     float totalDistanceY = BPM/60f * totalTime * softLandingRatio;
        //     float startLineY = totalDistanceY + JudgeLineY;

        //     float currentY;
        //     if (totalTime > 0)
        //     {
        //         float progress = (currentAbsTime - spawnAbsTime) / totalTime;
        //         currentY = Mathf.Lerp(startLineY, JudgeLineY, progress);
        //     }
        //     else
        //     {
        //         currentY = JudgeLineY;
        //     }
            
        //     transform.position = new Vector3(initialPosition.x, currentY, initialPosition.z);
        // }
        public event Action OnClearEvent;
        public virtual event Action<int> OnSofLanEvent;
        public virtual event Action<NotesJudgeState> JudgeDisplayEvent;
        
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
