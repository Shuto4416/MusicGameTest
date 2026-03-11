using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interface.FileLoader;
using Zenject;

public class InGameModel : MonoBehaviour
{
    [SerializeField] private NotesLoader _notesLoader;
    public NotesLoader NotesLoader => _notesLoader;
    private IFileLoader _fileLoader;
    public IFileLoader FileLoader => _fileLoader;
    //public IFileLoader FileLoader => _fileLoader;
    [Inject]
    void FIleLoaderInjection(IFileLoader fileLoader)
    {
        this._fileLoader = fileLoader;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
