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
    public enum BGM_STATE
    {
        WAIT,
        FADE_IN,
        NOW_PLAY,
        FADE_OUT,
        FADE_STOP,
        PAUSE,
        END,
    }
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioSource _audioSourceBGM;
        public AudioClip[] _audioClipsBGM;
        public AudioSource _audioSourceSE;
        public AudioClip[] _audioClipsSE;
        private BGM_STATE bGM_STATE;
        public BGM_STATE BGM_STATE => BGM_STATE;
        private AudioClip readAudioClip;

        public void PlayBGM(BGMFile fileName)
        {
            _audioSourceBGM.Stop();
            switch (fileName)
            {
                default:
                case BGMFile.Silence:
                    break;
            }
            _audioSourceBGM.Play();
            bGM_STATE = BGM_STATE.NOW_PLAY;
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

        public void ManualUpdate()
        {
            switch (bGM_STATE)
            {
                case BGM_STATE.NOW_PLAY:
                    if (!_audioSourceBGM.isPlaying) bGM_STATE = BGM_STATE.END;
                    break;
            }
        }

        public void Initialize()
        {
            bGM_STATE = BGM_STATE.WAIT;
        }

        public void PlayClip(AudioClip audioClip)
        {
            _audioSourceBGM.Stop();
            readAudioClip = audioClip;
            _audioSourceBGM.clip = audioClip;
            _audioSourceBGM.Play();
            bGM_STATE = BGM_STATE.NOW_PLAY;
        }

        public void SoundPause()
        {
            bGM_STATE = BGM_STATE.PAUSE;
            _audioSourceBGM.Pause();
        }

        public void SoundUnPause()
        {
            bGM_STATE = BGM_STATE.NOW_PLAY;
            _audioSourceBGM.UnPause();
        }

        public void StopSoundNow()
        {
            bGM_STATE = BGM_STATE.END;
            _audioSourceBGM.Stop();
        }
    }
}
