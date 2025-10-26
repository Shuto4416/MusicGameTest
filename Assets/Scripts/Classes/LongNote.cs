using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using TheSingleton;


namespace Notes {
    public class LongNote : BaseNote
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private LineRenderer lineRenderer;
        private Vector3 LineRendererEndPoint;
        private bool isJudged = false;
        private bool isBad = false;
        private int pushFrameCount = 0;
        private float _secondLifeSpan;
        public float SecondLifeSpan => _secondLifeSpan;
        public override event Action<int> OnSofLanEvent;
        public override event Action<NotesJudgeState> JudgeDisplayEvent;

        public void Initialize(int laneNum, int noteType, int firstNoteSoftLanding, float firstLifeSpan, int secondNoteSoftLanding, float secondLifeSpan, int isCritical, Vector3 endPoint)
        {
            _secondLifeSpan = secondLifeSpan;
            base.Initialize(laneNum, noteType, firstNoteSoftLanding, firstLifeSpan, isCritical);
            isBad = false;
            isJudged = false;
            LineRendererEndPoint = endPoint;
            Debug.Log($"firstLifeSpan: {firstLifeSpan}, secondLifeSpan: {_secondLifeSpan}");
            Visible();
            base.AddTrigger<int>(() => firstLifeSpan - TimeManager.instance.CurrentTime <= 0, OnSofLanEvent, firstNoteSoftLanding);
            base.AddTrigger<int>(() => _secondLifeSpan - TimeManager.instance.CurrentTime <= 0, OnSofLanEvent, secondNoteSoftLanding);
            base.AddTrigger(() => _secondLifeSpan - TimeManager.instance.CurrentTime <= 0 && !isJudged, () => Judge());
        }


        public override void ManualUpdate(float BPM)
        {
            if (!isJudged)
            {
                if (base.lifeSpan + 1f / 60f * 13.5f - TimeManager.instance.CurrentTime < 0 && !isPushed && _renderer.enabled)
                {
                    JudgeDisplay(NotesJudgeState.Miss);
                    Debug.Log("Miss2");
                    isJudged = true;
                    Invisible();
                }
                if (isPushed && _secondLifeSpan - TimeManager.instance.CurrentTime > 0 && !isBad)
                {
                    if (pushFrameCount > 0)
                    {
                        pushFrameCount--;
                    }
                    else if (_renderer.enabled)
                    {
                        JudgeDisplayEvent?.Invoke(NotesJudgeState.Miss);
                        isJudged = true;
                        Invisible();
                        Debug.Log("Miss");
                    }
                }
                if (isBad && _renderer.enabled)
                {
                    isJudged = true;
                    JudgeDisplayEvent?.Invoke(NotesJudgeState.Miss);
                }
            }
            base.ManualUpdate(BPM);
            // 画面外に出たらクリアイベントを発火
            if (transform.position.y + LineRendererEndPoint.y < -5f) // 画面外のY座標を適宜調整
            {
                base.Clear();
                Debug.Log("Note cleared.");
            }
        }

        public void Invisible()
        {
            _renderer.enabled = false;
            lineRenderer.SetPosition(1, new Vector3(0, 0, 0));
        }

        public void Visible()
        {
            _renderer.enabled = true;
            lineRenderer.SetPosition(1, new Vector3(0, LineRendererEndPoint.y, 0));
        }

        public void Press()
        {
            Debug.Log("Pressed");
            pushFrameCount += 2;
        }
        public void Push()
        {
            Debug.Log("Pushed");
            isPushed = true;
            pushFrameCount = 5;
        }
        public void BadPush()
        {
            JudgeDisplayEvent?.Invoke(NotesJudgeState.Bad);
            Debug.Log("BadPushed");
            isBad = true;
            isPushed = true;
        }
        private void Judge()
        {
            isJudged = true;
            Debug.Log("Judge");
            if (isPushed && pushFrameCount > 0)
            {
                Debug.Log($"isPushed: {isPushed}, pushFrameCount: {pushFrameCount}");
                JudgeDisplayEvent?.Invoke(NotesJudgeState.Perfect);
                Invisible();
                Debug.Log("P");
            }
            else
            {
                Debug.Log($"isPushed: {isPushed}, pushFrameCount: {pushFrameCount}");
                JudgeDisplayEvent?.Invoke(NotesJudgeState.Miss);
                Debug.Log("Miss3");
                Invisible();
            }
        }
        public void JudgeDisplay(NotesJudgeState state)
        {
            JudgeDisplayEvent?.Invoke(state);
        }

    }

}
