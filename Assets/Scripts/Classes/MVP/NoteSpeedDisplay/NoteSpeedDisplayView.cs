using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class NoteSpeedDisplayView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _noteSpeedText;
    public void Initialize()
    {
        _noteSpeedText.text = "";
    }
    public void ShowSpeed(int speed)
    {
        if (speed == 400)
        {
            _noteSpeedText.text = "ノーツ速度:遅い";
        }
        else if (speed == 600)
        {
            _noteSpeedText.text = "ノーツ速度:普通";
        }
        else if (speed == 800)
        {
            _noteSpeedText.text = "ノーツ速度:速い";
        }
        else if (speed == 1000)
        {
            _noteSpeedText.text = "ノーツ速度:とても速い";
        }
    }
}
