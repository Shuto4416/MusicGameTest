using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class NoteSpeedDisplayModel
{
    public ReactiveProperty<int> _notesSpeed;
    public int NoteSpeed => _notesSpeed.Value;
    public NoteSpeedDisplayModel(int initCombo)
    {
        _notesSpeed = new ReactiveProperty<int>(initCombo);
    }

    public void Set(int num)
    {
        _notesSpeed.Value = num;
    }
}
