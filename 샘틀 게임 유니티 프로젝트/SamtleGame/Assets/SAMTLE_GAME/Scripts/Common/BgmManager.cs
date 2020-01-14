using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    static public BgmManager instance;

    public AudioClip[] clips; //bgms

    private AudioSource source;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    public void Play(int _playTrack)
    {
        source.volume = 0.7f;
        source.clip = clips[_playTrack];
        source.Play();
    }

    public void Pause()
    {
        source.Pause();
    }
    public void UnPause()
    {
        source.UnPause();
    }


    public void Stop()
    {
        source.Stop();
    }


    public void FadeOutBgm()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutBgmCoroutine());
    }
    IEnumerator FadeOutBgmCoroutine()
    {
        for(float i = 0.7f ; i>=0f; i-=0.01f )
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    public void FadeInBgm()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInBgmCoroutine());
    }
    IEnumerator FadeInBgmCoroutine()
    {
        for (float i = 0f; i <= 0.7f; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }


    public void SetVolume(float _volume)
    {
        source.volume = _volume;
    }
}
