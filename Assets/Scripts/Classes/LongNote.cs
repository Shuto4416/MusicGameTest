using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using TheSingleton;


namespace Notes {
    public class LongNote : BaseNote
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private LineRenderer lineRenderer;
        private Vector3 LineRendererEndPoint;
        private bool isPushed = false;
        private int pushFrameCount = 0;
        private float _secondLifeSpan;
        public float SecondLifeSpan => _secondLifeSpan;
        public override event Action<int> OnSofLanEvent;
        public void Initialize(int laneNum, int noteType, int firstNoteSoftLanding, float firstLifeSpan, int secondNoteSoftLanding, float secondLifeSpan, Vector3 endPoint)
        {
            _secondLifeSpan = secondLifeSpan;
            base.Initialize(laneNum, noteType, firstNoteSoftLanding, firstLifeSpan);
            isPushed = false;
            LineRendererEndPoint = endPoint;
            Debug.Log($"firstLifeSpan: {firstLifeSpan}, secondLifeSpan: {_secondLifeSpan}");
            Visible();
            base.AddTrigger<int>(() => firstLifeSpan - TimeManager.instance.CurrentTime < 0, OnSofLanEvent, firstNoteSoftLanding);
            base.AddTrigger<int>(() => _secondLifeSpan - TimeManager.instance.CurrentTime < 0, OnSofLanEvent, secondNoteSoftLanding);
            base.AddTrigger(() => _secondLifeSpan - TimeManager.instance.CurrentTime < 0, () => Judge());
        }


        public override void ManualUpdate(float BPM)
        {
            if (base.lifeSpan + 1f / 60f * 13.5f - TimeManager.instance.CurrentTime < 0 && !isPushed)
            {
                Debug.Log("Miss2");
                Invisible();
            }
            if (isPushed && _secondLifeSpan - TimeManager.instance.CurrentTime > 0)
            {
                if (pushFrameCount > 0)
                {
                    pushFrameCount--;
                }
                else
                {
                    Invisible();
                    Debug.Log("Miss");
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
        private void Judge()
        {
            Debug.Log("Judge");
            if (isPushed && pushFrameCount > 0)
            {
                Debug.Log($"isPushed: {isPushed}, pushFrameCount: {pushFrameCount}");
                Invisible();
                Debug.Log("P");
            } else
            {
                Debug.Log($"isPushed: {isPushed}, pushFrameCount: {pushFrameCount}");
                Debug.Log("Miss3");
                Invisible();
            }
        }

    }

}
