using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;
using Audio;

public class Result : MonoBehaviour
{
    [SerializeField]
    private GameObject _result;
    [SerializeField]
    private GameObject _scoreObj;
    [SerializeField]
    private TextMeshPro _scoreText;
    [SerializeField]
    private GameObject _notesJudge;
    [SerializeField]
    private TextMeshPro _perfectCount;
    [SerializeField]
    private TextMeshPro _greatCount;
    [SerializeField]
    private TextMeshPro _badCount;
    [SerializeField]
    private TextMeshPro _missCount;
    [SerializeField]
    private GameObject[] Ranks;

    public async UniTask Show(int PC, int GC, int BC, int MC, int MaxNotesCount, CancellationToken token)
    {
        int _PC = 0, _GC = 0, _BC = 0, _MC = 0, _Score = 0;
        int Score = PC * 10000 + GC * 7000 + BC * 1000;
        float RankPercent = Score*100 / (MaxNotesCount * 10000);
        var sequence = DOTween.Sequence();
        _result.SetActive(true);
        SoundManager.instance.PlaySE(SEFile.AddScore);
        // _scoreObj.SetActive(true);
        // _notesJudge.SetActive(true);
        await sequence.Insert(0, DOTween.To(
            () => _PC,          // 何を対象にするのか
            num =>
            {
                _PC = num;
                _perfectCount.text = $": {_PC}";
            },   // 値の更新
            PC,                  // 最終的な値
            1f                  // アニメーション時間
            ))
            .Insert(0, DOTween.To(
            () => _GC,          // 何を対象にするのか
            num =>
            {
                _GC = num;
                _greatCount.text = $": {_GC}";
            },   // 値の更新
            GC,                  // 最終的な値
            1f                  // アニメーション時間
            ))
            .Insert(0, DOTween.To(
            () => _BC,          // 何を対象にするのか
            num =>
            {
                _BC = num;
                _badCount.text = $": {_BC}";
            },   // 値の更新
            BC,                  // 最終的な値
            1f                  // アニメーション時間
            ))
            .Insert(0, DOTween.To(
            () => _MC,          // 何を対象にするのか
            num =>
            {
                _MC = num;
                _missCount.text = $": {_MC}";
            },   // 値の更新
            MC,                  // 最終的な値
            1f                  // アニメーション時間
            ))
            .Insert(0, DOTween.To(
            () => _Score,          // 何を対象にするのか
            num =>
            {
                _Score = num;
                _scoreText.text = $"{_Score}";
            },   // 値の更新
            Score,                  // 最終的な値
            1f                  // アニメーション時間
            ))
            .OnComplete(() =>
            {
                Debug.Log($"YourScore: {Score} / {MaxNotesCount * 10000} = {RankPercent}%");
                var _RP = RankPercent;
                int _temp = 0;
                if (_RP >= 95f)
                {
                    _temp = 0;
                }
                else if (_RP >= 90f)
                {
                    _temp = 1;
                }
                else if (_RP >= 80f)
                {
                    _temp = 2;
                }
                else if (_RP >= 65f)
                {
                    _temp = 3;
                }
                else if (_RP >= 50f)
                {
                    _temp = 4;
                }
                else
                {
                    _temp = 0;
                }
                Ranks[_temp].SetActive(true);
            }).Play()
            .ToUniTask(cancellationToken: token);
    }

    public void Initialize()
    {
        Hide();
    }

    private void Hide()
    {
         foreach (var obj in Ranks)
        {
            obj.SetActive(false);
        }
        _result.SetActive(false);
    }
}
