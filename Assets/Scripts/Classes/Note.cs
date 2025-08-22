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
        public override event Action<int> OnSofLanEvent;
        public override void Initialize(int laneNum, int noteType, int noteSoftLanding, float lifeSpan)
        {
            base.Initialize(laneNum,noteType,noteSoftLanding,lifeSpan);
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
        }
    }

}
