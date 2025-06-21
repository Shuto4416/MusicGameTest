using System.Collections;
using System.Collections.Generic;
using Notes;
using InputSystem;
using Lights;
using UnityEngine;
using Unity.VisualScripting;
using Zenject;
using UnityEditor.VersionControl;

namespace InGame {
    public class InGamePresenter : MonoBehaviour
    {
        [SerializeField] private float _bpm;
        [SerializeField] private Lights.Light[] _lights = new Lights.Light[6];
        [SerializeField] private NotesManager _notesManager;
        [SerializeField] private int _maxNoteNum = 100; // 最大ノーツ数
        [SerializeField] private int _defaultNoteNum = 60; // 初期ノーツ数
        private IInputProvider inputProvider;

        private List<Notes.Note> _notes = new List<Notes.Note>();

        private string songName = "01 - Re_Unknown X";
        private int CurrentNoteNum;

        [Inject]
        void Injection(IInputProvider inputProvider){
            this.inputProvider = inputProvider;
        }
        void Start()
        {
            Initialize();
            Load();
        }

        // Update is called once per frame
        void Update()
        {
            LightController();
            foreach (Notes.Note note in _notes)
            {
                note.ManualUpdate(_bpm);
            }
        }


        void PrepareNote()
        {
            GameObject noteObj = _notesManager.Create();
            _notes.Add(noteObj.GetComponent<Notes.Note>());
        }

        void Bind()
        {
            foreach(Notes.Note note in _notes)
            {
                note.OnClearEvent += () => {
                    _notesManager.Pop();
                    CreateNote();
                    Debug.Log("Note cleared and created new note.");
                };
            }
        }




        void CreateNote()
        {
            if (CurrentNoteNum < _notesManager.noteNum)
            {
                _notesManager.CreateNote(CurrentNoteNum);
                CurrentNoteNum++;
                Debug.Log("Note created: " + CurrentNoteNum);
            }
        }

        void Load()
        {
            _notesManager.Initialize(songName);
            Debug.Log($"NotesNum: {_notesManager.noteNum}");
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
            foreach (var light in _lights)
            {
                light.Initialize();
            }
            Load();
            for (int i = 0; i < _maxNoteNum; i++)
            {
                PrepareNote();
            }
            Bind();
            for (int i = 0; i < _defaultNoteNum; i++)
            {
                CreateNote();
            }
        }

    }
}

