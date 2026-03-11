using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class NoteSpeedDisplayPresenter : MonoBehaviour
{
    [SerializeField]
    private NoteSpeedDisplayModel _model;
    [SerializeField]
    private NoteSpeedDisplayView _view;

    public NoteSpeedDisplayModel Model => _model;

    public void Initialize()
    {
        _model = new NoteSpeedDisplayModel(0);
        _view.Initialize();
        Bind();
    }

    private void Bind()
    {
        _model._notesSpeed
            .Subscribe(OnSpeedChanged)
            .AddTo(gameObject);
    }

    private void OnSpeedChanged(int speed)
    {
        _view.ShowSpeed(speed);
    }
}
