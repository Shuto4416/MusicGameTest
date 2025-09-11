using System.Collections;
using System.Collections.Generic;
using Notes;
using InputSystem;
using Lights;
using UnityEngine;
using Unity.VisualScripting;
using Zenject;
using UnityEditor.VersionControl;
using System;
using TheSingleton;

namespace InGame {
    public class InGamePresenter : MonoBehaviour
    {
        [SerializeField] private Lights.Light[] _lights = new Lights.Light[6];
        [SerializeField] private NotesManager _notesManager;
        [SerializeField] private NotesLoader _notesLoader;
        [SerializeField] private int _maxNoteNum = 100; // 最大ノーツ数
        [SerializeField] private int _defaultNoteNum = 60; // 初期ノーツ数
        [SerializeField] private float _noteSpeed;
        private IInputProvider inputProvider;

        private List<BaseNote> _notes = new List<BaseNote>();

        private string songName = "01 - Re_Unknown X";
        private int CurrentNoteNum;
        private float sofLan;

        [Inject]
        void Injection(IInputProvider inputProvider)
        {
            this.inputProvider = inputProvider;
        }
        void Start()
        {
            Load();
            Initialize();
        }

        void VariableInitialize()
        {
            sofLan = 100;
            CurrentNoteNum = 0;
        }

        // Update is called once per frame
        void Update()
        {
            _notesManager.Lock();
            foreach (BaseNote note in _notes)
            {
                note.ManualUpdate(_noteSpeed * sofLan / 100);
            }
            TimeManager.instance.ManualUpdate();
            inputProvider.ManualUpdate();
            LaneLightUp();
        }


        void PrepareNote()
        {
            BaseNote baseNote = _notesManager.Create();
            BaseNote longNote = _notesManager.LongNoteCreate();
            Notes.Note note = baseNote.GetComponent<Notes.Note>();
            Notes.LongNote LongNote = longNote.GetComponent<Notes.LongNote>();
            _notes.Add(note);
            _notes.Add(LongNote);
            BindNotes(note);
            BindNotes(LongNote);

        }
        void BindNotes(BaseNote note)
        {
            note.OnClearEvent += () =>
            {
                int laneNum = note.LaneNum;
                Debug.Log($"{note.LaneNum}");
                _notesManager.Pop(note.LaneNum, note.NoteType);
                Generate();
                Debug.Log("Note cleared and created new note.");
            };
            note.OnSofLanEvent += (_) =>
            {
                sofLan = _;
            };
        }





        void Generate()
        {
            if (CurrentNoteNum < _notesLoader.NoteNum)
            {
                _notesManager.Generate(CurrentNoteNum, _noteSpeed, _notesLoader.NotesDatas);
                if (_notesLoader.NotesDatas[CurrentNoteNum].noteType == 2) CurrentNoteNum++;
                CurrentNoteNum++;
                Debug.Log("Note created: " + CurrentNoteNum);
            }
        }

        void Load()
        {
            _notesLoader.Initialize(songName);
            _notesManager.Initialize(_notesLoader.MaxBlock);
            Debug.Log($"NotesNum: {_notesLoader.NoteNum}");
        }

        void InputBind()
        {
            BindPush();
        }
        void LaneLightUp()
        {
            if (_lights.Length == 0)
            {
                Debug.LogWarning("No lights assigned to InGamePresenter.");
                return;
            }
            for (int i = 0; i < _lights.Length; i++)
            {
                _lights[i].LightController(inputProvider.IsPushAllLanes()[i]);
            }
        }


        void Initialize()
        {
            VariableInitialize();
            foreach (var light in _lights)
            {
                light.Initialize();
            }
            Load();
            for (int i = 0; i < _maxNoteNum; i++)
            {
                PrepareNote();
            }
            for (int i = 0; i < _defaultNoteNum; i++)
            {
                Generate();
            }
            // Debug.Log($"NoteNum: {_notesManager.noteNum} , NoteData: {_notesManager.LaneNum[0]} , {_notesManager.NotesTime[0] }, {_notesManager.NoteSoftLanding[0] }");
            // for (int i = 0; i < _notesManager.noteNum; i++)
            // {
            //     _notesData.Add((_notesManager.LaneNum[i], _notesManager.NotesTime[i], _notesManager.NoteSoftLanding[i]));
            // }
            inputProvider.Initialize();
            InputBind();
        }

