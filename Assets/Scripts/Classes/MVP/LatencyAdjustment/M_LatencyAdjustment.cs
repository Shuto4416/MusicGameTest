using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class M_LatencyAdjustment
{
    public ReactiveProperty<float> _LScore;
    public float LScore => _LScore.Value;

    public float _AvgLScore;
    private float _SumLScore;
    private int _callCount;
    public M_LatencyAdjustment(int init)
    {
        _LScore = new ReactiveProperty<float>(init);
        _AvgLScore = 0;
        _callCount = 0;
        _SumLScore = 0;
    }

    public void Measure(float num)
    {
        Set(num);
        Add(num);
    }

    public void Set(float num)
    {
        _LScore.Value = num;
    }

    public void Add(float num)
    {
        _SumLScore += num;
        _AvgLScore = _SumLScore/++_callCount;
    }
}
