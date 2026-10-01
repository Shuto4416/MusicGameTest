using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class V_LatencyAdjustment : MonoBehaviour
{
    [SerializeField] GameObject _Main;
    [SerializeField] TextMeshProUGUI _LatencyScore;
    [SerializeField] Button _OpenButton;
    [SerializeField] Button _CloseButton;

    public bool isClick;

    public void Initialize()
    {
        isClick = false;
        _LatencyScore.text = "遅延：0ms(平均：0ms)";
        Close();
        Show();
        _OpenButton.onClick.AddListener(() =>
        {
            ButtonON();
            Open();
        });
        _CloseButton.onClick.AddListener(() =>
        {
            ButtonOFF();
            Close();
        });
    }

    private void ButtonON()
    {
        isClick = true;
    }

    private void ButtonOFF()
    {
        isClick = false;
    }


    public void SetText(float LScore, float AvgLScore)
    {
        _LatencyScore.text = $"遅延：{LScore * 1000:F0}ms(平均：{AvgLScore * 1000:F0}ms)";
    }

    public void Open()
    {
        _Main.SetActive(true);
    }

    public void Close()
    {
        _Main.SetActive(false);
    }

    public void Show()
    {
        _OpenButton.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _OpenButton.gameObject.SetActive(false);
    }
}
