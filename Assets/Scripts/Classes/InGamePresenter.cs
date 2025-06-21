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
        [SerializeField] private List<Notes.Note> _notes = new List<Notes.Note>();
        [SerializeField] private Lights.Light[] _lights = new Lights.Light[6];
        private IInputProvider inputProvider;

        [Inject]
        void Injection(IInputProvider inputProvider){
            this.inputProvider = inputProvider;
        }
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            NoteMove();
            LightController();
        }

        void NoteMove()
        {
            foreach (var note in _notes)
            {
                note.Move(_bpm);
            }
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
        }

    }
}

