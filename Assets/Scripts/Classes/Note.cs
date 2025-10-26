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
        public override void Initialize(int laneNum, int noteType, int noteSoftLanding, float lifeSpan, int isCritical)
        {
            base.Initialize(laneNum, noteType, noteSoftLanding, lifeSpan, isCritical);
            Visible();
            base.AddTrigger(() => lifeSpan - TimeManager.instance.CurrentTime <= 0, OnSofLanEvent, noteSoftLanding);
            base.AddTrigger(() => base.lifeSpan + 1f / 60f * 13.5f - TimeManager.instance.CurrentTime < 0 && !isPushed, () => Judge());
        }
        public override void ManualUpdate(float BPM)
        {
            base.ManualUpdate(BPM);
            // 画面外に出たらクリアイベントを発火
            if (transform.position.y < -5f) // 画面外のY座標を適宜調整
            {
                base.Clear();
                Debug.Log("Note cleared.");
            }
        }
        public void Push()
        {
            isPushed = true;
            Invisible();
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
            Debug.Log($"{TimeManager.instance.CurrentTime} : Miss!!");
            Invisible();
            isPushed = true;
            JudgeDisplay(NotesJudgeState.Miss);
        }

    }

}
