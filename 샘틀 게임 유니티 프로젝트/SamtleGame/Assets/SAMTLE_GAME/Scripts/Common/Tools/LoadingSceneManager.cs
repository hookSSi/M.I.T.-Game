using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace MIT.SamtleGame.Tools
{
    public class LoadingSceneManager : MonoBehaviour
    {
        public enum LoadingStatus { LoadStarted, LoadComplete, NewSceneLoaded }

        public struct LoadingSceneEvent
        {
            public LoadingStatus _status;
            public string _sceneName;
            public LoadingSceneEvent(string sceneName, LoadingStatus status)
            {
                _status = status;
                _sceneName = sceneName;
            }
            static LoadingSceneEvent e;
            public static void Trigger(string sceneName, LoadingStatus status)
            {
                e._status = status;
                e._sceneName = sceneName;
                EventManager.TriggerEvent(e);
            }
        }

        [Header("로드 중 보여줄 Scene 이름")]
        public static string _loadingScreenSceneName = "LoadingScreen";
        [Header("게임 오브젝트")]
        /// the text object where you want the loading message to be displayed
		public Text _loadingText;
		/// the canvas group containing the progress bar
		public CanvasGroup _loadingProgressBar;
		/// the canvas group containing the animation
		public CanvasGroup _loadingAnimation;
		/// the canvas group containing the animation to play when loading is complete
		public CanvasGroup _loadingCompleteAnimation;
        [Header("시간")]
        public float _startFadeDuration = 0.2f;
        public float _progressBarSpeed = 2f;
        public float _exitFadeDuration = 0.2f;
        public float _loadCompleteDelay = 0.5f;

        [Header("설정")]
        [SerializeField]
        private static string _sceneToLoad = "";
        private AsyncOperation _asyncOperation;
        private float _fadeDuration = 0.5f;
        private float _fillTarget = 0f; // Loading Progress amount
        private string _loadingTextValue;

        /// <summary>
		/// 어디든 이 함수로 scene을 로드하세요
		/// </summary>
        /// <param name = "sceneToLoad">scene 이름</param>
        public static void LoadScene(string sceneToLoad)
        {
            _sceneToLoad = sceneToLoad;
            Application.backgroundLoadingPriority = ThreadPriority.High;
            if(_loadingScreenSceneName != null)
            {
                LoadingSceneEvent.Trigger(sceneToLoad, LoadingStatus.LoadStarted);
                SceneManager.LoadScene(_loadingScreenSceneName);
            }
        }

        public static void LoadScene(string sceneToLoad, string loadingSceneName)
        {
            _sceneToLoad = sceneToLoad;
            Application.backgroundLoadingPriority = ThreadPriority.High;
            SceneManager.LoadScene(_loadingScreenSceneName);
        }

        private void Start() 
        {
            //_loadingTextValue = _loadingText.text;
            if(_sceneToLoad != "")
            {
                StartCoroutine(LoadAsynchronously());
            }
        }

        private void Update() 
        {
            Time.timeScale = 1f;
            _loadingProgressBar.GetComponent<Image>().fillAmount = Maths.Approach(_loadingProgressBar.GetComponent<Image>().fillAmount, _fillTarget, Time.deltaTime * _progressBarSpeed);
        }

        private IEnumerator LoadAsynchronously()
        {
            LoadingSetup();

            _asyncOperation = SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Single);
            _asyncOperation.allowSceneActivation = false;

            while(_asyncOperation.progress < 0.9f)
            {
                _fillTarget = _asyncOperation.progress;
                yield return null;
            }
            _fillTarget = 1f;

            while(_loadingProgressBar.GetComponent<Image>().fillAmount != _fillTarget)
            {
                yield return null;
            }

            LoadingComplete();
            yield return new WaitForSeconds(_loadCompleteDelay);

            // MMfadeInevene.trigger
            yield return new WaitForSeconds(_exitFadeDuration);

            // 새로운 scene으로 교체
            _asyncOperation.allowSceneActivation = true;
            LoadingSceneEvent.Trigger(_sceneToLoad, LoadingStatus.NewSceneLoaded);
        }

        private void LoadingSetup()
        {

        }

        private void LoadingComplete()
        {
            LoadingSceneEvent.Trigger(_sceneToLoad, LoadingStatus.LoadComplete);
        }
    }
}