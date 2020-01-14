using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBgm_FadeOut : MonoBehaviour
{
    BgmManager BGM;

    public int playMusicTrack;

    void Start()
    {
        BGM = FindObjectOfType<BgmManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(asdf());
    }

    IEnumerator asdf()
    {
        BGM.FadeOutBgm();

        yield return new WaitForSeconds(3f);

        BGM.FadeInBgm();
    }
}
