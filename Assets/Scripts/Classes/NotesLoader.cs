using System;
using System.Collections;
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
    public Note[] notes;
}

    public class NotesData
    {
        public int laneNum;
        public int noteType;
        public int noteSoftLanding;
        public float noteAbsTime;
        public float noteRelTime;

        public NotesData(int laneNum, int noteType, int noteSoftLanding, float noteAbsTime, float noteRelTime)
        {
            this.laneNum = laneNum;
            this.noteType = noteType;
            this.noteSoftLanding = noteSoftLanding;
            this.noteAbsTime = noteAbsTime;
            this.noteRelTime = noteRelTime;
        }
    }

public class NotesLoader : MonoBehaviour
{
    //総ノーツ数
    private int noteNum;
    private int maxBlock;
    //曲名
    private string songName;
    private List<NotesData> notesDatas = new List<NotesData>();
    public int NoteNum => noteNum;
    public int MaxBlock => maxBlock;
    public List<NotesData> NotesDatas => notesDatas;

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
        // jsonファイルを読み込む
        string inputString = Resources.Load<TextAsset>(SongName).ToString();
        Data inputJson = JsonUtility.FromJson<Data>(inputString);
        //for (int i = 0; i < inputJson.notes.Length; i++) Debug.Log($" type: {inputJson.notes[i].type}, num: {inputJson.notes[i].num}, block: {inputJson.notes[i].block}, LPB: {inputJson.notes[i].LPB}, softLanding: {inputJson.notes[i].softLanding}");

        //総ノーツ数を設定
        noteNum = inputJson.notes.Length;
        Debug.Log($"総ノーツ数: {noteNum}, 曲名: {inputJson.name}, BPM: {inputJson.BPM}, オフセット: {inputJson.offset}");
        Debug.Log($"ノーツ1 LPB: {inputJson.notes[0].LPB}");
        maxBlock = inputJson.maxBlock;
        CreateNotesList(inputJson, inputJson.notes);
        for (int i = 0; i < notesDatas.Count; i++)
        {
            notesDatas[i].noteRelTime = i != 0 ? notesDatas[i].noteAbsTime - notesDatas[i - 1].noteAbsTime : notesDatas[i].noteAbsTime;
        }
        for (int i = 0; i < notesDatas.Count; i++) Debug.Log($" type: {notesDatas[i].noteType}, lanenum: {notesDatas[i].laneNum}, AbsTime: {notesDatas[i].noteAbsTime}, RelTime: {notesDatas[i].noteRelTime}, softLanding: {notesDatas[i].noteSoftLanding}");
    }

    private void CreateNotesList(Data inputJson, Note[] notes)
    {
        for (int i = 0; i < notes.Length; i++)
        {
            //時間を計算
            float absTime = (60 / (inputJson.BPM * (float)notes[i].LPB) * notes[i].num)/* + inputJson.offset * 0.01f*/;
            //リストに追加
            notesDatas.Add(new NotesData(notes[i].block, notes[i].type, notes[i].softLanding, absTime, 0));
            if (notes[i].notes != null) CreateNotesList(inputJson, notes[i].notes);
        }
    }

}
