using System.Collections;
using System.Collections.Generic;
using SongDatas;
using UnityEngine;

public class SongFrameManager : MonoBehaviour
{
    [SerializeField] SongFrame songFrame;
    [SerializeField] GameObject songFrameObj;
    [SerializeField] GameObject BackGroundObj;

    public void ChangeSong(bool condition, SongComponent songComponent)
    {
        if (condition) songFrame.Initialize(songComponent);
    }

    public void Show()
    {
        songFrameObj.SetActive(true);
        BackGroundObj.SetActive(true);
    }
    public void Hide()
    {
        songFrameObj.SetActive(false);
        BackGroundObj.SetActive(false);
    }
}
