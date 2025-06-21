using System;
using System.Collections.Generic;
using UnityEngine;
using Audio;


    [Serializable]
    public class Data
    {
        public string name;  // 曲名
        public int maxBlock; // 最大ブロック数
        public int BPM;      // BPM（曲のテンポ）
        public int offset;   // 開始タイミングのオフセット 
        public Note[] notes; // ノーツ情報のリスト
    }

    [Serializable]
    public class Note
    {
        public int type;        // ノーツの種類（通常ノーツ・ロングノーツなど）
        public int num;         // 何拍目に配置されるか
        public int block;       // どのレーンに配置されるか
        public int LPB;         // 1拍あたりの分割数
        public int softLanding; //　ノーツの速度倍率(100=通常速度, 200=2倍速など)
    }

    public class NotesManager : MonoBehaviour
    {
        //総ノーツ数
        public int noteNum;
        //曲名
        private string songName;
        //ノーツのレーン
        public List<float> LaneNum = new List<float>();
        //ノーツの種類
        public List<int> NoteType = new List<int>();
        // ノーツの速度倍率
        public List<int> NoteSoftLanding = new List<int>();
        //ノーツが判定線と重なる時間
        public List<float> NotesTime = new List<float>();
        //gameobject
        public LinkedList<GameObject> NotesObj = new LinkedList<GameObject>();
        //ノーツの速度
        [SerializeField] private float NotesSpeed;
        //ノーツのprefabを入れる
        [SerializeField] GameObject noteObj;

        void Start()
        {
            //総ノーツを0にする
            noteNum = 0;
            //読み込む譜面のファイル名を入力
            songName = "01 - Re_Unknown X";

            Load(songName);
            SoundManager.instance.PlayBGM(BGMFile.GameScene);
        }

        private void Load(string SongName)
        {
            //jsonファイルを読み込む
            string inputString = Resources.Load<TextAsset>(SongName).ToString();
            Data inputJson = JsonUtility.FromJson<Data>(inputString);

            //総ノーツ数を設定
            noteNum = inputJson.notes.Length;
            Debug.Log($"総ノーツ数: {noteNum}, 曲名: {inputJson.name}, BPM: {inputJson.BPM}, オフセット: {inputJson.offset}");
            Debug.Log($"ノーツ1 LPB: {inputJson.notes[0].LPB}");



            for (int i = 0; i < inputJson.notes.Length; i++)
            {
                //時間を計算
                // float kankaku = 60 / (inputJson.BPM * (float)inputJson.notes[i].LPB/4);
                // float beatSec = kankaku * (float)inputJson.notes[i].LPB;
                // float time = (beatSec * inputJson.notes[i].num / (float)inputJson.notes[i].LPB) + inputJson.offset * 0.01f;
                float time = (60 / (inputJson.BPM * (float)inputJson.notes[i].LPB) * inputJson.notes[i].num)/* + inputJson.offset * 0.01f*/;
                //リストに追加
                NotesTime.Add(time);
                LaneNum.Add(inputJson.notes[i].block);
                NoteType.Add(inputJson.notes[i].type);
                NoteSoftLanding.Add(inputJson.notes[i].softLanding);
                
                // float y = NotesTime[i] * NotesSpeed;
                //ノーツを生成
                // NotesObj.Add(Instantiate(noteObj, new Vector3(-2.5f + inputJson.notes[i].block, y, -1), Quaternion.identity));
            }
            
            for (int i = 0; i < inputJson.notes.Length; i++)
            {
                CreateNote(i);
            }
        }

        public void CreateNote(int i)
        {
            float y = 0;
            float time = NotesTime[i];
            if(i != 0)
            {
                y = NotesObj.Last.Value.transform.position.y;
                time -= NotesTime[i - 1];
                time *= NoteSoftLanding[i-1] / 100f;
            }
            NotesObj.AddLast(Instantiate(noteObj, new Vector3(-2.5f + LaneNum[i], y + NotesSpeed * time, -1), Quaternion.identity));
        }

    }


