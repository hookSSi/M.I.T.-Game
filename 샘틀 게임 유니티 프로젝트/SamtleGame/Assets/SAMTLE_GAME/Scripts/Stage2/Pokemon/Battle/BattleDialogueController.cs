using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MIT.SamtleGame.Stage2
{
    public class BattleDialogueController : MonoBehaviour
    {
        public int _id = 0;
        public bool _isEnd = true;
        public string _typingSound = "";
        [SerializeField] private List<DialoguePage> _textPages;
        [SerializeField] private List<bool> _stopPoints;

        public List<bool> IsStoppingPoint { get { return _stopPoints; } }

        public void AddNextPage(string newText, bool isStopping = false)
        {
            DialoguePage newPage = new DialoguePage();
            newPage._text = newText;

            _textPages.Add(newPage);
            _stopPoints.Add(isStopping);
        }

        public void ClearPages()
        {
            if (_textPages == null) _textPages = new List<DialoguePage>();
            if (_stopPoints == null) _stopPoints = new List<bool>();

            _textPages.Clear();
            _stopPoints.Clear();
        }

        public void NextDialogue()
        {
            _isEnd = false;
            if (DialogueManager.Instance._isEnd)
                DialogueEvent.Trigger(_id, _textPages, 0, _typingSound, DialogueStatus.Start);
        }

        public void EndDialogue()
        {
            DialogueEvent.Trigger(_id, null, 0, _typingSound, DialogueStatus.End);
        }
    }
}