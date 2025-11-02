using System.Collections;
using System.Collections.Generic;
using Notes;
using InputSystem;
using Lights;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using Zenject;
using System;
using Cysharp.Threading.Tasks;
using TheSingleton;
using System.ComponentModel.Design;
using SongDatas;
using R3.Triggers;
using Audio;
using UnityEngine.Rendering;
using System.Threading;
using Classes.NotesManager;


public enum GameState
{
    Wait,
    SelectSong,
    Loading,
    PlaySong,
    Result
}

namespace InGame
{
    public class InGamePresenter : MonoBehaviour
    {
        [SerializeField] private InGameModel _model;
        [SerializeField] private InGameView _view;
        private Lights.Light[] _lights => _view.Lights;
        [SerializeField]
        private NotesManager _notesManager;
        private NotesLoader _notesLoader => _model.NotesLoader;
        [SerializeField]
        private int _maxNoteNum = 100; // 最大ノーツ数
        [SerializeField]
        private int _defaultNoteNum = 60; // 初期ノーツ数
        [SerializeField]
        private ComboDisplayPresenter _comboDisplayPresenter;
        [SerializeField]
        private NoteSpeedDisplayPresenter _noteSpeedDisplayPresenter;
        private IInputProvider inputProvider;
        private ISelectInputProvider selectInputProvider;
        private List<BaseNote> _notes = new List<BaseNote>();
        private string songName = "01 - Re_Unknown X";
        private int CurrentNoteNum;
        private float sofLan;
        private List<LinkedList<SongComponent>> songComponents;
        private LinkedListNode<SongComponent> currentSong;
        private SongDifficulty currentSongDifficulty;
        private GameState gameState = GameState.Wait;
        private double startTime;
        private int _perfectNum;
        private int _greatNum;
        private int _badNum;
        private int _missNum;
        private int[] _notesSpeeds = { 400, 600, 800, 1000};
        private int _speedSelector = 0;

        [Inject]
        void Injection(IInputProvider inputProvider)
        {
            this.inputProvider = inputProvider;
        }
        [Inject]
        void SelectInjection(ISelectInputProvider selectInputProvider)
        {
            this.selectInputProvider = selectInputProvider;
        }
        void Start()
        {
            _noteSpeedDisplayPresenter.Initialize();
            _noteSpeedDisplayPresenter.Model.Set(_notesSpeeds[_speedSelector]);
            songComponents = _model.FileLoader.GenerateSongComponents();
            try
            {
                currentSong = songComponents[0].First;
            }
            catch
            {
                currentSong.Value = null;
            }
            currentSongDifficulty = SongDifficulty.Easy;
            for (int i = 0; i < _maxNoteNum; i++)
            {
                PrepareNote();
            }
            SelectSongInitialize();
        }

        void SelectSongInitialize()
        {
            _view.SongFrameManager.Show();
            _view.SongFrameManager.ChangeSong(true, isCurrentSongNull(currentSong));
            gameState = GameState.SelectSong;
        }

        async UniTask ResultInitialize(CancellationToken token)
        {
            _view.result.Initialize();
            await _view.result.Show(_perfectNum, _greatNum, _badNum, _missNum, _notesLoader.NotesDatas.Count, token);
            gameState = GameState.Result;
        }

        void StartGame()
        {
            MusicGameInitialize();
        }

        SongComponent isCurrentSongNull(LinkedListNode<SongComponent> linkedListNode)
        {
            try
            {
                if (linkedListNode.Value != null)
                    return linkedListNode.Value;
            }
            catch
            {
                return null;
            }
            return null;
        }


        void VariableInitialize()
        {
            sofLan = 100;
            CurrentNoteNum = 0;
            _perfectNum = 0;
            _greatNum = 0;
            _badNum = 0;
            _missNum = 0;
        }

