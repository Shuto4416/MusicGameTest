using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NotesJudgementDisplay : MonoBehaviour
{

    public Sequence Display()
    {
        Sequence sequence = DOTween.Sequence().OnStart(() => gameObject.SetActive(true)).OnComplete(() => gameObject.SetActive(false));
        sequence.Append(transform.DOScale(new Vector2(0.8f, 0.8f), 0f));
        sequence.Append(transform.DOScale(new Vector2(1f, 1f), 0.1f));
        sequence.AppendInterval(2f);
        return sequence;
    }
}
