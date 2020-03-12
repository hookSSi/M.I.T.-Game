using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage3
{
    public class Door : Interactive
    {
        public override void Action()
        {
            StartCoroutine(DoorResponse(1f));
        }

        IEnumerator DoorResponse(float duration)
        {
            float lapse = 0f;

            SoundEvent.Trigger("문소리");

            while(lapse < duration)
            {
                lapse += Time.deltaTime;
                yield return null;
            }

            GameManager.Instance._player._controller.FocusOut();
            yield break;
        }
    }
}