        // Update is called once per frame
        void Update()
        {
            SoundManager.instance.ManualUpdate();
            _notesManager.Lock();
            switch (gameState)
            {
                case GameState.SelectSong:
                    SelectSongManualUpdate();
                    break;
                case GameState.PlaySong:
                    PlaySongManualUpdate();
                    break;
                case GameState.Result:
                    ResultManualUpdate();
                    break;
            }

        }
        private void ResultManualUpdate()
        {
            var sIP = selectInputProvider;
            if (sIP.Up() || sIP.Down() || sIP.Right() || sIP.Left() || sIP.DifficultyUp() || sIP.DifficultyDown())
            {
                _view.result.Initialize();
                gameState = GameState.Wait;
                SelectSongInitialize();
            }
        }

        private void SelectSongManualUpdate()
        {
            DifficultyUp();
            DifficultyDown();
            NextSong();
            PrevSong();
            EnterSong();
            SpeedUp();
            SpeedDown();
        }

        private int RotSelector(int i)
        {
            if (i < 0) return 3;
            if (i > 3) return 0;
            return i;
        }

        private void SpeedUp()
        {
            if (selectInputProvider.Up())
            {
                _speedSelector = RotSelector(++_speedSelector);
                _noteSpeedDisplayPresenter.Model.Set(_notesSpeeds[_speedSelector]);
            }
        }

        private void SpeedDown()
        {
            if (selectInputProvider.Down())
            {
                _speedSelector = RotSelector(--_speedSelector);
                _noteSpeedDisplayPresenter.Model.Set(_notesSpeeds[_speedSelector]);
            }
        }

        private void DifficultyUp()
        {
            if (selectInputProvider.DifficultyUp())
            {
                currentSongDifficulty = AddDifficulty(currentSongDifficulty);
                DifficultyChange(currentSongDifficulty);
                _view.SongFrameManager.ChangeSong(true, isCurrentSongNull(currentSong));
            }
        }

        private void DifficultyDown()
        {
            if (selectInputProvider.DifficultyDown())
            {
                currentSongDifficulty = ReduceDifficulty(currentSongDifficulty);
                DifficultyChange(currentSongDifficulty);
                _view.SongFrameManager.ChangeSong(true, isCurrentSongNull(currentSong));
            }
        }

        private void NextSong()
        {
            if (selectInputProvider.Right())
            {
                try
                {
                    currentSong = currentSong.Next;
                }
                catch
                {
                    try
                    {
                        currentSong = songComponents[SongDifficultyToInt(currentSongDifficulty)].First;
                    }
                    catch
                    {
                        currentSong.Value = null;
                    }
                }
                _view.SongFrameManager.ChangeSong(true, isCurrentSongNull(currentSong));
            }
        }

        private void PrevSong()
        {
            if (selectInputProvider.Left())
            {
                try
                {
                    currentSong = currentSong.Previous;
                }
                catch
                {
                    try
                    {
                        currentSong = songComponents[SongDifficultyToInt(currentSongDifficulty)].Last;
                    }
                    catch
                    {
                        currentSong.Value = null;
                    }
                }
                _view.SongFrameManager.ChangeSong(true, isCurrentSongNull(currentSong));
            }
        }

        private void EnterSong()
        {
            if (selectInputProvider.Enter())
            {
                if (currentSong.Value != null)
                {
                    SoundManager.instance.PlaySE(SEFile.MoveScene);
                    _view.SongFrameManager.Hide();
                    gameState = GameState.Loading;
                    StartGame();
                }
            }
        }

        private int SongDifficultyToInt(SongDifficulty songDifficulty)
        {
            switch (songDifficulty)
            {
                case SongDifficulty.Easy:
                    return 0;
                case SongDifficulty.Normal:
                    return 1;
                case SongDifficulty.Hard:
                    return 2;
                default:
                    return 3;
            }
        }

