using System;
using System.Collections.Generic;
using UnityEngine;
using Audio;
using Notes;


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
        private List<int> _laneNum = new List<int>();
        //ノーツの種類
        public List<int> NoteType = new List<int>();
        // ノーツの速度倍率
        private List<int> _noteSoftLanding = new List<int>();
        //ノーツが判定線と重なる時間
        private List<float> _notesTime = new List<float>();
        public List<int> LaneNum => _laneNum;
        public List<int> NoteSoftLanding => _noteSoftLanding;
        public List<float> NotesTime => _notesTime;
        private List<LinkedList<BaseNote>> UsingNotesObj = new List<LinkedList<BaseNote>>();
        private LinkedList<BaseNote> UnUseNotesObj = new LinkedList<BaseNote>();
        //ノーツの速度
        [SerializeField] private float _notesSpeed;
        public float NotesSpeed => _notesSpeed;
        //ノーツのprefabを入れる
        [SerializeField] Notes.Note note;

        public void Initialize(string songName)
        {
            //総ノーツを0にする
            noteNum = 0;
            //読み込む譜面のファイル名を入力

            Load(songName);
            SoundManager.instance.PlayBGM(BGMFile.GameScene);
        }

        private void Load(string SongName)
        {
            UsingNotesObj.Clear();
            // jsonファイルを読み込む
            string inputString = Resources.Load<TextAsset>(SongName).ToString();
            Data inputJson = JsonUtility.FromJson<Data>(inputString);

            for (int i = 0; i < inputJson.maxBlock; i++) UsingNotesObj.Add(new LinkedList<BaseNote>());

            //総ノーツ数を設定
            noteNum = inputJson.notes.Length;
            Debug.Log($"総ノーツ数: {noteNum}, 曲名: {inputJson.name}, BPM: {inputJson.BPM}, オフセット: {inputJson.offset}");
            Debug.Log($"ノーツ1 LPB: {inputJson.notes[0].LPB}");



            for (int i = 0; i < inputJson.notes.Length; i++)
            {
                //時間を計算
                float time = (60 / (inputJson.BPM * (float)inputJson.notes[i].LPB) * inputJson.notes[i].num)/* + inputJson.offset * 0.01f*/;
                //リストに追加
                _notesTime.Add(time);
                _laneNum.Add(inputJson.notes[i].block);
                NoteType.Add(inputJson.notes[i].type);
                _noteSoftLanding.Add(inputJson.notes[i].softLanding);
            }
        }

        public void CreateNote(int i)
        {
            float y = 0;
            float time = NotesTime[i];
            int laneNum = LaneNum[i];
            if(i != 0)
            {
                // 直前に生成したノーツの情報を取得
                y = UsingNotesObj[LaneNum[i-1]].Last.Value.transform.position.y;
                time -= NotesTime[i - 1];
                time *= NoteSoftLanding[i-1] / 100f;
            }
            Push(laneNum, y, time);
        }

        public BaseNote Create()
        {
            GameObject obj = Instantiate(note.gameObject, new Vector3(0,100,0), Quaternion.identity);
            BaseNote objsNote = obj.GetComponent<BaseNote>();
            UnUseNotesObj.AddLast(objsNote);
            obj.SetActive(false);
            return objsNote;
        }

        public void Pop(int laneNum)
        {
            if (UsingNotesObj[laneNum].Count >= 0)
            {
                BaseNote note = UsingNotesObj[laneNum].First.Value;
                UsingNotesObj[laneNum].RemoveFirst();
                UnUseNotesObj.AddLast(note);
                note.gameObject.transform.position = new Vector3(0, 100, 0); // 画面外に移動
                note.gameObject.SetActive(false);
            }
        }

    public void Push(int laneNum, float y, float time)
    {
        Vector3 vector3 = new Vector3(-2.5f + laneNum, y + NotesSpeed/60f * time, -1);
        if (UnUseNotesObj.Count > 0)
        {
            BaseNote note = UnUseNotesObj.First.Value;
            UnUseNotesObj.RemoveFirst();
            UsingNotesObj[laneNum].AddLast(note);
            note.Initialize(time, laneNum);
            note.gameObject.SetActive(true);
            note.transform.position = vector3;
        }
    }

    public void Lock()
    {
        foreach(BaseNote Obj in UnUseNotesObj)
        {
            Obj.transform.position = new Vector3(0,100,0);
        }
    }


}


