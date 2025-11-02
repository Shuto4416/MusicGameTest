using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class ComboDisplayPresenter : MonoBehaviour
{
    [SerializeField]
    private ComboDisplayModel _model;
    [SerializeField]
    private ComboDisplayView _view;

    public ComboDisplayModel Model => _model;

    public void Initialize()
    {
        _model = new ComboDisplayModel(0);
        _view.Initialize();
        Bind();
    }

    private void Bind()
    {
        _model._combo
            .Subscribe(OnComboChanged)
            .AddTo(gameObject);
    }

    private void OnComboChanged(int combo)
    {
        _view.ShowCombo(combo);
    }
}
