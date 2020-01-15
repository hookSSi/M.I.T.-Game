using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;
using MIT.SamtleGame.NPC;
using TMPro;

public enum DialogueStatus { Start, End, Next }

[System.Serializable]
public struct DialogueEvent
{
    public int _id;
    public int _index;
    public DialogueStatus _status;

    public DialogueEvent(int id = 0, DialogueStatus status = DialogueStatus.Start, int index = 0)
    {
        _id = id;
        _index = index;
        _status = status;
    }

    static DialogueEvent _event;

    public static void Trigger(int id = 0, DialogueStatus status = DialogueStatus.Start, int index = 0)
    {
        _event._id = id;
        _event._index = index;
        _event._status = status;
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

    private void DialogueUpdate(int id, DialogueStatus status)
    {
        if(!_npcs.ContainsKey(id))
        {
            return;
        }

        switch(status)
        {
            case DialogueStatus.Start:
                ShowDialogue(id);
                break;
            case DialogueStatus.End:
                EndDialogue();
                break;
            case DialogueStatus.Next:
                NextPage();
                break;
        }
    }

    private void CreateDialogueUI()
    {
        /// UI 프리팹을 주 Canvas에 자식으로 붙이는 과정
        _currentDialogueUI = Instantiate(_dialogueUIPrefab);
        _currentDialogueUI.transform.parent = this.gameObject.transform;

        _curretDialogue = _currentDialogueUI.GetComponentInChildren<DialogueBox>();
        Debug.Log(_curretDialogue);
    }

    private void ShowDialogue(int id)
    {
        EndDialogue();

        _currentNpc = _npcs[id];
        _dialogueUIPrefab = _currentNpc._dialogueUIPrefab;

        CreateDialogueUI();

        _curretDialogue._textPages = new List<string>(_currentNpc._textPages);
    }

    private void NextPage()
    {
        _curretDialogue.NextPage();
    }

    private void EndDialogue()
    {
        if(_currentDialogueUI != null)
            Destroy(_currentDialogueUI);
    }

    public static void AddNpc(Npc npc)
    {
        if(!_npcs.ContainsKey(npc._id))
        {
            _npcs.Add(npc._id, npc);
        }
    }

    public virtual void OnEvent(DialogueEvent dialogueEvent)
    {
        DialogueUpdate(dialogueEvent._id, dialogueEvent._status);
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
