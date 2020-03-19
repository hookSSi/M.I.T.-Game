using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    private AudioSource _source; //플레이어

    public string _name; //이름
    public List<AudioClip> _clip = new List<AudioClip>(); //파일
    [Range(0, 1)]
    public float _volume;
    public bool _isLoop;
    public bool _isFoldOut;

    public void SetSource(AudioSource _source)
    {
        this._source = _source;
        this._source.clip = _clip[0];
        this._source.loop = _isLoop;
    }

    public void Play()
    {
        if(_source.isPlaying)
        {
            _source.Stop();
        }
        
        if(_clip.Count >= 1)
        {
            this._source.clip = _clip[Random.Range(0, _clip.Count)];
        }
        
        _source.Play();
    }

    public void Play(AudioSource source)
    {
        if(source.isPlaying)
        {
            source.Stop();
        }

        if(_clip.Count >= 1)
        {
            source.clip = _clip[Random.Range(0, _clip.Count)];
        }
        
        source.Play();
    }

    public void Stop()
    {
        _source.Stop();
    }
    public void SetLoop(bool isLoop) //반복재생
    {
        _source.loop = isLoop;
        _isLoop = isLoop;
    }
    public void SetVolume(float volume)
    {
        _source.volume = volume;
        _volume = volume;
    }
}