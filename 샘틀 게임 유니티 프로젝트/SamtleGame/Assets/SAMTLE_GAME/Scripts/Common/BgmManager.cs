using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.DesignPattern;

[RequireComponent(typeof(AudioSource))]
public class BgmManager : Singleton<BgmManager>
{
    private Dictionary<string, Sound> _soundDic = new Dictionary<string, Sound>();
    private AudioSource _audio;
    private WaitForSeconds _waitTime = new WaitForSeconds(0.01f);
    [HideInInspector] public List<Sound> _sounds = new List<Sound>();

    protected virtual void Initialization()
    {
        foreach(Sound sound in _sounds)
        {
            _soundDic[sound._name] = sound;
        }

        _audio = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        Initialization();
    }

    public void Play(string trackName = "(None)", bool loop = true, float volume = 0.4f)
    {
        if(_soundDic.ContainsKey(trackName))
        {
            _soundDic[trackName].Play(_audio);
            _audio.volume = volume;
            _audio.loop = loop;
        }
        else
            Debug.LogFormat("{0} 이름의 bgm을 재생할 수 없습니다.", trackName);

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

    public struct BgmCreationParams 
    {
        public string _bgmName;
        public string _path;
    }
}