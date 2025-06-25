using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;


namespace Notes {
    public class Note : BaseNote
    {
        public override void Initialize(float lifeSpan, int laneNum)
        {
            base.Initialize(lifeSpan, laneNum);
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
