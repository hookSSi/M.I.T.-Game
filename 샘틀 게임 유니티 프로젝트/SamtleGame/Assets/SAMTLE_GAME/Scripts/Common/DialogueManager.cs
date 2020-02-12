using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;
using MIT.SamtleGame.Stage2.NPC;
using MIT.SamtleGame.Stage2;
using TMPro;

public enum DialogueStatus { Start, End }

[System.Serializable]
public struct DialogueEvent
{
    public int _id;
    public int _index;
    public string _sound;
    public DialogueStatus _status;

    /// 대화가 끝날시 플레이어 컨트롤 여부
    /// status가 Start인 경우에만 체크
    public bool _isControllable; 
    
    public DialogueEvent(int id = 0, string sound = "", DialogueStatus status = DialogueStatus.Start, int index = 0, bool isControllable = true)
    {
        _id = id;
        _sound = sound;
        _index = index;
        _status = status;
        _isControllable = isControllable;
    }

    static DialogueEvent _event;

    public static void Trigger(int id = 0, string sound = "", DialogueStatus status = DialogueStatus.Start, int index = 0, bool isControllable = true)
    {
        _event._id = id;
        _event._sound = sound;
        _event._index = index;
        _event._status = status;
        _event._isControllable = isControllable;
        EventManager.TriggerEvent(_event);
    }
}

public class DialogueManager : MonoBehaviour, EventListener<DialogueEvent>
{
    private static Dictionary<int, Npc> _npcs = new Dictionary<int, Npc>();
    private static DialogueBox _curretDialogue;
    private static GameObject _currentDialogueUI; /// 현재 UI 객체

    [Header("현재 대화 UI Prefab")]
    public GameObject _dialogueUIPrefab;

    [Header("현재 대화중인 NPC")]
    public Npc _currentNpc;

    [Header("대화 끝날시 플레이어 컨트롤 여부")]
    public bool _isControllable;

    private void DialogueUpdate(int id, string sound, DialogueStatus status, bool isControllable)
    {
        if(!_npcs.ContainsKey(id))
        {
            return;
        }

        switch(status)
        {
            case DialogueStatus.Start:
                StartDialogue(id, sound);
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
        _currentDialogueUI = Instantiate(_dialogueUIPrefab, this.transform);
        _currentDialogueUI.transform.parent = this.gameObject.transform;

        _curretDialogue = _currentDialogueUI.GetComponentInChildren<DialogueBox>();
        _curretDialogue.Reset(id, textPages, 0, sound);
    }
    #endregion

    #region  대화창 리셋
    private void ResetDialogue(int id, List<DialoguePage> textPages, string sound)
    {
        Debug.LogFormat("{0} 대화창 초기화", id);
        _currentDialogueUI.SetActive(true);
        _curretDialogue = _currentDialogueUI.GetComponentInChildren<DialogueBox>();
        _curretDialogue.Reset(id, textPages, 0, sound);
    }
    #endregion

    #region  대화창 시작
    protected virtual void StartDialogue(int id, string sound)
    {
        _currentNpc = _npcs[id];
        
        if(_dialogueUIPrefab != _currentNpc._dialogueUIPrefab)
        {
            _dialogueUIPrefab = _currentNpc._dialogueUIPrefab;
            CreateDialogueUI(id, _currentNpc._textPages, sound);
        }
        else
        {
            ResetDialogue(id, _currentNpc._textPages, sound);
        }
    }
    #endregion

    #region  대화 종료
    protected virtual void EndDialogue()
    {
        if(_currentDialogueUI != null)
        {
            _currentDialogueUI.SetActive(false);
            PlayerControllerEvent.Trigger(_isControllable);
        }
    }
    #endregion

    public static void AddNpc(Npc npc)
    {
        if(!_npcs.ContainsKey(npc._id))
        {
            _npcs.Add(npc._id, npc);
        }
    }

    public virtual void OnEvent(DialogueEvent dialogueEvent)
    {
        DialogueUpdate(dialogueEvent._id, dialogueEvent._sound, dialogueEvent._status, dialogueEvent._isControllable);
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
