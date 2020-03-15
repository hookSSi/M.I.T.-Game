using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;

public struct IntroEvent
{
    static IntroEvent _event;

    public static void Trigger()
    {
        EventManager.TriggerEvent(_event);
    }
}

public class IntroManager : MonoBehaviour, EventListener<IntroEvent>
{
    public GameObject _title;
    public GameObject _subTitle;
    public GameObject _dialogueBox;
    public GameObject _introImage;

    public LastImageAnim _lastImage;

    private void Start() 
    {
        StartCoroutine(WaitStartKey());
    }

    private void ActiveDialogue()
    {
        _dialogueBox.SetActive(true);
        _introImage.SetActive(true);
    }

    private void StartBgm()
    {
        BgmManager.Instance.Play(0);
        BgmManager.Instance.SetVolume(0.15f);
    }

    private void LoadNextScene()
    {
        BgmManager.Instance.Pause();
        LoadingSceneManager.LoadScene("기차안");
    }

    /// 시작 키 입력을 기다림
    private IEnumerator WaitStartKey()
    {   
        while(true)
        {
            if(Input.GetKeyDown(KeyCode.A) && _title.activeSelf)
            {
                _title.SetActive(false);
                _subTitle.SetActive(false);
                ActiveDialogue();
                StartBgm();
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    /// 다음 Scene으로 넘어가기 전에 연출용
    private IEnumerator LoadNextSceneRoutine(float waitTime = 4f)
    {
        yield return StartCoroutine(_lastImage.AnimImage());

        _title.SetActive(true);
        _dialogueBox.SetActive(false);
        _introImage.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        LoadNextScene();
    }

    public virtual void OnEvent(IntroEvent introEvent)
    {
       StartCoroutine(LoadNextSceneRoutine());
    }

    private void OnEnable() 
    {
        this.EventStartListening<IntroEvent>();
    }

    private void OnDisable() 
    {
        this.EventStopListening<IntroEvent>();
    }
}
