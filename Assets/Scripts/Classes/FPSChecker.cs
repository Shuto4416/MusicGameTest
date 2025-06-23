using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSChecker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI _fps;
    private int _num = 0;
    private int _count = 0;
    private int _numSum = 0;
    private float _currentTime = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;
        _num++;
        if (_currentTime > 1f) {
            Debug.Log("１秒経過");
            _numSum += _num;
            _count++;
            _fps.text = $"FPS:{_numSum/_count}";
            _fps.text = $"FPS:{_num}";
            _num=0;
            _currentTime=0;
        }
    }
}
