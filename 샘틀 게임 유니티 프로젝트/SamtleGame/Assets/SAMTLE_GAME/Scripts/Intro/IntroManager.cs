using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;
using MIT.SamtleGame.Attributes;

namespace MIT.SamtleGame.Intro
{
    public struct IntroEvent
    {
        static IntroEvent _event;

        public static void Trigger()
        {
            EventManager.TriggerEvent(_event);
        }
    }

    [System.Serializable]
    public struct IntroScenePage
    {
        // Sprite
        public Sprite _sprite;
        // DialoguePage
        [Multiline(3)]
        public string _text;
        [Range(0, 100)]
        [Tooltip("실행전 딜레이")]
        public float _delay;
        [Range(0, 100)]
        [Tooltip("실행후 지속시간")]
        public float _duration;
    }

    public class IntroManager : MonoBehaviour, EventListener<IntroEvent>
    {
        [Header("인트로 오브젝트")]
        public GameObject _title;
        public GameObject _subTitle;
        public GameObject _dialogueBox;
        public GameObject _introImage;
        public LastImageAnim _lastImage;
        public AnimImage _animImage;
        public IntroDialogueBox _introDialogue;

        [Header("인트로 BGM"), GameBgm]
        public string _introBgmTitle;

        [HideInInspector]
        public List<IntroScenePage> _pages;

        private void Start()
        {
            AddNextPage();
            StartCoroutine(WaitStartKey());
        }

        private void AddNextPage()
        {
            // 텍스트 추가
            List<DialoguePage> list = new List<DialoguePage>();

            for (int i = 0; i < _pages.Count; i++)
                list.Add(DialoguePage.CreatePage(_pages[i]._text, _pages[i]._delay, _pages[i]._duration));

            _introDialogue._textPages = list;

            // 이미지 추가
            _animImage.SpriteInitialize(_pages.Count);

            for (int i = 0; i < _pages.Count; i++)
                _animImage.AddNext(_pages[i]._sprite, i);
        }

        private void ActiveDialogue()
        {
            _dialogueBox.SetActive(true);
            _introImage.SetActive(true);
        }

        private void StartBgm()
        {
            BgmManager.Instance.Play(_introBgmTitle, true, 1f);
        }

        private void LoadNextScene()
        {
            BgmManager.Instance.Pause();
            LoadingSceneManager.LoadScene("기차안");
        }

        /// 시작 키 입력을 기다림
        private IEnumerator WaitStartKey()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.A) && _title.activeSelf)
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

        public struct PageCreationParams
        {
            public string _spriteName;
            public string _path;
        }
    }
}