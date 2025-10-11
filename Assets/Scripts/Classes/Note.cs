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
        private bool isPushed = false;
        public override void Initialize(int laneNum, int noteType, int noteSoftLanding, float lifeSpan, int isCritical)
        {
            base.Initialize(laneNum, noteType, noteSoftLanding, lifeSpan, isCritical);
            isPushed = false;
            Visible();
            base.AddTrigger(() => lifeSpan - TimeManager.instance.CurrentTime < 0, OnSofLanEvent, noteSoftLanding);
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
            if (base.lifeSpan + 1f / 60f * 13.5f - TimeManager.instance.CurrentTime < 0)
            {
                if (!isPushed)
                {
                    Invisible();
                }
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

    }

}
