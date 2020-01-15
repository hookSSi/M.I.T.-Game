using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PokemonBattleUIManager : MonoBehaviour
{
    // [HideInInspector]
    public bool _isEnemyHpAnimating = false;
    // [HideInInspector]
    public bool _isPlayerHpAnimating = false;
    // [HideInInspector]
    public bool _isPlayerExpAnimating = false;

    // Hp Update Type : 포켓몬 체력이 변할 때 체력바가 변하는 방식을 결정한다.
    // FixedTime : 일정한 시간을 두고 체력바가 변한다.
    // 체력이 많이 까지면 빠르게, 적게 까지면 천천히 변한다.(최신 포켓몬 소드실드 참고)
    // FixedSpeed : 체력바가 변하는 속도가 일정해진다.(구 포켓몬 골드 버전 참고)
    public enum UpdateType
    {
        FixedTime, FixedSpeed
    }

    [SerializeField] private UpdateType _hpUpdateType = UpdateType.FixedTime;
    [SerializeField] private float _hpChangeTime = 0.8f;
    [SerializeField] private float _hpChangeSpeed = 0.5f; // (초당 차오르는 비율; 1f는 초당 100%)

    [SerializeField] private float _expChangeSpeed = 2f; // (초당 차오르는 비율)

    // 상대 : 이름, 레벨, 체력바
    [Tooltip("적의 포켓몬 UI창 오브젝트")]
    [SerializeField] private GameObject _enemyPokemonUI;

    [SerializeField] private Text _enemyPokemonName;
    [SerializeField] private Text _enemyLevelText;

    [SerializeField] private Slider _enemyHpSlider;

    // 플레이어 : 이름, 레벨, 체력과 체력바, 경험치
    [Tooltip("플레이어의 포켓몬 UI창 오브젝트")]
    [SerializeField] private GameObject _playerPokemonUI;

    [SerializeField] private Text _playerPokemonName;
    [SerializeField] private Text _playerHpText;
    [SerializeField] private Text _playerLevelText;

    [SerializeField] private Slider _playerHpSlider;
    [SerializeField] private Slider _playerExpSlider;

    // 적 포켓몬 UI 업데이트 
    public void UpdateEnemyPokemonUI(bool isvisible)
    {
        if (_enemyPokemonUI != null)
            _enemyPokemonUI.SetActive(isvisible);
    }

    public void UpdateEnemyPokemonNameText(string newEnemyPokemonName)
    {
        if (_enemyPokemonName != null)
            _enemyPokemonName.text = newEnemyPokemonName;
    }

    public void UpdateEnemyHpUI(int newEnemyHp, int enemyMaxHp, bool useAnimation)
    {
        Debug.Log("적 체력 업데이트");
        _enemyHpSlider.minValue = 0;
        _enemyHpSlider.maxValue = enemyMaxHp;

        if (!useAnimation)
        {
            _enemyHpSlider.value = newEnemyHp;
            return;
        }

        float previousHp = _enemyHpSlider.value;
        float hpChangeSpeed =
            (_hpUpdateType == UpdateType.FixedTime) ? (newEnemyHp - previousHp) / _hpChangeTime
            : (newEnemyHp > previousHp ? _hpChangeSpeed : -1 * _hpChangeSpeed) * enemyMaxHp;

        Action<bool> SetFlag = (bool flag) => { _isEnemyHpAnimating = flag; };

        StartCoroutine(SliderTextAnimation(SetFlag, newEnemyHp, hpChangeSpeed, _enemyHpSlider));
    }

    public void UpdateEnemyLevelText(int enemyLevel)
    {
        if (_enemyLevelText != null)
            _enemyLevelText.text = enemyLevel.ToString();
    }

    // 플레이어 포켓몬 UI 업데이트
    public void UpdatePlayerPokemonUI(bool isvisible)
    {
        if (_playerPokemonUI != null)
            _playerPokemonUI.SetActive(isvisible);
    }

    public void UpdatePlayerPokemonNameText(string newPlayerPokemonName)
    {
        if (_playerPokemonName != null)
            _playerPokemonName.text = newPlayerPokemonName;
    }

    public void UpdatePlayerHpUI(int newPlayerHp, int playerMaxHp, bool useAnimation)
    {
        _playerHpSlider.minValue = 0;
        _playerHpSlider.maxValue = playerMaxHp;

        if (!useAnimation)
        {
            _playerHpText.text = newPlayerHp + "/" + playerMaxHp;
            _playerHpSlider.value = newPlayerHp;
            return;
        }

        float previousHp = _playerHpSlider.value;
        float hpChangeSpeed =
            (_hpUpdateType == UpdateType.FixedTime) ? (newPlayerHp - previousHp) / _hpChangeTime
            : (newPlayerHp > previousHp ? _hpChangeSpeed : _hpChangeSpeed * (-1)) * playerMaxHp;

        Action<bool> SetFlag = (bool flag) => { _isPlayerHpAnimating = flag; };

        StartCoroutine(SliderTextAnimation(SetFlag, newPlayerHp, hpChangeSpeed, _playerHpSlider, _playerHpText));
    }

    public void UpdatePlayerLevelText(int playerLevel)
    {
        if (_playerLevelText != null)
            _playerLevelText.text = playerLevel.ToString();
    }

    public void UpdatePlayerExpUI(int newPlayerExp, int playerMaxExp, bool useAnimation)
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

        StartCoroutine(SliderTextAnimation(SetFlag, newPlayerExp, expChangeSpeed, _playerExpSlider));
    }

    
    // 텍스트, 슬라이더 Animation을 제어하는 함수.
    private IEnumerator SliderTextAnimation(Action<bool> SetBool, float newValue, float changeSpeed, Slider slider = null, Text text = null)
    {
        float previousValue = _enemyHpSlider.value;
        float currentValue = previousValue;

        SetBool(true);

        while(true)
        {
            // 여기 스킵키 넣을 예정
            float deltaValue = Mathf.Abs(newValue - currentValue);
            if (deltaValue <= Mathf.Abs(changeSpeed) * Time.deltaTime)
                break;

            currentValue += changeSpeed * Time.deltaTime;

            if (slider != null) slider.value = currentValue;
            if (text != null) text.text = (int)currentValue + "/" + (int)slider.maxValue;

            yield return null;
        }

        if (slider != null) slider.value = newValue;
        if (text != null) text.text = (int)newValue + "/" + (int)slider.maxValue;

        SetBool(false);
    }
}
