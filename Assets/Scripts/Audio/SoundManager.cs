using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheSingleton;

namespace Audio {
    
    public enum BGMFile {
        Silence,
        GameScene,
        Result
    }
    public enum SEFile {
        GetItem,
        Damage,
        AddScore,
        MoveScene
    }
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioSource _audioSourceBGM;
        public AudioClip[] _audioClipsBGM;
        public AudioSource _audioSourceSE;
        public AudioClip[] _audioClipsSE;

        public void PlayBGM(BGMFile fileName)
        {
            _audioSourceBGM.Stop();
            switch (fileName)
            {
                default:
                case BGMFile.Silence:
                    break;
                case BGMFile.GameScene:
                    _audioSourceBGM.clip = _audioClipsBGM[0];
                    break;
            }
            _audioSourceBGM.Play();
        }

        public void PlaySE(SEFile fileName)
        {
            switch (fileName)
            {
                default:
                case SEFile.MoveScene:
                _audioSourceSE.PlayOneShot(_audioClipsSE[0]);
                break;
            }
        }
    }
}
