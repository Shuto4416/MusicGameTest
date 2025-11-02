using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interface.FileLoader;
using System.IO;
using System;
using SongDatas;

namespace Classes.FileLoader
{
    public class FileLoader : IFileLoader
    {
        private const string _defaultPath = "SongDatas";

        public List<LinkedList<SongComponent>> GenerateSongComponents()
        {
            List<LinkedList<SongComponent>> songComponents = new List<LinkedList<SongComponent>>();
            for (int i = 0; i < 4; i++)
            {
                songComponents.Add(new LinkedList<SongComponent>());
            }
            string path = Application.dataPath + "/" + _defaultPath;
            byte[] texture = null;
            AudioClip audioClip = null;
            SongData songData = null;
            Debug.Log(path);
            DirectoryInfo directoryInfo = new DirectoryInfo(@path);
            if (directoryInfo.Exists)
            {
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    try
                    {
                        texture = GetFileImage(directory);
                        audioClip = GetFileAudio(directory);
                        songData = JsonUtility.FromJson<SongData>(GetFileJson(directory));
                        (int?, string) easy = GetBeatMap(directory, SongDifficulty.Easy);
                        (int?, string) normal = GetBeatMap(directory, SongDifficulty.Normal);
                        (int?, string) hard = GetBeatMap(directory, SongDifficulty.Hard);
                        (int?, string) expert = GetBeatMap(directory, SongDifficulty.Expert);
                        LinkedListNode<SongComponent> easyNode = null, normalNode = null, hardNode = null, expertNode = null;
                        if (easy.Item1 != null)
                        {
                            Debug.Log("easyAdd");
                            SongComponent Component = new SongComponent(songData: songData, textureData: texture, audioClip: audioClip, songDifficulty: SongDifficulty.Easy, difficultyNum: easy.Item1, beatMapData: easy.Item2);
                            easyNode = AddBeatMap(Component, ref songComponents);
                        }
                        if (normal.Item1 != null)
                        {
                            Debug.Log("normalAdd");
                            SongComponent Component = new SongComponent(songData: songData, textureData: texture, audioClip: audioClip, songDifficulty: SongDifficulty.Normal, difficultyNum: normal.Item1, beatMapData: normal.Item2);
                            normalNode = AddBeatMap(Component, ref songComponents);
                        }
                        if (hard.Item1 != null)
                        {
                            Debug.Log("hardAdd");
                            SongComponent Component = new SongComponent(songData: songData, textureData: texture, audioClip: audioClip, songDifficulty: SongDifficulty.Hard, difficultyNum: hard.Item1, beatMapData: hard.Item2);
                            hardNode = AddBeatMap(Component, ref songComponents);
                        }
                        if (expert.Item1 != null)
                        {
                            Debug.Log("ExpertAdd");
                            SongComponent Component = new SongComponent(songData: songData, textureData: texture, audioClip: audioClip, songDifficulty: SongDifficulty.Expert, difficultyNum: expert.Item1, beatMapData: expert.Item2);
                            expertNode = AddBeatMap(Component, ref songComponents);
                        }
                        SongComponentNodeSetAll(ref easyNode, ref normalNode, ref hardNode, ref expertNode);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"FileLoader: Error: {e}");
                    }
                }
            }
            return songComponents;
        }

        public void SongComponentNodeSetAll(ref LinkedListNode<SongComponent> easyNode,
                                            ref LinkedListNode<SongComponent> normalNode,
                                            ref LinkedListNode<SongComponent> hardNode,
                                            ref LinkedListNode<SongComponent> expertNode)
        {
            var _easyNode = easyNode;
            var _normalNode = normalNode;
            var _hardNode = hardNode;
            var _expertNode = expertNode;
            void NodeSet(ref LinkedListNode<SongComponent> node)
            {
                if (node != null)
                {
                    node.Value.SetListNode(_easyNode, SongDifficulty.Easy);
                    node.Value.SetListNode(_normalNode, SongDifficulty.Normal);
                    node.Value.SetListNode(_hardNode, SongDifficulty.Hard);
                    node.Value.SetListNode(_expertNode, SongDifficulty.Expert);
                }
            }
            ref LinkedListNode<SongComponent> linkedListNode = ref easyNode;
            NodeSet(ref linkedListNode);
            linkedListNode = ref normalNode;
            NodeSet(ref linkedListNode);
            linkedListNode = ref hardNode;
            NodeSet(ref linkedListNode);
            linkedListNode = ref expertNode;
            NodeSet(ref linkedListNode);
        }

        public LinkedListNode<SongComponent> AddBeatMap(SongComponent songComponent, ref List<LinkedList<SongComponent>> songComponents)
        {
            int num = -1;
            int count;
            switch (songComponent.songDifficulty)
            {
                case SongDifficulty.Easy:
                    num = 0;
                    break;
                case SongDifficulty.Normal:
                    num = 1;
                    break;
                case SongDifficulty.Hard:
                    num = 2;
                    break;
                case SongDifficulty.Expert:
                    num = 3;
                    break;
            }
            if (num == -1)
            {
                Debug.LogError("num = -1");
            }
            try
            {
                Debug.Log(songComponents[num].Count);
                count = songComponents[num].Count;
            }
            catch (Exception e)
            {
                Debug.LogError($"songComponents[num].Count << Error! : {e}");
                count = 0;
            }
            if (count == 0)
            {
                Debug.Log($"songComponents[{num}].Count == 0");
                try
                {
                    songComponents[num].AddFirst(songComponent);
                    return songComponents[num].First;
                }
                catch (Exception e)
                {
                    Debug.LogError($"FailedAddList: {e}");
                }
            }
            LinkedListNode<SongComponent> linkedListNode = null;
            try
            {
                linkedListNode = songComponents[num].First;
            }
            catch (Exception e)
            {
                songComponents[num].AddFirst(songComponent);
                Debug.LogError($"FailedLinkedListNode = songComponents[num].First : {e}");
                return songComponents[num].First;
            }
            try
            {
                while (songComponent.difficultyNum < linkedListNode.Value.difficultyNum)
                {
                    try
                    {
                        linkedListNode = linkedListNode.Next;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"AddBeatMap: Error :{e}");
                        songComponents[num].AddLast(songComponent);
                        return songComponents[num].Last;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"AddBeatMapWhile: Error :{e}");
                songComponents[num].AddFirst(songComponent);
                return songComponents[num].First;
            }
            songComponents[num].AddBefore(linkedListNode, songComponent);
            return linkedListNode.Previous;
        }

        public void ReadFile()
        {
            string path = Application.dataPath + "/" + _defaultPath;
            byte[] texture = null;
            Debug.Log(path);
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo fileInfo = new FileInfo(path);
            if (directoryInfo.Exists)
            {
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    texture = GetFileImage(directory);
                }
            }
        }

        private byte[] GetFileImage(DirectoryInfo directoryInfo)
        {
            string path = null;
            var files = directoryInfo.GetFiles("*.png");
            foreach (var file in files)
            {
                path = file.FullName;
            }
            FileStream fs = new FileStream(@path, FileMode.Open);
            BinaryReader bin = new BinaryReader(fs);
            byte[] result = bin.ReadBytes((int)bin.BaseStream.Length);
            bin.Close();
            return result;
        }

        private AudioClip GetFileAudio(DirectoryInfo directoryInfo)
        {
            string path = null;
            var files = GetFiles(directoryInfo, ".mp3|.wav");
            if (files == null) throw new Exception("音源がありません");
            foreach (var file in files)
            {
                path = file.FullName;
            }
            WWW request = new WWW(@path);
            AudioClip audioClip = request.GetAudioClip(false, true);
            return audioClip;
        }

        private string GetFileJson(DirectoryInfo directoryInfo)
        {
            string path = null;
            var files = directoryInfo.GetFiles("*.json");
            foreach (var file in files)
            {
                path = file.FullName;
            }
            if(path == null) throw new Exception($"Error!フォルダ内に.jsonファイルが含まれていません:{directoryInfo.FullName}");
            using (var sr = new StreamReader(@path, System.Text.Encoding.UTF8))
            {
                string filedata = sr.ReadToEnd();
                return filedata;
            }
        }

        private (int?, string) GetBeatMap(DirectoryInfo directoryInfo, SongDifficulty difficulty)
        {
            foreach (var directory in directoryInfo.GetDirectories())
            {
                var directoryName = directory.Name.ToLower().Split("_");
                foreach (var str in directoryName)
                {
                    Debug.Log(str);
                }
                string difficultyStr = null;
                if (difficulty == SongDifficulty.Easy) difficultyStr = "easy";
                if (difficulty == SongDifficulty.Normal) difficultyStr = "normal";
                if (difficulty == SongDifficulty.Hard) difficultyStr = "hard";
                if (difficulty == SongDifficulty.Expert)difficultyStr = "expert";
                Debug.Log(difficultyStr.ToLower());
                if (difficultyStr.ToLower() == directoryName[0])
                {
                    if (directoryName[1] == null)
                    {
                        throw new Exception($"Error!フォルダ名に難易度の数値がありません:{directory.FullName}");
                    }
                    string jsonFile = GetFileJson(directory);
                    return (int.Parse(directoryName[1]), jsonFile);
                }
            }
            return (null, null);
        }

        private FileInfo[] GetFiles(DirectoryInfo directoryInfo, string ext) {
        FileInfo[] files = directoryInfo.GetFiles();
        ext = ext.ToLower();
        string[] exts = ext.Split('|');
        FileInfo[] filtered =
            Array.FindAll(files,
                delegate(FileInfo file) {
                    return Array.IndexOf(exts, Path.GetExtension(file.FullName).ToLower()) >= 0;
                });
        return filtered;
    }
    }
}