        private void DifficultyChange(SongDifficulty difficulty)
        {
            switch (difficulty)
            {
                case SongDifficulty.Easy:
                    try
                    {
                        if (currentSong.Value.Easy != null)
                        {
                            currentSong = currentSong.Value.Easy;
                            break;
                        }
                    }
                    catch
                    {
                        try
                        {
                            currentSong = songComponents[0].First;
                        }
                        catch
                        {
                            currentSong.Value = null;
                        }
                    }
                    break;
                case SongDifficulty.Normal:
                    try
                    {
                        if (currentSong.Value.Normal != null)
                        {
                            currentSong = currentSong.Value.Normal;
                            break;
                        }
                    }
                    catch
                    {
                        try
                        {
                            currentSong = songComponents[1].First;
                        }
                        catch
                        {
                            currentSong.Value = null;
                        }
                    }
                    break;
                case SongDifficulty.Hard:
                    try
                    {
                        if (currentSong.Value.Hard != null)
                        {
                            currentSong = currentSong.Value.Hard;
                            break;
                        }
                    }
                    catch
                    {
                        try
                        {
                            currentSong = songComponents[2].First;
                        }
                        catch
                        {
                            currentSong.Value = null;
                        }
                    }
                    break;
                case SongDifficulty.Expert:
                    try
                    {
                        if (currentSong.Value.Expert != null)
                        {
                            currentSong = currentSong.Value.Expert;
                            break;
                        }
                    }
                    catch
                    {
                        try
                        {
                            currentSong = songComponents[3].First;
                        }
                        catch
                        {
                            currentSong.Value = null;
                        }
                    }
                    break;
            }
        }


        private SongDifficulty AddDifficulty(SongDifficulty songDifficulty)
        {
            switch (songDifficulty)
            {
                case SongDifficulty.Easy:
                    return SongDifficulty.Normal;
                case SongDifficulty.Normal:
                    return SongDifficulty.Hard;
                case SongDifficulty.Hard:
                    return SongDifficulty.Expert;
                case SongDifficulty.Expert:
                    return SongDifficulty.Easy;
                default:
                    return SongDifficulty.Easy;
            }
        }

        private SongDifficulty ReduceDifficulty(SongDifficulty songDifficulty)
        {
            switch (songDifficulty)
            {
                case SongDifficulty.Easy:
                    return SongDifficulty.Expert;
                case SongDifficulty.Normal:
                    return SongDifficulty.Easy;
                case SongDifficulty.Hard:
                    return SongDifficulty.Normal;
                case SongDifficulty.Expert:
                    return SongDifficulty.Hard;
                default:
                    return SongDifficulty.Easy;
            }
        }

        private void PlaySongManualUpdate()
        {

            if (SoundManager.instance._audioSourceBGM.isPlaying)
            {
                TimeManager.instance.SetTime((float)(AudioSettings.dspTime - startTime));
                var time = TimeManager.instance.CurrentTime;
                if (TimeManager.instance.CurrentTime > 0)
                {
                    // Debug.Log($"Time: {TimeManager.instance.CurrentTime}");
                    foreach (BaseNote note in _notes)
                    {
                        note.ManualUpdate((_noteSpeedDisplayPresenter.Model.NoteSpeed / 60f) * (sofLan / 100f) * time/*TimeManager.instance.CurrentTime*/);
                    }
                    // TimeManager.instance.ManualUpdate();
                    inputProvider.ManualUpdate();
                    foreach (Lights.Light light in _view.Lights)
                    {
                        light.ManualUpdate();
                    }
                }
            }
            if (SoundManager.instance.BGM_STATE == BGM_STATE.END)
            {
                gameState = GameState.Wait;
                ResultInitialize(default).Forget();
            }
        }


