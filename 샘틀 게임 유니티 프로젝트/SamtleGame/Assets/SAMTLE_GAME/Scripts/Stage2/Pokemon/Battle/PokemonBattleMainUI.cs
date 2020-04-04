using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public class PokemonBattleMainUI : MonoBehaviour
    {
        public bool _isEnemyHpAnimating { get; private set; }
        public bool _isPlayerHpAnimating { get; private set; }
        public bool _isPlayerExpAnimating { get; private set; }
        public bool _isIpsangMoving { get; private set; }
        public bool _isPlayingHitEffect { get; private set; }
        public bool _isFadingInBattle { get; private set; }

        // Main UI 표시 상태 : 배틀 화면, 가방 탐색 화면, 포켓몬 정보 확인 화면
        public enum UIState
        {
            None, Battle, Bag, Information
        }

        // Pokemon UI
        // Hp Update Type : 포켓몬 체력이 변할 때 체력바가 변하는 방식을 결정한다.
        // FixedTime : 일정한 시간을 두고 체력바가 변한다.
        // 체력이 많이 까지면 빠르게, 적게 까지면 천천히 변한다.(최신 포켓몬 소드실드 참고)
        // FixedSpeed : 체력바가 변하는 속도가 일정해진다.(구 포켓몬 골드 버전 참고)
        public enum UpdateType
        {
            FixedTime, FixedSpeed
        }

        [Header("[포켓몬 UI]")]
        [Tooltip("Hp Update Type : 포켓몬 체력이 변할 때 체력바가 변하는 방식을 결정한다.\n" +
            "FixedTime : 일정한 시간을 두고 체력바가 변한다.\n" +
            "체력이 많이 까지면 빠르게, 적게 까지면 천천히 변한다.(최신 포켓몬 소드실드 참고)\n" +
            "FixedSpeed : 체력바가 변하는 속도가 일정해진다.(구 포켓몬 골드 버전 참고)")]
        public UpdateType _hpUpdateType = UpdateType.FixedTime;
        public float _hpChangeTime = 0.8f;
        public float _hpChangeSpeed = 0.5f; // (초당 차오르는 비율; 1f는 초당 100%)
        public float _expChangeSpeed = 2f; // (초당 차오르는 비율)

        // 상대 : 이름, 레벨, 체력바
        [Header("- 적 포켓몬 UI")]
        [Tooltip("적의 포켓몬 UI창 오브젝트")]
        public GameObject _enemyPokemonUI;
        public Text _enemyPokemonName;
        public Text _enemyLevelText;
        public Slider _enemyHpSlider;
        public Image _enemyPokemonImage;
        [Header("- 플레이어 포켓몬 UI")]
        [Tooltip("플레이어의 포켓몬 UI창 오브젝트")]
        public GameObject _playerPokemonUI;
        public Text _playerPokemonName;
        public Text _playerHpText;
        public Text _playerLevelText;
        public Slider _playerHpSlider;
        public Slider _playerExpSlider;
        public Image _playerPokemonImage;

        // 전투 화면 표시
        [Header("[전투 화면]")]
        [SerializeField] private UIState _state;
        public GameObject _battleUI;
        public GameObject _bagUI;
        public GameObject _informationUI;

        [Header("입학생 캐릭터")]
        public RectTransform _ipsangTransform;
        public RectTransform _ipsangStart;
        public RectTransform _ipsangDestination;
        public float _ipsangSpeed;

        [Header("페이드용")]
        public Image _blackBox;
        public Image _battleFaderImage;
        public Animator _battleFaderAnim;

        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            // Initialize
            _isEnemyHpAnimating = false;
            _isPlayerHpAnimating = false;
            _isPlayerExpAnimating = false;
            _isIpsangMoving = false;

            _state = UIState.Battle;

            if (_ipsangStart)
                _ipsangTransform.anchoredPosition = _ipsangStart.anchoredPosition;

            float playerHp = PokemonBattleManager.Instance._myPokemon != null ? PokemonBattleManager.Instance._myPokemon.Health : 100f;
            float playerMaxHp = PokemonBattleManager.Instance._myPokemon != null ? PokemonBattleManager.Instance._myPokemon.MaxHealth : 100f;
            float enemyHp = PokemonBattleManager.Instance._enemyPokemon != null ? PokemonBattleManager.Instance._enemyPokemon.Health : 100f;
            float enemyMaxHp = PokemonBattleManager.Instance._enemyPokemon != null ? PokemonBattleManager.Instance._enemyPokemon.MaxHealth : 100f;

            UpdatePlayerHpUI(playerHp, playerMaxHp, false);
            UpdateEnemyHpUI(enemyHp, enemyMaxHp, false);
            SetHealthColor(_playerHpSlider, playerHp, playerMaxHp);
            SetHealthColor(_enemyHpSlider, enemyHp, enemyMaxHp);
        }

        public void MoveIpsangSide()
        {
            if (_ipsangTransform == null || _ipsangStart == null || _ipsangDestination == null)
                return;

            _isIpsangMoving = true;
            StartCoroutine("MovePlayerSide");
        }

        private IEnumerator MovePlayerSide()
        {
            while (true)
            {
                Vector2 delta = _ipsangDestination.anchoredPosition - _ipsangTransform.anchoredPosition;
                Vector2 direction = delta.normalized;
                float speed = Mathf.Clamp(_ipsangSpeed * Time.deltaTime, 0f, delta.magnitude);
                if (speed == 0f)
                    break;

                _ipsangTransform.anchoredPosition += direction * speed;
                yield return null;
            }

            _isIpsangMoving = false;
        }

        public void UpdateValue(Pokemon playerPokemon, Pokemon enemyPokemon, int playerLevel, int enemyLevel, float playerExperience, float maxExperience)
        {
            // Initialize
            _isEnemyHpAnimating = false;
            _isPlayerHpAnimating = false;
            _isPlayerExpAnimating = false;

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
                case UIState.None:
                    _battleUI.SetActive(false);
                    _bagUI.SetActive(false);
                    _informationUI.SetActive(false);
                    break;
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

        public void UpdateEnemyImage(bool isvisible, bool isAnimated = false)
        {
            Pokemon enemyPokemon = PokemonBattleManager.Instance._enemyPokemon;
            Image image = _enemyPokemonImage;

            if (enemyPokemon.Info._frontImage)
                image.sprite = enemyPokemon.Info._frontImage;

            if (isAnimated == false) image.gameObject.SetActive(isvisible);
            else if (isvisible) StartCoroutine(FadeInImage(image, 0.3f));
            else StartCoroutine(FadeOutImage(image, 1f));
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

        public void HitEnemy()
        {
            Image enemyPokemonImage = PokemonBattleManager.Instance._enemyPokemon.GetComponentInChildren<Image>();

            _isPlayingHitEffect = true;
            StartCoroutine(PlayHitEffect(enemyPokemonImage));
        }

        // 플레이어 포켓몬 UI 업데이트
        public void SetActivePlayerPokemonUI(bool isvisible)
        {
            if (_playerPokemonUI != null)
                _playerPokemonUI.SetActive(isvisible);
        }

        public void UpdatePlayerImage(bool isvisible, bool isAnimated = false)
        {
            Pokemon playerPokemon = PokemonBattleManager.Instance._myPokemon;
            Image image = _playerPokemonImage;

            if (playerPokemon.Info._backImage)
                image.sprite = playerPokemon.Info._backImage;

            if (isAnimated == false) image.gameObject.SetActive(isvisible);
            else if (isvisible)
            {
                StartCoroutine(FadeInImage(image, 0.3f));
            }
            else StartCoroutine(FadeOutImage(image, 1f));
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

        public void HitPlayer()
        {
            Image playerPokemonImage = PokemonBattleManager.Instance._myPokemon.GetComponentInChildren<Image>();

            _isPlayingHitEffect = true;
            StartCoroutine(PlayHitEffect(playerPokemonImage));
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
                        SetHealthColor(slider, currentValue, slider.maxValue);
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
                    SetHealthColor(slider, currentValue, slider.maxValue);
            }
            if (text != null) text.text = (int)newValue + "/" + (int)slider.maxValue;

            SetBool(false);
        }

        private void SetHealthColor(Slider slider, float currentValue, float maxValue)
        {
            float valueRatio = currentValue / maxValue;
            var colors = slider.colors;

            if (valueRatio > 0.5f)
                colors.disabledColor = new Color(0f, 0.688f, 0.11f);
            else if (valueRatio > 0.2f)
                colors.disabledColor = Color.yellow;
            else
                colors.disabledColor = Color.red;

            slider.colors = colors;
        }

        // 포켓몬 타격 이펙트
        private IEnumerator PlayHitEffect(Image pokemonImage)
        {
            // 1번 : 붉게 깜빡임.
            int flickNumber = 2;
            float timeDelta = 0.2f;
            Color hitColor = new Color(1f, 0.5f, 0.5f);

            for (int i = 0; i < flickNumber; i++)
            {
                pokemonImage.color = hitColor;
                yield return new WaitForSeconds(timeDelta);
                pokemonImage.color = Color.white;
                yield return new WaitForSeconds(timeDelta);
            }
        }

        public IEnumerator FadeOutImage(Image image, float time)
        {
            float fadeOutSpeed = 1f / time;
            Color curColor = image.color;
            
            while(curColor.a > 0f)
            {
                curColor.a = Mathf.Clamp(curColor.a - fadeOutSpeed * Time.deltaTime, 0f, 1f);
                image.color = curColor;
                yield return null;
            }
            image.gameObject.SetActive(false);
            curColor.a = 1;
            image.color = curColor;
        }

        public IEnumerator FadeInImage(Image image, float time)
        {
            float fadeInSpeed = 1f / time;
            Color curColor = image.color;
            curColor.a = 0f;
            image.color = curColor;

            image.gameObject.SetActive(true);

            while (curColor.a < 1f)
            {
                curColor.a = Mathf.Clamp(curColor.a + fadeInSpeed * Time.deltaTime, 0f, 1f);
                image.color = curColor;
                yield return null;
            }
        }

        public IEnumerator FadeInBattle(float timeToFadeIn = 1.5f)
        {
            float timeToFadeOut = 0.5f;
            Image image = _battleFaderImage;

            _isFadingInBattle = true;

            image.gameObject.SetActive(true);
            _battleFaderAnim.SetTrigger("Fight");

            yield return new WaitForSeconds(timeToFadeIn);

            StartCoroutine(FadeOutImage(image, timeToFadeOut));

            yield return new WaitForSeconds(timeToFadeOut);

            _isFadingInBattle = false;
        }
    }
}