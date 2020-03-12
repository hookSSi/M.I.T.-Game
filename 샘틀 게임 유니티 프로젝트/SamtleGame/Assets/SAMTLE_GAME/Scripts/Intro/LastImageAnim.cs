using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;

public class LastImageAnim : MonoBehaviour
{
    private float _prevY;

    public RectTransform _imageRect;
    public float _destY = -420f;
    public float _delay = 1f;
    public float _endDelay = 1f;
    public float _duration = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _prevY = _imageRect.anchoredPosition.y;
    }

    public IEnumerator AnimImage()
    {
        Debug.Log("마지막 애니메이션 연출!");
        this.gameObject.SetActive(true);
        FadeOutEvent.Trigger(1);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(_delay);

        float timelapse = 0;

        while(true)
        {
            _imageRect.anchoredPosition = new Vector3(0, Mathf.Lerp(_prevY, _destY, timelapse / _duration), 0);
            timelapse += Time.deltaTime;

            if(_duration < timelapse)
            {
                yield return new WaitForSeconds(_endDelay);
                this.gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }
}