        void PrepareNote()
        {
            BaseNote baseNote = _notesManager.Create();
            BaseNote longNote = _notesManager.LongNoteCreate();
            Notes.Note note = baseNote.GetComponent<Notes.Note>();
            Notes.LongNote LongNote = longNote.GetComponent<Notes.LongNote>();
            _notes.Add(note);
            _notes.Add(LongNote);
            BindNotes(note);
            BindNotes(LongNote);

        }
        void BindNotes(BaseNote note)
        {
            note.OnClearEvent += () =>
            {
                int laneNum = note.LaneNum;
                Debug.Log($"{note.LaneNum}");
                _notesManager.Pop(note.LaneNum, note.NoteType);
                Generate();
                Debug.Log("Note cleared and created new note.");
            };
            note.OnSofLanEvent += (_) =>
            {
                sofLan = _;
            };
            note.JudgeDisplayEvent += (_) =>
            {
                _view.CharacterDispaly.NotesJudgePlay(_);
                switch (_)
                {
                    case NotesJudgeState.Perfect:
                        _comboDisplayPresenter.Model.Set(_comboDisplayPresenter.Model.Combo + 1);
                        _perfectNum++;
                        break;
                    case NotesJudgeState.Great:
                        _comboDisplayPresenter.Model.Set(_comboDisplayPresenter.Model.Combo + 1);
                        _greatNum++;
                        break;
                    case NotesJudgeState.Bad:
                        _comboDisplayPresenter.Model.Set(0);
                        _badNum++;
                        break;
                    case NotesJudgeState.Miss:
                        _comboDisplayPresenter.Model.Set(0);
                        _missNum++;
                        break;
                }
            };
        }





        void Generate()
        {
            if (CurrentNoteNum < _notesLoader.NotesDatas.Count)
            {
                _notesManager.Generate(CurrentNoteNum, _noteSpeedDisplayPresenter.Model.NoteSpeed, _notesLoader.NotesDatas);
                if (_notesLoader.NotesDatas[CurrentNoteNum].noteType == 2) CurrentNoteNum++;
                CurrentNoteNum++;
                Debug.Log($"Note created: {CurrentNoteNum}");
            }
        }

        void Load(string jsonData)
        {
            _notesLoader.Initialize(jsonData);
            _notesManager.Initialize(_notesLoader.MaxBlock);
            Debug.Log($"NotesNum: {_notesLoader.NoteNum}");
        }

        void InputBind()
        {
            BindPush();
        }


        void MusicGameInitialize()
        {
            _comboDisplayPresenter.Initialize();
            TimeManager.instance.ResetTime();
            startTime = AudioSettings.dspTime + 1.0f;
            SoundManager.instance.PlayClipScheduled(currentSong.Value.audioClip, startTime);
            TimeManager.instance.SetTime((float)(AudioSettings.dspTime - startTime));
            // SoundManager.instance.PlayClip(currentSong.Value.audioClip);
            // await UniTask.Delay(450, cancellationToken: ct);
            VariableInitialize();
            foreach (var light in _lights)
            {
                light.Initialize();
            }
            Load(currentSong.Value.beatMapData);
            for (int i = 0; i < _defaultNoteNum; i++)
            {
                Generate();
            }
            inputProvider.Initialize();
            // Debug.Log($"NoteNum: {_notesManager.noteNum} , NoteData: {_notesManager.LaneNum[0]} , {_notesManager.NotesTime[0] }, {_notesManager.NoteSoftLanding[0] }");
            // for (int i = 0; i < _notesManager.noteNum; i++)
            // {
            //     _notesData.Add((_notesManager.LaneNum[i], _notesManager.NotesTime[i], _notesManager.NoteSoftLanding[i]));
            // }
            InputBind();
            //await UniTask.WaitUntil(() => SoundManager.instance.BGM_STATE == BGM_STATE.NOW_PLAY);
            gameState = GameState.PlaySong;
        }

