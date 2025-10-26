using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileLoaderTest : MonoBehaviour
{
    private const string _defaultPath = "Test";
    // Start is called before the first frame update
    public void LoadFile()
    {
        string testPath = null;
        string path = Application.dataPath + "/" + _defaultPath;
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo fileInfo = new FileInfo(path);
        foreach (var directory in directoryInfo.GetDirectories())
        {
            var files = directory.GetFiles("*.png");
            foreach (var file in files)
            {
                Debug.Log($"{file.FullName}");
                testPath = file.FullName;
                Debug.Log($"{testPath}");
            }
        }
        FileStream fs = new FileStream(@testPath, FileMode.Open);
        BinaryReader bin = new BinaryReader(fs);
        byte[] result = bin.ReadBytes((int)bin.BaseStream.Length);
        bin.Close();

        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(result);

        GameObject tmpQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        tmpQuad.transform.position = new Vector3(5, 5, 0);
        tmpQuad.transform.rotation = Quaternion.Euler(0, 0, 0);
        tmpQuad.transform.localScale = new Vector3(0.001f*tex.width, 0.001f*tex.height, 1);
        tmpQuad.GetComponent<Renderer>().material.mainTexture = tex;
    }

    void Start()
    {
        LoadFile();
    }
}
