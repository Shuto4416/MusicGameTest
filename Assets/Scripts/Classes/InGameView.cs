using System.Collections;
using System.Collections.Generic;
using Notes;
using InputSystem;
using Lights;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using Zenject;
using UnityEditor.VersionControl;

namespace InGame{
    public class InGameView : MonoBehaviour
    {
        [SerializeField]
        private Lights.Light[] _lights = new Lights.Light[6];
        public Lights.Light[] Lights => _lights;
        [SerializeField]
        private CharacterDispaly _characterDisplay;
        public CharacterDispaly CharacterDispaly => _characterDisplay;
        [SerializeField]
        private SongFrameManager songFrameManager;
        public SongFrameManager SongFrameManager => songFrameManager;


    }
    //[SerializeField] private Lights.Light[] _lights = new Lights.Light[6];
}