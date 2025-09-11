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
        private float secondLifeSpan;
        public float SecondLifeSpan => secondLifeSpan;
        public override event Action<int> OnSofLanEvent;
        public void Initialize(int laneNum, int noteType, int firstNoteSoftLanding, float firstLifeSpan, int secondNoteSoftLanding, float secondLifeSpan, Vector3 endPoint)
        {
            base.Initialize(laneNum, noteType, firstNoteSoftLanding, firstLifeSpan);
            Visible();
            LineRendererEndPoint = endPoint;
            this.secondLifeSpan = secondLifeSpan;
            lineRenderer.SetPosition(1, new Vector3(0, endPoint.y, 0));
            base.AddTrigger<int>(() => firstLifeSpan - TimeManager.instance.CurrentTime < 0, OnSofLanEvent, firstNoteSoftLanding);
            base.AddTrigger<int>(() => secondLifeSpan - TimeManager.instance.CurrentTime < 0, OnSofLanEvent, secondNoteSoftLanding);
        }


        public override void ManualUpdate(float BPM)
        {
            if (base.lifeSpan + 1 / 60 * 13.5 - TimeManager.instance.CurrentTime < 0)
            {
                if (isPushed && pushFrameCount > 0)
                {
                    pushFrameCount--;
                }
                else Invisible();
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
            this.enabled = false;
        }

        public void Visible()
        {
            this.enabled = true;
        }

        public void Press()
        {
            pushFrameCount = 2;
        }
        public void Push()
        {
            isPushed = true;
        }

    }

}
