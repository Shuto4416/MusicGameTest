using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputSystem;

namespace Lights
{
    public class Light : MonoBehaviour
    {
        [SerializeField] private float FadeSpeed = 3f;
        private Renderer _rend;
        private float _alpha = 0f;
        // Start is called before the first frame update
        public void Initialize()
        {
            _rend = GetComponent<Renderer>();
            if (_rend == null)
            {
                Debug.LogError("Renderer component not found on the Light object.");
            }
        }

        // Update is called once per frame
        public void LightController(bool Input)
        {
            if (!(_rend.material.color.a <= 0))
            {
                _rend.material.color = new Color(_rend.material.color.r, _rend.material.color.r, _rend.material.color.r, _alpha);
            }
            if(Input)
            {
                ColorChange();
            }
            _alpha -= FadeSpeed * Time.deltaTime;
        }
        private void ColorChange()
        {
            _alpha = 0.3f;
            _rend.material.color = new Color(_rend.material.color.r, _rend.material.color.g, _rend.material.color.b, _alpha);
        }
    }
}

