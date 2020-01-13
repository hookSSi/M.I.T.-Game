using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.NPC
{
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

        protected virtual void Talk()
        {
            DialogueEvent.Trigger(_talkEvent._id);
        }

        private void NextPage()
        {
            DialogueEvent.Trigger(_id, DialogueStatus.Next);
        }

        protected virtual void Update() 
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                Talk();
            }
            if(Input.GetKeyDown(KeyCode.D))
            {
                NextPage();
            }
        }
    }
}
