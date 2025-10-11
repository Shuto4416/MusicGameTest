using System.Collections;
using System.Collections.Generic;
using Notes;
using InputSystem;
using Lights;
using UnityEngine;
using Unity.VisualScripting;
using Zenject;
using UnityEditor.VersionControl;

namespace InGame{
    public class InGameView : MonoBehaviour {
        [SerializeField] private Lights.Light[] _lights = new Lights.Light[6];
        public Lights.Light[] Lights => _lights;
    }
    //[SerializeField] private Lights.Light[] _lights = new Lights.Light[6];
}