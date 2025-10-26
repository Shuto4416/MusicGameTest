using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SongDatas;
using UnityEngine.UI;
using TMPro;
using System;

public class SongFrame : MonoBehaviour
{
    [SerializeField] TextMeshPro songName;
    [SerializeField] GameObject easyObj;
    [SerializeField] TextMeshPro easyNum;
    [SerializeField] GameObject normalObj;
    [SerializeField] TextMeshPro normalNum;
    [SerializeField] GameObject hardObj;
    [SerializeField] TextMeshPro hardNum;
    [SerializeField] GameObject expertObj;
    [SerializeField] TextMeshPro expertNum;
    [SerializeField] GameObject backGround;
    [SerializeField] Material easyColor;
    [SerializeField] Material normalColor;
    [SerializeField] Material hardColor;
    [SerializeField] Material expertColor;
    [SerializeField] Material NoDataColor;
    public void Initialize(SongComponent songComponent = null)
    {
        var backGroundRender = backGround.GetComponent<Renderer>();
        easyObj.SetActive(false);
        normalObj.SetActive(false);
        hardObj.SetActive(false);
        expertObj.SetActive(false);
        if (songComponent == null)
        {
            songName.text = "NoData";
            backGroundRender.material.color = NoDataColor.color;
            return;
        }
        if (songComponent.Easy != null)
        {
            easyObj.SetActive(true);
            easyNum.text = songComponent.Easy.Value.difficultyNum.ToString();
        }
        if (songComponent.Normal != null)
        {
            normalObj.SetActive(true);
            normalNum.text = songComponent.Normal.Value.difficultyNum.ToString();
        }
        if (songComponent.Hard != null)
        {
            hardObj.SetActive(true);
            hardNum.text = songComponent.Hard.Value.difficultyNum.ToString();
        }
        if (songComponent.Expert != null)
        {
            expertObj.SetActive(true);
            expertNum.text = songComponent.Expert.Value.difficultyNum.ToString();
        }
        songName.text = songComponent.songData.songName;
        Color _color = NoDataColor.color;
        switch (songComponent.songDifficulty)
        {
            case SongDifficulty.Easy:
                _color = easyColor.color;
                break;
            case SongDifficulty.Normal:
                _color = normalColor.color;
                break;
            case SongDifficulty.Hard:
                _color = hardColor.color;
                break;
            case SongDifficulty.Expert:
                _color = expertColor.color;
                break;
        }
        backGroundRender.material.color = _color;
    }
}
