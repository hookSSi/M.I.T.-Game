using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;

namespace MIT.SamtleGame.Stage1
{
    public struct PasueGameEvent
    {
        public static PasueGameEvent _event;
        public static void Trigger()
        {
            EventManager.TriggerEvent(_event);
        }
    }

    public struct PlayGameEvent
    {
        public static PlayGameEvent _event;
        public static void Trigger()
        {
            EventManager.TriggerEvent(_event);
        }
    }

    public class GameManager : MonoBehaviour, EventListener<PlayGameEvent>, EventListener<PasueGameEvent>
    {
        public PlayerController _player;

        public GameObject _readyText;
        public GameObject _startText;

        private void Awake() 
        {
            ReadyAndStart();
        }

        void ReadyAndStart()
        {
            StartCoroutine(ReadyAndStartRoutine());
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            _player._isControllable = false;
            Debug.Log("게임 멈춤");
        }

        public void PlayGame()
        {
            Time.timeScale = 1.0f;
            _player._isControllable = true;
            Debug.Log("게임 재개");
        }
        /// 하드 코딩함 리팩토링 요구됨
        private IEnumerator ReadyAndStartRoutine()
        {
            PauseGame();
            _readyText.SetActive(true);
            yield return new WaitForSecondsRealtime(2.0f);
            _startText.SetActive(true);
            yield return new WaitForSecondsRealtime(2.0f);
            _readyText.SetActive(false);
            _startText.SetActive(false);
            PlayGame();
        }

        public virtual void OnEvent(PlayGameEvent playGameEvent)
        {
            PlayGame();
        }
        public virtual void OnEvent(PasueGameEvent pauseGameEvent)
        {
            PauseGame();
        }

        private void OnEnable() 
        {
            this.EventStartListening<PlayGameEvent>();
            this.EventStartListening<PasueGameEvent>();
        }

        private void OnDisable() 
        {
            this.EventStopListening<PlayGameEvent>();
            this.EventStopListening<PasueGameEvent>();
        }
    }
}
