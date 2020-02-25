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
            _text.PlayText();
        }
    }
}

