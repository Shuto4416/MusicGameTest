using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommandLogger
{
    private char[] _cmd;
    public char[] Cmd => _cmd;
    private float SavedTime;
    public InputCommandLogger()
    {
        _cmd = new char[10];
        for (int i = 0; i < _cmd.Length; i++)
        {
            _cmd[i] = 'N';
        }
        SavedTime = -1f;
    }

    public void inputChar(char c, float time)
    {
        for (int i = _cmd.Length - 1; i > 0; i--)
        {
            _cmd[i] = _cmd[i - 1];
        }
        _cmd[0] = c;
        SavedTime = time;
    }

    public void ManualUpdate(float time)
    {
        if (time - SavedTime < 3f) return;
        inputChar('N', time);
    }

    public void ResetCommand()
    {
        for (int i = 0; i < _cmd.Length; i++)
        {
            _cmd[i] = 'N';
        }
    }


}
