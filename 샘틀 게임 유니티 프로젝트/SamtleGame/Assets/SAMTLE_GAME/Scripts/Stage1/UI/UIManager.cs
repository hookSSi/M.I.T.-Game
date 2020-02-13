using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MIT.SamtleGame.Tools;
using MIT.SamtleGame.UI;

namespace MIT.SamtleGame.Stage1
{
    public struct PlayerHittedEvent
    {
        public int _playerHp;

        public PlayerHittedEvent(int damage = 0)
        {
            _playerHp = damage;
        }

        private static PlayerHittedEvent _event;

        public static void Trigger(int damage = 0)
        {
            _event._playerHp = damage;
            EventManager.TriggerEvent(_event);
        }
    }

    public struct BossHittedEvent
    {
        public int _bossHp;

        public BossHittedEvent(int damage = 0)
        {
            _bossHp = damage;
        }

        private static BossHittedEvent _event;

        public static void Trigger(int damage = 0)
        {
            _event._bossHp = damage;
            EventManager.TriggerEvent(_event);
        }
    }

    public struct ScoreUpEvent
    {
        public int _amount;

        public ScoreUpEvent(int amount = 0)
        {
            _amount = amount;
        }

        private static ScoreUpEvent _event;

        public static void Trigger(int amount = 0)
        {
            _event._amount = amount;
            EventManager.TriggerEvent(_event);
        }
    }

    public class UIManager : MonoBehaviour, EventListener<PlayerHittedEvent>, EventListener<BossHittedEvent>, EventListener<ScoreUpEvent>
    {
        private float _currentTimeLeft = 2000;
        private const int MAX_SCORE = 999999;
        private int _currentScore = 0;

        public HealthBar _playerHpUI;
        public HealthBar _bossHpUI;
        public TMP_Text _timeUI;
        public TMP_Text _score;
        public float _timeStart = 2000;

        private void Initialization()
        {
            _currentTimeLeft = _timeStart;
        }

        private void Start() 
        {
            Initialization();   
        }

        private void FixedUpdate() 
        {
            if(GameManager._isPlayable)
                TimeUpdate();
        }

        private void TimeUpdate()
        {
            _currentTimeLeft -= Time.deltaTime * 20;
            int temp = (int)_currentTimeLeft;

            if(_currentTimeLeft > 0)
                _timeUI.text = temp.ToString();
            else
                _timeUI.text = "0";
        }

        public virtual void OnEvent(PlayerHittedEvent playerHittedEvent)
        {
            _playerHpUI.SetHealth(playerHittedEvent._playerHp);
        }

        public virtual void OnEvent(BossHittedEvent bossHittedEvent)
        {
            _bossHpUI.SetHealth(bossHittedEvent._bossHp);
        }

        private string IntToScoreFormat(int score)
        {
            string scoreText = "1-";
            return (scoreText + score.ToString("000000"));
        }
        public virtual void OnEvent(ScoreUpEvent scoreUpEvent)
        {
            _currentScore += scoreUpEvent._amount;
            _score.text = IntToScoreFormat(_currentScore);
        }

        private void OnEnable() 
        {
            this.EventStartListening<PlayerHittedEvent>();
            this.EventStartListening<BossHittedEvent>();
            this.EventStartListening<ScoreUpEvent>();
        }

        private void OnDisable() 
        {
            this.EventStopListening<PlayerHittedEvent>();
            this.EventStopListening<BossHittedEvent>();
            this.EventStopListening<ScoreUpEvent>();
        }
    }
}
