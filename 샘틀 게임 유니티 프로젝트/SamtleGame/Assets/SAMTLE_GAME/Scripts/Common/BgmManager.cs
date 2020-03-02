using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.DesignPattern;

[RequireComponent(typeof(AudioSource))]
public class BgmManager : Singleton<BgmManager>
{
    public AudioClip[] _clips; //bgms

    private AudioSource _source;

    private WaitForSeconds _waitTime = new WaitForSeconds(0.01f);

    protected override void Awake()
    {
        base.Awake();
        _source = GetComponent<AudioSource>();
    }

    public void Play(int _playTrack, bool loop = false, float volume = 0.4f)
    {
        _source.volume = volume;
        _source.clip = _clips[_playTrack];
        _source.loop = loop;
        _source.Play();
    }

    public void Pause()
    {
        _source.Pause();
    }
    public void UnPause()
    {
        _source.UnPause();
    }

    public void Stop()
    {
        _source.Stop();
    }

    public void SetVolume(float _volume)
    {
        _source.volume = _volume;
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
            _source.volume = i;
            yield return _waitTime;
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
            _source.volume = i;
            yield return _waitTime;
        }
    }
}
