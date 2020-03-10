using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage3
{
    public class NoteBook : Interactive
    {
        public GameObject _top;
        public CameraFilterPack_Drawing_Paper _cameraPaperEffect;
        public float _delay = 1f;
        public float _duration = 1f;

        public override void Action()
        {
            GameManager.Instance._player._controller.SetMovable(false);
            GameManager.Instance._player._controller.FocusIn();

            StartCoroutine(OpenNoteBook());
        }

        IEnumerator OpenNoteBook()
        {
            yield return new WaitForSeconds(_delay);

            float lapse = 0f;

            while(lapse < _duration)
            {
                float value = Mathf.Lerp(this.transform.rotation.z, -180, Time.deltaTime);
                _top.transform.Rotate(0, 0, value);
                lapse += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            _cameraPaperEffect.enabled = true;
            yield break;
        }
    }
}
