using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.DesignPattern;

[RequireComponent(typeof(AudioSource))]
public class BgmManager : Singleton<BgmManager>
{
    public AudioClip[] _clips; //bgms

    private AudioSource _audio;

    private WaitForSeconds _waitTime = new WaitForSeconds(0.01f);

    protected override void Awake()
    {
        base.Awake();
        _audio = GetComponent<AudioSource>();
    }

    public void Play(int _playTrack, bool loop = true, float volume = 0.4f)
    {
        _audio.volume = volume;
        _audio.clip = _clips[_playTrack];
        _audio.loop = loop;
        _audio.Play();
    }

    public void Pause()
    {
        _audio.Pause();
    }

    public void UnPause()
    {
        _audio.UnPause();
    }

    public void Stop()
    {
        _audio.Stop();
    }

    public void SetVolume(float _volume)
    {
        _audio.volume = _volume;
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
            _audio.volume = i;
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
            _audio.volume = i;
            yield return _waitTime;
        }
    }

    public bool IsPlaying(){ return _audio.isPlaying; }
    public AudioSource GetAudio(){ return _audio; }
}
