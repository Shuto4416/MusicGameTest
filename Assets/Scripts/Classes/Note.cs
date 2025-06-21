using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;


namespace Notes {
    public class Note : MonoBehaviour, INote
    {
        public int Type;  // ノーツの種類（通常ノーツ・ロングノーツなど）
        public int BeatNum;   // 何拍目に配置されるか
        public int LaneNum; // どのレーンに配置されるか
        public int LPB;   // 1拍あたりの分割数
        public void Move(float BPM)
        {
            transform.position += Vector3.down * (BPM/60f) * Time.deltaTime;
        }
        void Update()
        {
            Move(400);
        }
    }

}
