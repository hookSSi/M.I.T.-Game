using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimImage : Image
{
    [Header("애니메이션 설정")]
    [SerializeField]
    [Tooltip("애니메이션 스프라이트")]
    private Sprite[] _sprites;
    [SerializeField]
    [Tooltip("애니메이션 장면 전환 딜레이")]
    private float _delay;
    
    public bool _isLoop;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayAnim(_delay));
    }

    IEnumerator PlayAnim(float delay)
    {   
        int index = 0;
        int length = _sprites.Length;

        while(true)
        {
            if(_isLoop)
            {
                this.sprite = _sprites[index];
                yield return new WaitForSeconds(delay);

                index = (index + 1)  % length;
            }
            else
                yield return null;
        }
    }
}
