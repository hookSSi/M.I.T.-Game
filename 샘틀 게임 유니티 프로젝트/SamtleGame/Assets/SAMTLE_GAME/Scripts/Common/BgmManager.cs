using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< .merge_file_a19776
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
=======
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
>>>>>>> .merge_file_a10724
    }

    public void Play(int _playTrack)
    {
<<<<<<< .merge_file_a19776
        source.volume = 0.7f;
        source.clip = clips[_playTrack];
        source.Play();
=======
        _source.volume = 0.7f;
        _source.clip = _clips[_playTrack];
        _source.Play();
>>>>>>> .merge_file_a10724
    }

    public void Pause()
    {
<<<<<<< .merge_file_a19776
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

=======
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
>>>>>>> .merge_file_a10724

    public void FadeOutBgm()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutBgmCoroutine());
    }
    IEnumerator FadeOutBgmCoroutine()
    {
        for(float i = 0.7f ; i>=0f; i-=0.01f )
        {
<<<<<<< .merge_file_a19776
            source.volume = i;
            yield return waitTime;
=======
            _source.volume = i;
            yield return _waitTime;
>>>>>>> .merge_file_a10724
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
<<<<<<< .merge_file_a19776
            source.volume = i;
            yield return waitTime;
        }
    }


    public void SetVolume(float _volume)
    {
        source.volume = _volume;
    }
=======
            _source.volume = i;
            yield return _waitTime;
        }
    }
>>>>>>> .merge_file_a10724
}
