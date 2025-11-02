using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class ComboDisplayView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _comboText;
    [SerializeField]
    private NotesJudgementDisplay Combo;
    public void Initialize()
    {
        _comboText.text = "";
    }
    public void ShowCombo(int combo)
    {
        _comboText.text = combo.ToString();
        if (combo > 1)
        {
            Combo.Display().Play();
        }
        else
        {
            Combo.Display().Complete();
        }
    }
}
