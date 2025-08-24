using System;
using System.Collections.Generic;
using UnityEngine;
using Audio;
using Notes;


public class NotesManager : MonoBehaviour
    {
        private List<LinkedList<BaseNote>> UsingNotesObj = new List<LinkedList<BaseNote>>();
        private LinkedList<BaseNote> UnUseNotesObj = new LinkedList<BaseNote>();
        private LinkedList<LongNote> UnUseLongNotesObj = new LinkedList<LongNote>();

        public List<LinkedList<BaseNote>> UsingNotesObjDatas => UsingNotesObj;


        [SerializeField]
        BaseNote baseNote;

        [SerializeField]
        LongNote longNote;


        public void Initialize(int maxBlock)
        {
            UsingNotesObj.Clear();
            for (int i = 0; i < maxBlock; i++) UsingNotesObj.Add(new LinkedList<BaseNote>());
        }

        private Vector3 GeneratePosition(int i, float NotesSpeed, List<NotesData> notesDatas)
        {
            float y = 0;
            float time = notesDatas[i].noteAbsTime;
            if(i != 0)
            {
                int beforeLaneNum = notesDatas[i-1].laneNum;
                // 直前に生成したノーツの情報を取得
                y = UsingNotesObj[beforeLaneNum].Last.Value.transform.position.y;
                time = notesDatas[i].noteRelTime * notesDatas[i-1].noteSoftLanding / 100f;
            }
            Vector3 vector3 = new Vector3(-2.5f + notesDatas[i].laneNum, y + NotesSpeed/60f * time, -1);
            return vector3;
        }


        public void Generate(int i, float NotesSpeed, List<NotesData> notesDatas)
        {
            Vector3 vector3 = GeneratePosition(i, NotesSpeed, notesDatas);
            if (notesDatas[i].noteType == 1) {
                if (UnUseNotesObj.Count > 0) {
                    BaseNote note = UnUseNotesObj.First.Value;
                    UnUseNotesObj.RemoveFirst();
                    UsingNotesObj[notesDatas[i].laneNum].AddLast(note);
                    NoteInitialize(note, notesDatas[i].laneNum, notesDatas[i].noteType, notesDatas[i].noteSoftLanding, notesDatas[i].noteAbsTime);
                    note.gameObject.SetActive(true);
                    note.transform.position = vector3;
                }
            }
            else if (notesDatas[i].noteType == 2) {
                Debug.Log("LongNote created");
                if (UnUseLongNotesObj.Count > 0) {
                    LongNote note = UnUseLongNotesObj.First.Value;
                    UnUseLongNotesObj.RemoveFirst();
                    UsingNotesObj[notesDatas[i].laneNum].AddLast(note);
                    Vector3 endPoint = GeneratePosition(i+1, NotesSpeed, notesDatas);
                    note.Initialize(notesDatas[i].laneNum, notesDatas[i].noteType, notesDatas[i].noteSoftLanding, notesDatas[i].noteAbsTime, notesDatas[i+1].noteSoftLanding, notesDatas[i+1].noteAbsTime, endPoint);
                    note.gameObject.SetActive(true);
                    note.transform.position = vector3;
                }
            }
        }

        public BaseNote Create()
        {
            GameObject obj = Instantiate(baseNote.gameObject, new Vector3(0,100,0), Quaternion.identity);
            BaseNote objsNote = obj.GetComponent<BaseNote>();
            UnUseNotesObj.AddLast(objsNote);
            obj.SetActive(false);
            return objsNote;
        }

        public BaseNote LongNoteCreate()
        {
            GameObject obj = Instantiate(longNote.gameObject, new Vector3(0,100,0), Quaternion.identity);
            LongNote objsNote = obj.GetComponent<LongNote>();
            UnUseLongNotesObj.AddLast(objsNote);
            obj.SetActive(false);
            return objsNote;
        }

        public void Pop(int laneNum, int noteType)
        {
            if (UsingNotesObj[laneNum].Count > 0)
            {
                BaseNote note = UsingNotesObj[laneNum].First.Value;
                UsingNotesObj[laneNum].RemoveFirst();
                if (noteType == 1) UnUseNotesObj.AddLast(note);
                if (noteType == 2) UnUseLongNotesObj.AddLast(note as LongNote);
                note.gameObject.transform.position = new Vector3(0, 100, 0); // 画面外に移動
                note.gameObject.SetActive(false);
            }
        }


    public void Lock()
    {
        foreach(BaseNote Obj in UnUseNotesObj)
        {
            Obj.transform.position = new Vector3(0,100,0);
        }
        foreach(BaseNote Obj in UnUseLongNotesObj)
        {
            Obj.transform.position = new Vector3(0,100,0);
        }
    }

    public void NoteInitialize(BaseNote note, int laneNum, int noteType, int noteSoftLanding, float lifeSpan)
    {
        note.Initialize(laneNum, noteType, noteSoftLanding, lifeSpan);
    }


}


