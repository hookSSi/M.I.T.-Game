using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< .merge_file_a19700
=======
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
>>>>>>> .merge_file_a03492

[System.Serializable]
public class Sound
{
<<<<<<< .merge_file_a19700
    public string name; //이름

    public AudioClip clip; //파일
    private AudioSource source; //플레이어

    public float volume;
    public bool loop;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
=======
    private AudioSource _source; //플레이어

    public string _name; //이름
    public AudioClip _clip; //파일
    [Range(0, 1)]
    public float _volume;
    public bool _isLoop;

    public void SetSource(AudioSource _source)
    {
        this._source = _source;
        this._source.clip = _clip;
        this._source.loop = _isLoop;
>>>>>>> .merge_file_a03492
    }

    public void Play()
    {
<<<<<<< .merge_file_a19700
        source.Play();
    }
    public void Stop()
    {
        source.Stop();
    }
    public void SetLoop() //반복재생
    {
        source.loop = true;
    }
    public void SetVolume()
    {
        source.volume = volume;
    }

    public void SetLoopCancel() //반복재생 취소
    {
        source.loop = false;
    }
}

public class AudioManager : MonoBehaviour
{
    static public AudioManager instance;

    [SerializeField]
    public Sound[] sounds;

    void Start()
    {
        for ( int i = 0; i < sounds.Length ; i++ )
        {
            GameObject soundObject = new GameObject("사운드 파일 이름 : " + i + "=" + sounds[i].name );
            sounds[i].SetSource( soundObject.AddComponent<AudioSource>() );
            soundObject.transform.SetParent(this.transform);
        }
    }

    private void Awake() //scene 이동 대비
    {
        if(instance  != null)
        {
            Destroy(this.gameObject);
        }

        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }


    public void Play (string _name)
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            if( _name == sounds[i].name )
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }

    public void SetLoopCancel(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoopCancel();
                return;
            }
        }
    }

    public void SetVolume(string _name , float _volume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].volume = _volume;
                sounds[i].SetVolume();
                return;
            }
        }
=======
        if(!_source.isPlaying)
            _source.Play();
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

public class AudioManager : Singleton<AudioManager>, EventListener<SoundEvent>
{
    [SerializeField]
    private Sound[] _sounds;
    private Dictionary<string, Sound> _soundDic = new Dictionary<string, Sound>();

    protected virtual void Initialization()
    {
        for ( int i = 0; i < _sounds.Length ; i++ )
        {
            GameObject soundObject = new GameObject("사운드 파일 이름 : " + i + "=" + _sounds[i]._name );
            _sounds[i].SetSource( soundObject.AddComponent<AudioSource>() );
            soundObject.transform.SetParent(this.transform);
            _soundDic[_sounds[i]._name] = _sounds[i];
        }
    }

    void Start()
    {
        Initialization();
    }

	protected override void Awake()
    {
        base.Awake();
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
>>>>>>> .merge_file_a03492
    }
}
