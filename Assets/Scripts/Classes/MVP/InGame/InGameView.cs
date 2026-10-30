using System.Collections;
using System.Collections.Generic;
using Notes;
using InputSystem;
using Lights;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using Zenject;

namespace InGame{
    public class InGameView : MonoBehaviour
    {
        [SerializeField]
        private Lights.Light[] _lights = new Lights.Light[6];
        [SerializeField]
        private Lights.Light[] _ELights = new Lights.Light[6];
        [SerializeField]
        private Lights.Light[] _HideLights = new Lights.Light[6];
        [SerializeField]
        private ParticleSystem[] _tapEffects = new ParticleSystem[6];
        public Lights.Light[] Lights => _lights;
        public Lights.Light[] ELights => _ELights;
        public Lights.Light[] HideLights => _HideLights;
        public ParticleSystem[] TapEffects => _tapEffects;
        [SerializeField]
        private CharacterDispaly _characterDisplay;
        public CharacterDispaly CharacterDispaly => _characterDisplay;
        [SerializeField]
        private SongFrameManager songFrameManager;
        public SongFrameManager SongFrameManager => songFrameManager;
        [SerializeField]
        private Result _result;
        public Result result => _result;


    }
    //[SerializeField] private Lights.Light[] _lights = new Lights.Light[6];
}