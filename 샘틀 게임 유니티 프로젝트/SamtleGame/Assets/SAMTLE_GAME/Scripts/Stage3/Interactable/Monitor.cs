using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage3
{    
    public class Monitor : Interactive
    {
        public List<DialoguePage> _textPages;
        public DialogueBox _text;

        public override void Action()
        {
            SoundEvent.Trigger("버튼");
            GameManager.Instance._player._controller.SetMovable(false);
            GameManager.Instance._player._controller.FocusIn();

            _text.Reset(0, _textPages, 0, "Typing");
            _text.PlayText();
            GameManager.Instance.Programmer();
        }
    }
}

