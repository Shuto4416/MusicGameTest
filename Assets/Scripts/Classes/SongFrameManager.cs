using System.Collections;
using System.Collections.Generic;
using SongDatas;
using UnityEngine;

public class SongFrameManager : MonoBehaviour
{
    [SerializeField] SongFrame prevSongFrame;
    [SerializeField] GameObject prevSongFrameObj;
    [SerializeField] SongFrame songFrame;
    [SerializeField] GameObject songFrameObj;
    [SerializeField] SongFrame nextSongFrame;
    [SerializeField] GameObject nextSongFrameObj;
    [SerializeField] GameObject BackGroundObj;
    [SerializeField] GameObject _noteSpeedObj;
    public void ChangePrevSong(bool condition, SongComponent songComponent)
    {
        if (condition) prevSongFrame.Initialize(songComponent);
    }
    public void ChangeSong(bool condition, SongComponent songComponent)
    {
        if (condition) songFrame.Initialize(songComponent);
    }
    public void ChangeNextSong(bool condition, SongComponent songComponent)
    {
        if (condition) nextSongFrame.Initialize(songComponent);
    }

    public void Show()
    {
        prevSongFrameObj.SetActive(true);
        songFrameObj.SetActive(true);
        nextSongFrameObj.SetActive(true);
        BackGroundObj.SetActive(true);
        _noteSpeedObj.SetActive(true);
    }
    public void Hide()
    {
        prevSongFrameObj.SetActive(false);
        songFrameObj.SetActive(false);
        nextSongFrameObj.SetActive(false);
        BackGroundObj.SetActive(false);
        _noteSpeedObj.SetActive(false);
    }

    public void Left()
    {
        float moveValue = -0.5f;
        prevSongFrame.MoveHorizon(moveValue);
        songFrame.MoveHorizon(moveValue);
        nextSongFrame.MoveHorizon(moveValue);
    }

    public void Right()
    {
        float moveValue = 0.5f;
        prevSongFrame.MoveHorizon(moveValue);
        songFrame.MoveHorizon(moveValue);
        nextSongFrame.MoveHorizon(moveValue);
    }
}
