using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIT.SamtleGame.Tools;
using MIT.SamtleGame.DesignPattern;
using MIT.SamtleGame.Stage2.NPC;
using MIT.SamtleGame.Stage2;
using TMPro;

public enum DialogueStatus { Start, End }

[System.Serializable]
public struct DialogueEvent
{
    public int _id;
    public int _index;
    public List<DialoguePage> _textPages;

    public string _sound;
    public DialogueStatus _status;

    /// 대화가 끝날시 플레이어 컨트롤 여부
    /// status가 Start인 경우에만 체크
    public bool _isControllable; 
    
    public DialogueEvent(int id, List<DialoguePage> textPages, int index, string sound = "", DialogueStatus status = DialogueStatus.Start, bool isControllable = true)
    {
        _id = id;
        _index = index;
        _textPages = textPages;
        _sound = sound;
        _status = status;
        _isControllable = isControllable;
    }

    static DialogueEvent _event;

    public static void Trigger(int id, List<DialoguePage> textPages, int index, string sound = "", DialogueStatus status = DialogueStatus.Start, bool isControllable = true)
    {
        _event._id = id;
        _event._textPages = textPages;
        _event._index = index;
        _event._sound = sound;
        _event._status = status;
        _event._isControllable = isControllable;
        EventManager.TriggerEvent(_event);
    }
}

public class DialogueManager : Singleton<DialogueManager>, EventListener<DialogueEvent>
{
    private DialogueBox _curretDialogue;

    [Header("현재 대화 UI Prefab")]
    public GameObject _currentDialogueUI; /// 현재 UI 객체
    public GameObject[] _dialogueUIPrefab;


    [Header("대화 끝날시 플레이어 컨트롤 여부")]
    public bool _isControllable;

    public bool _isEnd = true; // 대화가 종료된 상태인지?

    private void DialogueUpdate(int id, List<DialoguePage> textPages, string sound, DialogueStatus status, bool isControllable)
    {
        switch(status)
        {
            case DialogueStatus.Start:
                StartDialogue(id, textPages, sound);
                _isControllable = isControllable;
                break;
            case DialogueStatus.End:
                EndDialogue();
                break;
        }
    }
    #region 대화창 객체 생성
    private void CreateDialogueUI(int id, List<DialoguePage> textPages, string sound)
    {
        Debug.LogFormat("{0} 대화창 생성", id);
        /// 초기화
        if(_currentDialogueUI != null)
            Destroy(_currentDialogueUI);

        /// UI 프리팹을 주 Canvas에 자식으로 붙이는 과정
        _currentDialogueUI = Instantiate(_dialogueUIPrefab[id], this.transform);
        _currentDialogueUI.transform.SetParent(this.gameObject.transform);

        _curretDialogue = _currentDialogueUI.GetComponentInChildren<DialogueBox>();
        _curretDialogue.Reset(id, textPages, 0, sound);
    }
    #endregion

    #region  대화창 시작
    protected virtual void StartDialogue(int id, List<DialoguePage> textPages, string sound)
    {
        _isEnd = false;
        CreateDialogueUI(id, textPages, sound);
    }
    #endregion

    #region  대화 종료
    protected virtual void EndDialogue()
    {
        if(_currentDialogueUI != null)
        {
            _isEnd = true;
            _currentDialogueUI.SetActive(false);
            PlayerControllerEvent.Trigger(_isControllable);
        }
    }
    #endregion

    public virtual void OnEvent(DialogueEvent dialogueEvent)
    {
        DialogueUpdate(dialogueEvent._id, dialogueEvent._textPages, dialogueEvent._sound, dialogueEvent._status, dialogueEvent._isControllable);
    }

    private void OnEnable() 
    {
        this.EventStartListening<DialogueEvent>();
    }

    private void OnDisable() 
    {
        this.EventStopListening<DialogueEvent>();
    }
}
