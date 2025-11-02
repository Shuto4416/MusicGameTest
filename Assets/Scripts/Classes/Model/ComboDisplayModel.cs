using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class ComboDisplayModel
{
    public ReactiveProperty<int> _combo;
    public int Combo => _combo.Value;
    public ComboDisplayModel(int initCombo)
    {
        _combo = new ReactiveProperty<int>(initCombo);
    }

    public void Set(int num)
    {
        _combo.Value = num;
    }
}