        void BindPush()
        {
            Debug.Log("Push");
            for (int i = 0; i < inputProvider.KeyCount(); i++)
            {
                //inputProvider.SubscriptForKeyDownOnceAction(i, () => NoteJudge(i));
                inputProvider.SubscriptForKeyDownAction(i, () => LongNoteJudge(i));
            }
            inputProvider.SubscriptForKeyDownOnceAction(0, () => NoteJudge(0));
            inputProvider.SubscriptForKeyDownOnceAction(1, () => NoteJudge(1));
            inputProvider.SubscriptForKeyDownOnceAction(2, () => NoteJudge(2));
            inputProvider.SubscriptForKeyDownOnceAction(3, () => NoteJudge(3));
            inputProvider.SubscriptForKeyDownOnceAction(4, () => NoteJudge(4));
            inputProvider.SubscriptForKeyDownOnceAction(5, () => NoteJudge(5));

        }

        void NoteJudge(int laneNum)
        {
            Debug.Log($"Lane {laneNum} pushed");
            float NearestTime = float.MaxValue;
            int NearestTimeNoteNum = -1;
            Notes.Note note = null;
            Notes.LongNote longNote = null;
            for (int i = CurrentNoteNum - _defaultNoteNum < 0 ? 0 : CurrentNoteNum - _defaultNoteNum; i < CurrentNoteNum; i++)
            {
                if (_notesLoader.NotesDatas[i].laneNum != laneNum) continue;
                float time = TimeManager.instance.CurrentTime - _notesLoader.NotesDatas[i].noteAbsTime;
                if (Math.Abs(time) < NearestTime)
                {
                    NearestTime = time;
                    NearestTimeNoteNum = i;
                }
            }
            Debug.Log($"NearestTime: {NearestTime}, NearestTimeNoteNum: {NearestTimeNoteNum}");
            if (NearestTimeNoteNum == -1) return;
            // 普通のノーツの時
            if (_notesLoader.NotesDatas[NearestTimeNoteNum].noteType == 1)
            {
                foreach (var _note in _notesManager.UsingNotesObjDatas[laneNum])
                    if (_note.LifeSpan == _notesLoader.NotesDatas[NearestTimeNoteNum].noteAbsTime)
                        note = _note.GetComponent<Notes.Note>();
                if (note.LifeSpan - TimeManager.instance.CurrentTime < 1f / 60f * 13.5f) note.Invisible();
            }
            // ロングノーツの時
            else if (_notesLoader.NotesDatas[NearestTimeNoteNum].noteType == 2)
            {
                foreach (var _note in _notesManager.UsingNotesObjDatas[laneNum])
                    if (_note.LifeSpan == _notesLoader.NotesDatas[NearestTimeNoteNum].noteAbsTime)
                        longNote = _note.GetComponent<LongNote>();
                    if (NearestTime < 1f/60f*5f) longNote.Push();
            }
            
        }
        
        void LongNoteJudge(int laneNum)
        {
            Debug.Log($"Lane {laneNum} pressed");
            float NearestTime = float.MaxValue;
            int NearestTimeNoteNum = -1;
            Notes.LongNote longNote = null;
            for (int i = CurrentNoteNum-_defaultNoteNum < 0 ? 0 : CurrentNoteNum - _defaultNoteNum; i < CurrentNoteNum; i++)
            {
                if(_notesLoader.NotesDatas[i].laneNum != laneNum) continue;
                float time = TimeManager.instance.CurrentTime - _notesLoader.NotesDatas[i].noteAbsTime;
                if (Math.Abs(time) < NearestTime)
                {
                    NearestTime = time;
                    NearestTimeNoteNum = i;
                }
            }
            Debug.Log($"NearestTime: {NearestTime}, NearestTimeNoteNum: {NearestTimeNoteNum}");
            if (NearestTimeNoteNum == -1) return;
            if (_notesLoader.NotesDatas[NearestTimeNoteNum].noteType == 1) return;
            if (_notesLoader.NotesDatas[NearestTimeNoteNum].noteType == 2) {
                foreach (var _note in _notesManager.UsingNotesObjDatas[laneNum])
                    if (_note.LifeSpan == _notesLoader.NotesDatas[NearestTimeNoteNum].noteAbsTime)
                        longNote = _note.GetComponent<LongNote>();
            }
            if (NearestTime < 1f/60f*5f) longNote.Press();
        }

    }
}

