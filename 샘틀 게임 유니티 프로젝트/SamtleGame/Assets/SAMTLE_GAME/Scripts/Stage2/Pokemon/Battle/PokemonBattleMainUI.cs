using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public class PokemonBattleMainUI : MonoBehaviour
    {
        /*
         * 체력, 경험치 등을 갱신할 때 다른 창으로 넘어갈 수 없게 하기 위한 Flag
         * 삭제 보류중
         */
        [HideInInspector] public bool _isEnemyHpAnimating { get; private set; }
        [HideInInspector] public bool _isPlayerHpAnimating { get; private set; }
        [HideInInspector] public bool _isPlayerExpAnimating { get; private set; }

        // Main UI 표시 상태 : 배틀 화면, 가방 탐색 화면, 포켓몬 정보 확인 화면
        public enum UIState
        {
            Battle, Bag, Information
        }

        // Pokemon UI
        // Hp Update Type : 포켓몬 체력이 변할 때 체력바가 변하는 방식을 결정한다.
        // FixedTime : 일정한 시간을 두고 체력바가 변한다.
        // 체력이 많이 까지면 빠르게, 적게 까지면 천천히 변한다.(최신 포켓몬 소드실드 참고)
        // FixedSpeed : 체력바가 변하는 속도가 일정해진다.(구 포켓몬 골드 버전 참고)
        private enum UpdateType
        {
            FixedTime, FixedSpeed
        }

        [Header("[포켓몬 UI]")]
        [Tooltip("Hp Update Type : 포켓몬 체력이 변할 때 체력바가 변하는 방식을 결정한다.\n" +
            "FixedTime : 일정한 시간을 두고 체력바가 변한다.\n" +
            "체력이 많이 까지면 빠르게, 적게 까지면 천천히 변한다.(최신 포켓몬 소드실드 참고)\n" +
            "FixedSpeed : 체력바가 변하는 속도가 일정해진다.(구 포켓몬 골드 버전 참고)")]
        [SerializeField] private UpdateType _hpUpdateType = UpdateType.FixedTime;
        [SerializeField] private float _hpChangeTime = 0.8f;
        [SerializeField] private float _hpChangeSpeed = 0.5f; // (초당 차오르는 비율; 1f는 초당 100%)

        [SerializeField] private float _expChangeSpeed = 2f; // (초당 차오르는 비율)

        // 상대 : 이름, 레벨, 체력바
        [Header("- 적 포켓몬 UI")]
        [Tooltip("적의 포켓몬 UI창 오브젝트")]
        [SerializeField] private GameObject _enemyPokemonUI;

        [SerializeField] private Text _enemyPokemonName;
        [SerializeField] private Text _enemyLevelText;

        [SerializeField] private Slider _enemyHpSlider;

        // 플레이어 : 이름, 레벨, 체력과 체력바, 경험치
        [Header("- 플레이어 포켓몬 UI")]
        [Tooltip("플레이어의 포켓몬 UI창 오브젝트")]
        [SerializeField] private GameObject _playerPokemonUI;

        [SerializeField] private Text _playerPokemonName;
        [SerializeField] private Text _playerHpText;
        [SerializeField] private Text _playerLevelText;

        [SerializeField] private Slider _playerHpSlider;
        [SerializeField] private Slider _playerExpSlider;

        // 전투 화면 표시
        [Header("[전투 화면]")]
        [SerializeField] private UIState _state;
        [SerializeField] private GameObject _battleUI;
        [SerializeField] private GameObject _bagUI;
        [SerializeField] private GameObject _informationUI;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            // Initialize
            _isEnemyHpAnimating = false;
            _isPlayerHpAnimating = false;
            _isPlayerExpAnimating = false;

            _state = UIState.Battle;

            UpdatePlayerHpUI(100f, 100f, false);
            UpdateEnemyHpUI(100f, 100f, false);

            SetHealthColor(_playerHpSlider, 100f, 100f);
            SetHealthColor(_enemyHpSlider, 100f, 100f);
        }

        public void UpdateValue(Pokemon playerPokemon, Pokemon enemyPokemon, int playerLevel, int enemyLevel, float playerExperience, float maxExperience)
        {
            // Initialize
            _isEnemyHpAnimating = false;
            _isPlayerHpAnimating = false;
            _isPlayerExpAnimating = false;

            _state = UIState.Battle;

            UpdateEnemyPokemonNameText(enemyPokemon.Info._name);
            UpdateEnemyHpUI(enemyPokemon.Health, enemyPokemon.Health, false);
            UpdateEnemyLevelText(enemyLevel);

            UpdatePlayerPokemonNameText(playerPokemon.Info._name);
            UpdatePlayerHpUI(playerPokemon.Health, playerPokemon.Health, false);
            UpdatePlayerExpUI(playerExperience, maxExperience, false);
            UpdatePlayerLevelText(playerLevel);

            SetActiveEnemyPokemonUI(true);
            SetActivePlayerPokemonUI(true);

            SetHealthColor(_playerHpSlider, 100f, 100f);
            SetHealthColor(_enemyHpSlider, 100f, 100f);
        }

        // 게임 UI 변경
        public void UpdateMainUI(UIState newState)
        {
            _state = newState;

            switch(newState)
            {
                case UIState.Battle:
                    _battleUI.SetActive(true);
                    _bagUI.SetActive(false);
                    _informationUI.SetActive(false);
                    break;
                case UIState.Bag:
                    _battleUI.SetActive(false);
                    _bagUI.SetActive(true);
                    _informationUI.SetActive(false);
                    break;
                case UIState.Information:
                    _battleUI.SetActive(false);
                    _bagUI.SetActive(false);
                    _informationUI.SetActive(true);
                    break;
            }
        }

        // 적 포켓몬 UI 업데이트
        public void SetActiveEnemyPokemonUI(bool isvisible)
        {
            if (_enemyPokemonUI != null)
                _enemyPokemonUI.SetActive(isvisible);
        }

        public void UpdateEnemyImage(bool isvisible)
        {
            Pokemon enemyPokemon = PokemonBattleManager.Instance._enemyPokemon;
            Image image = enemyPokemon.GetComponent<Image>();

            enemyPokemon.gameObject.SetActive(isvisible);

            image.sprite = enemyPokemon.Info._frontImage;
        }

        public void UpdateEnemyPokemonNameText(string newEnemyPokemonName)
        {
            if (_enemyPokemonName != null)
                _enemyPokemonName.text = newEnemyPokemonName;
        }

        public void UpdateEnemyHpUI(float newEnemyHp, float enemyMaxHp, bool useAnimation)
        {
            _enemyHpSlider.minValue = 0;
            _enemyHpSlider.maxValue = enemyMaxHp;

            if (!useAnimation)
            {
                _enemyHpSlider.value = newEnemyHp;
                SetHealthColor(_enemyHpSlider, newEnemyHp, enemyMaxHp);
                return;
            }

            float previousHp = _enemyHpSlider.value;
            float hpChangeSpeed =
                (_hpUpdateType == UpdateType.FixedTime) ? (newEnemyHp - previousHp) / _hpChangeTime
                : (newEnemyHp > previousHp ? _hpChangeSpeed : -1 * _hpChangeSpeed) * enemyMaxHp;

            Action<bool> SetFlag = (bool flag) => { _isEnemyHpAnimating = flag; };

            StartCoroutine(SliderTextAnimation(SetFlag, previousHp, newEnemyHp, hpChangeSpeed, _enemyHpSlider));
        }

        public void UpdateEnemyLevelText(int enemyLevel)
        {
            if (_enemyLevelText != null)
                _enemyLevelText.text = enemyLevel.ToString();
        }

        // 플레이어 포켓몬 UI 업데이트
        public void SetActivePlayerPokemonUI(bool isvisible)
        {
            if (_playerPokemonUI != null)
                _playerPokemonUI.SetActive(isvisible);
        }

        public void UpdatePlayerImage(bool isvisible)
        {
            Pokemon playerPokemon = PokemonBattleManager.Instance._myPokemon;
            Image image = playerPokemon.GetComponent<Image>();

            playerPokemon.gameObject.SetActive(isvisible);

            image.sprite = playerPokemon.Info._backImage;
        }

        public void UpdatePlayerPokemonNameText(string newPlayerPokemonName)
        {
            if (_playerPokemonName != null)
                _playerPokemonName.text = newPlayerPokemonName;
        }

        public void UpdatePlayerHpUI(float newPlayerHp, float playerMaxHp, bool useAnimation)
        {
            _playerHpSlider.minValue = 0;
            _playerHpSlider.maxValue = playerMaxHp;

            if (!useAnimation)
            {
                _playerHpText.text = newPlayerHp + "/" + playerMaxHp;
                _playerHpSlider.value = newPlayerHp;
                SetHealthColor(_playerHpSlider, newPlayerHp, playerMaxHp);
                return;
            }

            float previousHp = _playerHpSlider.value;
            float hpChangeSpeed =
                (_hpUpdateType == UpdateType.FixedTime) ? (newPlayerHp - previousHp) / _hpChangeTime
                : (newPlayerHp > previousHp ? _hpChangeSpeed : _hpChangeSpeed * (-1)) * playerMaxHp;

            Action<bool> SetFlag = (bool flag) => { _isPlayerHpAnimating = flag; };

            StartCoroutine(SliderTextAnimation(SetFlag, previousHp, newPlayerHp, hpChangeSpeed, _playerHpSlider, _playerHpText));
        }

        public void UpdatePlayerLevelText(int playerLevel)
        {
            if (_playerLevelText != null)
                _playerLevelText.text = playerLevel.ToString();
        }

        public void UpdatePlayerExpUI(float newPlayerExp, float playerMaxExp, bool useAnimation)
        {
            _playerExpSlider.minValue = 0;
            _playerExpSlider.maxValue = playerMaxExp;

            if (!useAnimation)
            {
                _playerExpSlider.value = newPlayerExp;
                return;
            }

            float previousExp = _playerExpSlider.value;
            float expChangeSpeed = _playerExpSlider.value < newPlayerExp ? _expChangeSpeed : _expChangeSpeed * (-1);

            expChangeSpeed *= playerMaxExp;

            Action<bool> SetFlag = (bool flag) => { _isPlayerExpAnimating = flag; };

            StartCoroutine(SliderTextAnimation(SetFlag, previousExp, newPlayerExp, expChangeSpeed, _playerExpSlider));
        }


        // 텍스트, 슬라이더 Animation을 제어하는 함수.
        private IEnumerator SliderTextAnimation(Action<bool> SetBool, float previousValue, float newValue, float changeSpeed, Slider slider = null, Text text = null)
        {
            float currentValue = previousValue;

            SetBool(true);

            while (true)
            {
                float deltaValue = Mathf.Abs(newValue - currentValue);
                if (deltaValue <= Mathf.Abs(changeSpeed) * Time.deltaTime)
                    break;

                currentValue += changeSpeed * Time.deltaTime;

                if (slider != null)
                {
                    slider.value = currentValue;

                    // Color Settings
                    if (slider == _enemyHpSlider || slider == _playerHpSlider)
                        SetHealthColor(slider, currentValue, 100f);
                }
                if (text != null) text.text = (int)currentValue + "/" + (int)slider.maxValue;

                if (currentValue <= 0f)
                {
                    newValue = 0f;
                    break;
                }

                yield return null;
            }

            if (slider != null)
            {
                slider.value = newValue;

                // Color Settings
                if (slider == _enemyHpSlider || slider == _playerHpSlider)
                    SetHealthColor(slider, currentValue, 100f);
            }
            if (text != null) text.text = (int)newValue + "/" + (int)slider.maxValue;

            SetBool(false);
        }

        private void SetHealthColor(Slider slider, float currentValue, float maxValue)
        {
            float valueRatio = currentValue / maxValue;
            var colors = slider.colors;

            if (valueRatio > 0.5f)
                colors.disabledColor = Color.green;
            else if (valueRatio > 0.2f)
                colors.disabledColor = Color.yellow;
            else
                colors.disabledColor = Color.red;

            slider.colors = colors;
        }
    }
}