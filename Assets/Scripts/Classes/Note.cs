using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using TheSingleton;


namespace Notes {
    public class Note : BaseNote
    {
        [SerializeField] private Renderer _renderer;
        public override event Action<int> OnSofLanEvent;
        public override event Action<NotesJudgeState> JudgeDisplayEvent;
        public override void Initialize(int laneNum, int noteType, int noteSoftLanding, float lifeSpan, int isCritical, Vector3 initialPosition/*, float spawnAbsTime, float targetAbsTime*/)
        {
            var time = TimeManager.instance.CurrentTime;
            base.Initialize(laneNum, noteType, noteSoftLanding, lifeSpan, isCritical, initialPosition/*, spawnAbsTime, targetAbsTime*/);
            Visible();
            base.AddTrigger(() => lifeSpan - time <= 0, OnSofLanEvent, noteSoftLanding);
            base.AddTrigger(() => base.lifeSpan + 1f / 60f * 13.5f - TimeManager.instance.CurrentTime < 0 && !isPushed, () => Judge());
            base.AddTrigger(() => base.lifeSpan + 1f / 60f * 14f - TimeManager.instance.CurrentTime < 0, () => base.Clear());
            // base.AddTrigger(() => TimeManager.instance.CurrentTime > lifeSpan + 0.5f, () => base.Clear());
        }
        public override void ManualUpdate(float BPM)
        {
            base.ManualUpdate(BPM);
            // 画面外に出たらクリアイベントを発火
            // if (transform.position.y < -5f) // 画面外のY座標を適宜調整
            // {
            //     base.Clear();
            //     Debug.Log("Note cleared.");
            // }
        }
        public void Push()
        {
            isPushed = true;
            Invisible();
            // base.Clear();
        }

        public void Invisible()
        {
            _renderer.enabled = false;
        }

        public void Visible()
        {
            _renderer.enabled = true;
        }

        public void JudgeDisplay(NotesJudgeState state)
        {
            JudgeDisplayEvent?.Invoke(state);
        }

        private void Judge()
        {
            var time = TimeManager.instance.CurrentTime;
            Debug.Log($"{time} : Miss!!");
            Invisible();
            isPushed = true;
            JudgeDisplay(NotesJudgeState.Miss);
        }

    }

}
