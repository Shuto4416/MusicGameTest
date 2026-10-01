using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;
using R3;
using Zenject;
using InputSystem;

public class P_LatencyAdjustment : MonoBehaviour
{
    ILatencyAdjustInputProvider latencyAdjustInputProvider;
    [Inject]
    void Injection(ILatencyAdjustInputProvider latencyAdjustInputProvider)
    {
        this.latencyAdjustInputProvider = latencyAdjustInputProvider;
    }

    M_LatencyAdjustment m_LA;
    [SerializeField] V_LatencyAdjustment v_LA;
    // Start is called before the first frame update
    float LScore;
    bool flag;
    bool prepareFlag;
    public bool isClick => v_LA.isClick;
    public void Initialize()
    {
        m_LA = new M_LatencyAdjustment(0);
        v_LA.Initialize();
        Bind();
        flag = false;
        prepareFlag = false;
    }

    private void Bind()
    {
        m_LA._LScore
            .Subscribe(SetText)
            .AddTo(gameObject);
    }

    private void SetText(float LScore)
    {
        v_LA.SetText(LScore, m_LA._AvgLScore);
    }

    // Update is called once per frame
    public void ManualUpdate()
    {
        if (latencyAdjustInputProvider.Space()) prepareFlag = true;

        if (prepareFlag) Measure(latencyAdjustInputProvider.Space());
    }

    void Measure(bool isPressSpace)
    {
        if (LScore == 0)
        {
            SoundManager.instance.PlaySE(SEFile.Tap);
        }
        if (isPressSpace && !flag)
        {
            m_LA.Measure(LScore);
            flag = true;
        }
        LScore += Time.deltaTime;
        if (LScore > 1)
        {
            LScore = 0;
            flag = false;
        }
    }

    public void Hide()
    {
        v_LA.Hide();
    }
}