        void BindPush()
        {
            Debug.Log("Push");

            // inputProvider.SubscriptForKeyDownOnceAction(0, () => NoteJudge(0));
            // inputProvider.SubscriptForKeyDownOnceAction(1, () => NoteJudge(1));
            // inputProvider.SubscriptForKeyDownOnceAction(2, () => NoteJudge(2));
            // inputProvider.SubscriptForKeyDownOnceAction(3, () => NoteJudge(3));
            // inputProvider.SubscriptForKeyDownOnceAction(4, () => NoteJudge(4));
            // inputProvider.SubscriptForKeyDownOnceAction(5, () => NoteJudge(5));
            // inputProvider.SubscriptForKeyDownAction(0, () => LongNoteJudge(0));
            // inputProvider.SubscriptForKeyDownAction(1, () => LongNoteJudge(1));
            // inputProvider.SubscriptForKeyDownAction(2, () => LongNoteJudge(2));
            // inputProvider.SubscriptForKeyDownAction(3, () => LongNoteJudge(3));
            // inputProvider.SubscriptForKeyDownAction(4, () => LongNoteJudge(4));
            // inputProvider.SubscriptForKeyDownAction(5, () => LongNoteJudge(5));
            for (int i = 0; i < _view.Lights.Length; i++)
            {
                int index = i; // ローカル変数にコピー
                inputProvider.SubscriptForKeyDownOnceAction(index, () => NoteJudge(index));
                inputProvider.SubscriptForKeyDownAction(index, () => LongNoteJudge(index));
                inputProvider.SubscriptForKeyDownAction(index, () => _view.Lights[index].ColorChange());
            }


        }


