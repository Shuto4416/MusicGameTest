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
        [SerializeField] private NotesManager _notesManager;
        private NotesLoader _notesLoader => _model.NotesLoader;
        [SerializeField] private int _maxNoteNum = 100; // 最大ノーツ数
        [SerializeField] private int _defaultNoteNum = 60; // 初期ノーツ数
        [SerializeField] private float _noteSpeed;
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

        void StartGame()
        {
            Initialize(destroyCancellationToken).Forget();
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
            }
            
        }

        private void SelectSongManualUpdate()
        {
            DifficultyUp();
            DifficultyDown();
            NextSong();
            PrevSong();
            EnterSong();
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
                foreach (BaseNote note in _notes)
                {
                    note.ManualUpdate(_noteSpeed * sofLan / 100);
                }
                TimeManager.instance.ManualUpdate();
                inputProvider.ManualUpdate();
                foreach (Lights.Light light in _view.Lights)
                {
                    light.ManualUpdate();
                }
            }
            if (SoundManager.instance._audioSourceBGM.isPlaying == false)
                {
                    gameState = GameState.Wait;
                    SelectSongInitialize();
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
            };
        }





        void Generate()
        {
            if (CurrentNoteNum < _notesLoader.NoteNum)
            {
                _notesManager.Generate(CurrentNoteNum, _noteSpeed, _notesLoader.NotesDatas);
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


        async UniTaskVoid Initialize(CancellationToken ct)
        {
            SoundManager.instance.PlayClip(currentSong.Value.audioClip);
            await UniTask.Delay(450, cancellationToken: ct);
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
            // Debug.Log($"NoteNum: {_notesManager.noteNum} , NoteData: {_notesManager.LaneNum[0]} , {_notesManager.NotesTime[0] }, {_notesManager.NoteSoftLanding[0] }");
            // for (int i = 0; i < _notesManager.noteNum; i++)
            // {
            //     _notesData.Add((_notesManager.LaneNum[i], _notesManager.NotesTime[i], _notesManager.NoteSoftLanding[i]));
            // }
            inputProvider.Initialize();
            InputBind();
            //await UniTask.WaitUntil(() => SoundManager.instance.BGM_STATE == BGM_STATE.NOW_PLAY);
            TimeManager.instance.ResetTime();
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

            float NearestTime = float.MaxValue;
            int NearestTimeNoteNum = -1;
            int num;
            int MaxNum = CurrentNoteNum + 1;
            if (CurrentNoteNum - _defaultNoteNum < 0) num = 0;
            else if (CurrentNoteNum >= _notesLoader.NotesDatas.Count) num = MaxNum = _notesLoader.NotesDatas.Count;
            else num = CurrentNoteNum - _defaultNoteNum;
            for (int i = num; i <= MaxNum; i++)
            {
                try
                {
                    if (_notesLoader.NotesDatas[i].laneNum != laneNum) continue;
                    float time = _notesLoader.NotesDatas[i].noteAbsTime - TimeManager.instance.CurrentTime;
                    if (SearchNote(i, laneNum).IsPushed && isExcludePushedNote) continue;
                    if (Math.Abs(time) < NearestTime)
                    {
                        NearestTime = time;
                        NearestTimeNoteNum = i;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error accessing note data at index {i}: {e.Message}");
                    continue;
                }
            }
            return NearestTimeNoteNum;
        }

        BaseNote SearchNote(int noteNum, int laneNum)
        {
            foreach (var _note in _notesManager.UsingNotesObjDatas[laneNum])
                if (_note.LifeSpan == _notesLoader.NotesDatas[noteNum].noteAbsTime)
                    return _note;
            return null;
        }

        void NoteJudge(int laneNum)
        {
            float NearestTime;
            int NearestTimeNoteNum = NearestNoteNum(laneNum, true);
            if (NearestTimeNoteNum == -1) return;
            Notes.Note note;
            Notes.LongNote longNote;
            NearestTime = Math.Abs(_notesLoader.NotesDatas[NearestTimeNoteNum].noteAbsTime - TimeManager.instance.CurrentTime);
            // 普通のノーツの時
            if (_notesLoader.NotesDatas[NearestTimeNoteNum].noteType == 1)
            {
                note = SearchNote(NearestTimeNoteNum, laneNum)?.GetComponent<Notes.Note>();
                if (note == null) return;
                if (NearestTime < 1f / 60f * 4.5f)
                {
                    note.JudgeDisplay(NotesJudgeState.Perfect);
                    note.Push();
                }
                else if (NearestTime < 1f / 60f * 8.5f)
                {
                    note.JudgeDisplay(NotesJudgeState.Great);
                    note.Push();
                }
                else if (NearestTime < 1f / 60f * 13.5f)
                {
                    note.JudgeDisplay(NotesJudgeState.Bad);
                    note.Push();
                }
            }
            // ロングノーツの時
            else if (_notesLoader.NotesDatas[NearestTimeNoteNum].noteType == 2)
            {
                longNote = SearchNote(NearestTimeNoteNum, laneNum)?.GetComponent<LongNote>();
                if (longNote == null) return;
                if (longNote.IsPushed) return;
                if (NearestTime < 1f / 60f * 4.5f)
                {
                    longNote.JudgeDisplay(NotesJudgeState.Perfect);
                    longNote.Push();
                }
                else if (NearestTime < 1f / 60f * 8.5f)
                {
                    longNote.JudgeDisplay(NotesJudgeState.Great);
                    longNote.Push();
                }
                else if (NearestTime < 1f / 60f * 13.5f)
                {
                    longNote.JudgeDisplay(NotesJudgeState.Bad);
                    longNote.BadPush();
                }
            }

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

