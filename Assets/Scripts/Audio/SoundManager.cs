using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheSingleton;
using DG.Tweening;

namespace Audio {
    
    public enum BGMFile {
        Silence,
        GameScene,
        Result
    }
    public enum SEFile {
        Tap,
        MoveScene,
        AddScore,
    }
    public enum BGM_STATE
    {
        WAIT,
        NOW_PLAY,
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
        public BGM_STATE BGM_STATE => bGM_STATE;
        private AudioClip readAudioClip;
        private Sequence sequence;

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
                case SEFile.Tap:
                    _audioSourceSE.PlayOneShot(_audioClipsSE[0]);
                    break;
                case SEFile.AddScore:
                    _audioSourceSE.PlayOneShot(_audioClipsSE[1]);
                    break;
                case SEFile.MoveScene:
                    _audioSourceSE.PlayOneShot(_audioClipsSE[2]);
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
            sequence.Complete();
            _audioSourceBGM.volume = 1f;
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

        public void PlayOneShotClip(AudioClip audioClip)
        {
            sequence.Complete();
            _audioSourceBGM.volume = 1f;
            _audioSourceBGM.Stop();
            if (audioClip == null) return;
            sequence = DOTween.Sequence();
            if (audioClip.length > 1f) sequence.AppendInterval(audioClip.length - 1f);
            sequence.Append(_audioSourceBGM.DOFade(0f, 1f).SetEase(Ease.Linear));
            readAudioClip = audioClip;
            _audioSourceBGM.clip = audioClip;
            _audioSourceBGM.Play();
            sequence.Play();
        }

        public void StopOneShot()
        {
            sequence.Complete();
            _audioSourceBGM.volume = 1f;
            _audioSourceBGM.Stop();
        }

        public void PlayClipScheduled(AudioClip audioClip, double time)
        {
            sequence.Complete();
            _audioSourceBGM.volume = 1f;
            _audioSourceBGM.Stop();
            readAudioClip = audioClip;
            _audioSourceBGM.clip = audioClip;
            _audioSourceBGM.PlayScheduled(time);
            bGM_STATE = BGM_STATE.NOW_PLAY;
        }

        public void RePlayClipScheduled(AudioClip audioClip, double time, double playPosition)
        {
            readAudioClip = audioClip;
            _audioSourceBGM.clip = audioClip;
            _audioSourceBGM.time = (float)playPosition;
            _audioSourceBGM.PlayScheduled(time);
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
