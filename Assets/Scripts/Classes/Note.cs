using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;


namespace Notes {
    public class Note : MonoBehaviour, INote
    {
        [SerializeField]
        private GameObject _notePrefab; // ノーツのプレハブ
        public int Type;  // ノーツの種類（通常ノーツ・ロングノーツなど）
        public int BeatNum;   // 何拍目に配置されるか
        public int LaneNum; // どのレーンに配置されるか
        public int LPB;   // 1拍あたりの分割数
        public void Move(float BPM)
        {
            Debug.Log("Move");
            transform.position += Vector3.down * (BPM/60f) * Time.deltaTime;
        }
        public event Action OnClearEvent;

        public void Clear()
        {
            OnClearEvent?.Invoke();
        }

        public void ManualUpdate(float BPM)
        {
            // ノーツの位置をBPMに基づいて更新
            Move(BPM);
            
            // 画面外に出たらクリアイベントを発火
            if (_notePrefab.transform.position.y < -5f) // 画面外のY座標を適宜調整
            {
                OnClearEvent?.Invoke();
                Debug.Log("Note cleared.");
            }
        }
    }

}
