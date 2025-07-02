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

        private List<Notes.Note> _notes = new List<Notes.Note>();

        private string songName = "01 - Re_Unknown X";
        private int CurrentNoteNum;
        private int CurrentNum;
        private float sofLan;
        private List<(float laneNum, float noteTime, int noteSofLan)> _notesData;

        [Inject]
        void Injection(IInputProvider inputProvider){
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
            CurrentNum = 0;
        }

        // Update is called once per frame
        void Update()
        {
            _notesManager.Lock();
            LightController();
            foreach (Notes.Note note in _notes)
            {
                note.ManualUpdate(_noteSpeed * sofLan/100);
            }
            TimeManager.instance.ManualUpdate();
        }


        void PrepareNote()
        {
            BaseNote baseNote = _notesManager.Create();
            Notes.Note note = baseNote.GetComponent<Notes.Note>();
            _notes.Add(note);
            baseNote.OnClearEvent += () => {
                    int laneNum = baseNote.LaneNum;
                    Debug.Log($"{baseNote.LaneNum}");
                    _notesManager.Pop(baseNote.LaneNum);
                    CreateNote();
                    Debug.Log("Note cleared and created new note.");
                };
            note.OnSofLanEvent += () => {
                sofLan = note.NoteSoftLanding;
            };
        }





        void CreateNote()
        {
            if (CurrentNoteNum < _notesLoader.NoteNum)
            {
                _notesManager.CreateNote(CurrentNoteNum, _noteSpeed, _notesLoader.NotesDatas);
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

       
        void LightController()
        {
            if (_lights.Length == 0)
            {
                Debug.LogWarning("No lights assigned to InGamePresenter.");
                return;
            }
            bool[] inputs = inputProvider.IsPressedAllLanes();
            for (int i = 0; i < _lights.Length; i++)
            {
                _lights[i].LightController(inputs[i]);
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
                CreateNote();
            }
            // Debug.Log($"NoteNum: {_notesManager.noteNum} , NoteData: {_notesManager.LaneNum[0]} , {_notesManager.NotesTime[0] }, {_notesManager.NoteSoftLanding[0] }");
            // for (int i = 0; i < _notesManager.noteNum; i++)
            // {
            //     _notesData.Add((_notesManager.LaneNum[i], _notesManager.NotesTime[i], _notesManager.NoteSoftLanding[i]));
            // }
        }

        void Push(int laneNum)
        {
            float NearestTime = float.MaxValue;
            int NearestTimeNoteNum = -1;
            for (int i = CurrentNum-12 < 0 ? 0 : CurrentNum - 12; i < CurrentNum+12; i++)
            {
                if(_notesLoader.NotesDatas[i].laneNum != laneNum) continue;
                float time = TimeManager.instance.CurrentTime - _notesLoader.NotesDatas[i].noteAbsTime;
                if (Math.Abs(time) > NearestTime)
                {
                    NearestTime = time;
                    NearestTimeNoteNum = i;
                }

            }
            if (NearestTimeNoteNum == -1) return;

        }

    }
}