        int NearestNoteNum(int laneNum, bool isExcludePushedNote)
        {
            // var currentTime = TimeManager.instance.CurrentTime;
            float NearestTime = float.MaxValue;
            int NearestTimeNoteNum = -1;
            int num;
            int MaxNum = CurrentNoteNum;
            int processNum = 60;
            num = CurrentNoteNum - processNum;
            if (num < 0)
            {
                num = 0;
            }
            if (CurrentNoteNum >= _notesLoader.NotesDatas.Count)
            {
                num = _notesLoader.NotesDatas.Count - processNum;
                MaxNum = _notesLoader.NotesDatas.Count;
            }
            for (int i = num; i < MaxNum; i++)
            {
                try
                {
                    // Debug.Log($"Note: {i}");
                    if (_notesLoader.NotesDatas[i].laneNum == laneNum)
                    {
                        Debug.Log($"Note: {i} _notesLoader.NotesDatas[i].laneNum == laneNum");
                        float time = _notesLoader.NotesDatas[i].noteAbsTime - TimeManager.instance.CurrentTime;
                        if (SearchNote(i, laneNum).IsPushed && isExcludePushedNote) continue;
                        // Debug.Log($"Note: {i}, Time: {time}");
                        if (Math.Abs(time) < NearestTime)
                        {
                            NearestTime = time;
                            NearestTimeNoteNum = i;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error accessing note data at index {i}: {e.Message}");
                    continue;
                }
            }
            Debug.Log($"ThisNoteNum = {NearestTimeNoteNum}");
            return NearestTimeNoteNum;
        }

        void NearestNoteNumJudge(int laneNum, bool isExcludePushedNote)
        {
            // var currentTime = TimeManager.instance.CurrentTime;
            int NearestTimeNoteNum = -1;
            int num;
            int MaxNum = CurrentNoteNum;
            int processNum = 80;
            num = CurrentNoteNum - processNum;
            if (num < 0)
            {
                num = 0;
            }
            if (CurrentNoteNum >= _notesLoader.NotesDatas.Count)
            {
                num = _notesLoader.NotesDatas.Count - processNum;
                MaxNum = _notesLoader.NotesDatas.Count;
            }
            for (int i = num; i < MaxNum; i++)
            {
                try
                {
                    // Debug.Log($"Note: {i}");
                    if (_notesLoader.NotesDatas[i].laneNum == laneNum)
                    {
                        Debug.Log($"Note: {i} _notesLoader.NotesDatas[i].laneNum == laneNum");
                        float time = _notesLoader.NotesDatas[i].noteAbsTime - TimeManager.instance.CurrentTime;
                        if (SearchNote(i, laneNum).IsPushed && isExcludePushedNote) continue;
                        if (NoteJudgeProcess(i)) return;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error accessing note data at index {i}: {e.Message}");
                    continue;
                }
            }
            Debug.Log($"ThisNoteNum = {NearestTimeNoteNum}");
            return;
        }

        BaseNote SearchNote(int noteNum, int laneNum)
        {
            BaseNote note = null;
            float NearestTime = float.MaxValue;
            foreach (var _note in _notesManager.UsingNotesObjDatas[laneNum])
            {
                float time = Math.Abs(_note.LifeSpan - _notesLoader.NotesDatas[noteNum].noteAbsTime);
                if (_note.LifeSpan == _notesLoader.NotesDatas[noteNum].noteAbsTime)
                    return _note;
                if (time < NearestTime)
                {
                    note = _note;
                }
            }
            return note;
        }

        void NoteJudge(int laneNum)
        {
            NearestNoteNumJudge(laneNum, true);
            // int NearestTimeNoteNum = NearestNoteNum(laneNum, true);
            // if (NearestTimeNoteNum == -1) return;
            // NoteJudgeProcess(NearestTimeNoteNum);

        }

        bool NoteJudgeProcess(int i)
        {
            var laneNum = _notesLoader.NotesDatas[i].laneNum;
            Notes.Note note;
            Notes.LongNote longNote;
            var NearestTime = Math.Abs(_notesLoader.NotesDatas[i].noteAbsTime - TimeManager.instance.CurrentTime);
            // 普通のノーツの時
            if (_notesLoader.NotesDatas[i].noteType == 1)
            {
                note = SearchNote(i, laneNum)?.GetComponent<Notes.Note>();
                if (note == null) return false;
                if (NearestTime < 1f / 60f * 4.5f)
                {
                    // SoundManager.instance.PlaySE(SEFile.Tap);
                    note.JudgeDisplay(NotesJudgeState.Perfect);
                    note.Push();
                    return true;
                }
                else if (NearestTime < 1f / 60f * 8.5f)
                {
                    // SoundManager.instance.PlaySE(SEFile.Tap);
                    note.JudgeDisplay(NotesJudgeState.Great);
                    note.Push();
                    return true;
                }
                else if (NearestTime < 1f / 60f * 13.5f)
                {
                    // SoundManager.instance.PlaySE(SEFile.Tap);
                    note.JudgeDisplay(NotesJudgeState.Bad);
                    note.Push();
                    return true;
                }
                return false;
            }
            // ロングノーツの時
            else if (_notesLoader.NotesDatas[i].noteType == 2)
            {
                longNote = SearchNote(i, laneNum)?.GetComponent<LongNote>();
                if (longNote == null) return false;
                if (longNote.IsPushed) return false;
                if (NearestTime < 1f / 60f * 4.5f)
                {
                    longNote.JudgeDisplay(NotesJudgeState.Perfect);
                    longNote.Push();
                    return true;
                }
                else if (NearestTime < 1f / 60f * 8.5f)
                {
                    longNote.JudgeDisplay(NotesJudgeState.Great);
                    longNote.Push();
                    return true;
                }
                else if (NearestTime < 1f / 60f * 13.5f)
                {
                    longNote.JudgeDisplay(NotesJudgeState.Bad);
                    longNote.BadPush();
                    return true;
                }
                return false;
            }
            return false;
        }

        void LongNoteJudge(int laneNum)
        {
            int NearestTimeNoteNum = NearestNoteNum(laneNum, false);
            if (NearestTimeNoteNum == -1) return;
            Notes.LongNote longNote = null;
            if (_notesLoader.NotesDatas[NearestTimeNoteNum].noteType != 2) return;
            longNote = SearchNote(NearestTimeNoteNum, laneNum)?.GetComponent<LongNote>();
            if (longNote == null) return;
            if (longNote.IsPushed) longNote.Press();
        }
        
        

    }
}

