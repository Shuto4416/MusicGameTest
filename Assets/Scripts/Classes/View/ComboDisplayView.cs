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
    private Sequence _comboSequence;
    public void Initialize()
    {
        _comboText.text = "";
    }
    public void ShowCombo(int combo)
    {
        
        _comboText.text = combo.ToString();
        if (combo > 1)
        {
            _comboSequence?.Complete();
            _comboSequence = Combo.Display();
            _comboSequence?.Play();
        }
        else
        {
            _comboSequence?.Complete();
        }
    }
}
