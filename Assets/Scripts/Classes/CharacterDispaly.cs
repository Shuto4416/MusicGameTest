using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public enum NotesJudgeState
    {
        Miss,
        Bad,
        Great,
        Perfect
    }

public class CharacterDispaly : MonoBehaviour
{


    [SerializeField]
    private NotesJudgementDisplay Perfect;
    [SerializeField]
    private NotesJudgementDisplay Great;
    [SerializeField]
    private NotesJudgementDisplay Bad;
    [SerializeField]
    private NotesJudgementDisplay Miss;

    private Sequence ExecutionSequence;

    public void NotesJudgePlay(NotesJudgeState state)
    {
        ExecutionSequence?.Complete();
        switch (state)
        {
            case NotesJudgeState.Miss:
                ExecutionSequence = Miss.Display();
                break;
            case NotesJudgeState.Bad:
                ExecutionSequence = Bad.Display();
                break;
            case NotesJudgeState.Great:
                ExecutionSequence = Great.Display();
                break;
            case NotesJudgeState.Perfect:
                ExecutionSequence = Perfect.Display();
                break;
        }
        ExecutionSequence?.Play();
    }
}
