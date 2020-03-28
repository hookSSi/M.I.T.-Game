using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;
using MIT.SamtleGame.DesignPattern;

public enum SoundStatus { Play, Stop }

public struct SoundEvent
{
    public string _name;
    public SoundStatus _status;
    public bool _isLoop;
    [Range(0, 1)]
    public float _volume;

    public SoundEvent(string name, SoundStatus status = SoundStatus.Play, bool isLoop = false, float volume = 0.5f)
    {
        _name = name;
        _status = status;
        _isLoop = isLoop;
        _volume = volume;
    }

    public static SoundEvent _event;

    public static void Trigger(string name, SoundStatus status = SoundStatus.Play, bool isLoop = false, float volume = 0.5f)
    {
        _event._name = name;
        _event._status = status;
        _event._isLoop = isLoop;
        _event._volume = volume;
        EventManager.TriggerEvent(_event);
    }
}

public class AudioManager : Singleton<AudioManager>, EventListener<SoundEvent>
{
    [HideInInspector] public List<Sound> _sounds = new List<Sound>();
    private Dictionary<string, Sound> _soundDic = new Dictionary<string, Sound>();

    protected virtual void Initialization()
    {
        for ( int i = 0; i < _sounds.Count; i++ )
        {
            GameObject soundObject = new GameObject("사운드 파일 이름 : " + i + "=" + _sounds[i]._name );
            _sounds[i].SetSource( soundObject.AddComponent<AudioSource>() );
            soundObject.transform.SetParent(this.transform);
            _soundDic[_sounds[i]._name] = _sounds[i];
        }
    }

    void Awake()
    {
        Initialization();
    }

    public void Play (string name)
    {
        if(_soundDic.ContainsKey(name))
            _soundDic[name].Play();
    }

    public void Stop(string name)
    {
        if(_soundDic.ContainsKey(name))
            _soundDic[name].Stop();
    }

    public void SetLoop(string name, bool isLoop = false)
    {
        if(_soundDic.ContainsKey(name))
            _soundDic[name].SetLoop(isLoop);
        
    }

    public void SetVolume(string name , float volume)
    {
        if(_soundDic.ContainsKey(name))
        {
            _soundDic[name].SetVolume(volume);
        }
    }

    public virtual void OnEvent(SoundEvent soundEvent)
    {
        switch(soundEvent._status)
        {
            case SoundStatus.Play:
                SetLoop(soundEvent._name, soundEvent._isLoop);
                SetVolume(soundEvent._name, soundEvent._volume);
                Play(soundEvent._name);
                break;
            case SoundStatus.Stop:
                Stop(soundEvent._name);
                break;
        }
    }

    private void OnEnable() 
    {
        this.EventStartListening<SoundEvent>();
    }

    private void OnDisable() 
    {
        this.EventStopListening<SoundEvent>();
    }

    public struct AudioCreationParams 
    {
        public string _audioName;
        public string _path;
    }
}
