using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage3
{
    public class NoteBook : Interactive
    {
        public override void Action()
        {
            GameManager.Instance._player._controller.SetMovable(false);
            GameManager.Instance._player._controller.FocusIn();

        }
    }
}
