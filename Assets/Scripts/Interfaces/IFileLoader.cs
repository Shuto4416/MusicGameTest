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
        Expert,
        Secret
    }

    public class SongComponent
    {
        public LinkedListNode<SongComponent> Easy;
        public LinkedListNode<SongComponent> Normal;
        public LinkedListNode<SongComponent> Hard;
        public LinkedListNode<SongComponent> Expert;
        public LinkedListNode<SongComponent> Secret;
        public SongData songData;
        public byte[] textureData;
        public AudioClip audioClip;
        public AudioClip preView;
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
                case SongDifficulty.Secret:
                    Secret = linkedListNode;
                    break;
            }
        }
        public SongComponent(SongData songData, byte[] textureData, AudioClip audioClip, SongDifficulty songDifficulty, int? difficultyNum, string beatMapData, AudioClip preView = null,LinkedListNode<SongComponent> Easy = null, LinkedListNode<SongComponent> Normal = null, LinkedListNode<SongComponent> Hard = null, LinkedListNode<SongComponent> Expert = null, LinkedListNode<SongComponent> Secret = null)
        {
            this.songData = songData;
            this.textureData = textureData;
            this.audioClip = audioClip;
            this.preView = preView;
            this.songDifficulty = songDifficulty;
            this.difficultyNum = difficultyNum;
            this.beatMapData = beatMapData;
            this.Easy = Easy;
            this.Normal = Normal;
            this.Hard = Hard;
            this.Expert = Expert;
            this.Secret = Secret;
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
