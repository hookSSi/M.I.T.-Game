using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.NPC
{
    public enum NpcAnimState { Idle = 0, Walk = 1} 

    public class Npc : MonoBehaviour
    {
        [Header("Identification")]
        public int _id = 0;
        [Header("UI 프리팹")]
        public GameObject _dialogueUIPrefab;
        [Header("대사 목록")]
        public List<string> _textPages;
        [Header("Event")]
        [SerializeField]
        private DialogueEvent _talkEvent;

        protected virtual void Initialization() 
        {
            DialogueManager.AddNpc(this);
        }

        private void Start() 
        {
            Initialization();
        }

        public string GetText(int index)
        {
            return _textPages[index];
        }
        public void SetText(int index, string text)
        {
            _textPages[index] = text;
        }

        public  virtual void Talk()
        {
            DialogueEvent.Trigger(_talkEvent._id);
        }

        public  void NextPage()
        {
            DialogueEvent.Trigger(_id, DialogueStatus.Next);
        }
    }
}
