﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;
using MIT.SamtleGame.Attributes;

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

	public struct ClearGameEvent
	{
		public static ClearGameEvent _event;
		public static void Trigger()
		{
			EventManager.TriggerEvent(_event);
		}
	}

	public class GameManager : MonoBehaviour, EventListener<PlayGameEvent>, EventListener<PasueGameEvent>, EventListener<ClearGameEvent>
    {
        public static int _totalEnemyCount = 0;
        public static bool _isPlayable = false;

        [Header("Stage 1 bgm 설정")]
        [GameBgm] public string _stage1Openingbgm;
        [GameBgm] public string _stage1Mainbgm;
        [GameBgm] public string _stage1Clearbgm;

        [Header("게임 정보")]
        public PlayerController _player;
        public GameObject _readyText;
        public GameObject _startText;

        private void Start() 
        {
            ReadyAndStart();
        }

        void ReadyAndStart()
        {
            StartCoroutine(ReadyAndStartRoutine());
        }

        public void Pause()
        {
            SpawnerEvent.Trigger(SpawnerState.Pasue);
            _player._isControllable = false;
            _isPlayable = false;
            Debug.Log("게임 멈춤");
        }

        public void Play()
        {
            _player._isControllable = true;
            _isPlayable = true;
            Debug.Log("게임 재개");
        }

        /// 하드 코딩함 리팩토링 요구됨
        private IEnumerator ReadyAndStartRoutine()
        {
            Pause();
            BgmManager.Instance.Play(_stage1Openingbgm);
            _readyText.SetActive(true);
            yield return new WaitForSecondsRealtime(3.0f);
            _startText.SetActive(true);
            yield return new WaitForSecondsRealtime(3.0f);
            _readyText.SetActive(false);
            _startText.SetActive(false);
            Play();
            BgmManager.Instance.Play(_stage1Mainbgm, true);
            yield return new WaitForSecondsRealtime(0.33f);
            SpawnerEvent.Trigger(SpawnerState.Play);
        }

        public virtual void OnEvent(PlayGameEvent playGameEvent)
        {
            Play();
        }
        public virtual void OnEvent(PasueGameEvent pauseGameEvent)
        {
            Pause();
        }
		public virtual void OnEvent(ClearGameEvent clearGameEvent)
		{
			Debug.Log("game clear!!");
            BgmManager.Instance.Play(_stage1Clearbgm, true);
			_player.transform.GetComponent<PlayerAnimController>().StartToGoToStair();
			Pause();
		}

        private void OnEnable() 
        {
            this.EventStartListening<PlayGameEvent>();
			this.EventStartListening<PasueGameEvent>();
			this.EventStartListening<ClearGameEvent>();
		}

        private void OnDisable() 
        {
            this.EventStopListening<PlayGameEvent>();
            this.EventStopListening<PasueGameEvent>();
			this.EventStopListening<ClearGameEvent>();
		}
    }
}
