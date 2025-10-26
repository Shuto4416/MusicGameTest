using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SongDatas;

namespace SongDatas {

    [Serializable]
    public class SongData
    {
        public string songName;
    }

    public enum SongDifficulty
    {
        Easy,
        Normal,
        Hard,
        Expert
    }

    public class SongComponent
    {
        public LinkedListNode<SongComponent> Easy;
        public LinkedListNode<SongComponent> Normal;
        public LinkedListNode<SongComponent> Hard;
        public LinkedListNode<SongComponent> Expert;
        public SongData songData;
        public byte[] textureData;
        public AudioClip audioClip;
        public int? difficultyNum;
        public SongDifficulty songDifficulty;
        public string beatMapData;
        public void SetListNode(LinkedListNode<SongComponent> linkedListNode, SongDifficulty songDifficulty)
        {
            switch (songDifficulty)
            {
                case SongDifficulty.Easy:
                    Easy = linkedListNode;
                    break;
                case SongDifficulty.Normal:
                    Normal = linkedListNode;
                    break;
                case SongDifficulty.Hard:
                    Hard = linkedListNode;
                    break;
                case SongDifficulty.Expert:
                    Expert = linkedListNode;
                    break;
            }
        }
        public SongComponent(SongData songData, byte[] textureData, AudioClip audioClip, SongDifficulty songDifficulty, int? difficultyNum, string beatMapData, LinkedListNode<SongComponent> Easy = null, LinkedListNode<SongComponent> Normal = null, LinkedListNode<SongComponent> Hard = null, LinkedListNode<SongComponent> Expert = null)
        {
            this.songData = songData;
            this.textureData = textureData;
            this.audioClip = audioClip;
            this.songDifficulty = songDifficulty;
            this.difficultyNum = difficultyNum;
            this.beatMapData = beatMapData;
            this.Easy = Easy;
            this.Normal = Normal;
            this.Hard = Hard;
            this.Expert = Expert;
        }
    }
}
namespace Interface.FileLoader
{
    public interface IFileLoader
    {
        public void ReadFile();
        public List<LinkedList<SongComponent>> GenerateSongComponents();
    }
}
