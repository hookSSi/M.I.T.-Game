using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    [System.Serializable]
    public class RegisterAction : UnityEvent<string> { };

    public class StartingPokemon : MonoBehaviour
    {
        public RectTransform _spotlight;
        public TextMeshProUGUI _text;
        public string[] _pokemonName = new string[3];
        public GameObject _choice;
        public GameObject _yesObject;
        public float _moveDistance = 200f;
        [Range(0f, 10f)]
        public float _deselectScale = 0.8f;
        [Range(0f, 10f)]
        public float _selectScale = 1.5f;
        [Range(0f, 1f)]
        public float _smoothTime = 0.2f;
        public bool _isEnd = true;
        [Header("포켓몬 등록 이벤트(void event(string pokeName))")]
        public RegisterAction _pokemonRegisterAction;

        [SerializeField]
        private RectTransform[] _startList = new RectTransform[3];
        private Button[] _buttonList;
        private EventSystem _eventSystem;
        private Vector3[] _previous;
        private Vector3[] _destination;
        private float[] _targetSize = new float[3];
        private float[] _currentSizeVelocity = new float[3];
        private float _currentSpotVelocity = 0f;
        private int _currentIndex;

        [Header("테스트용 인자")]
        public bool _testMode= false;

        private void Awake()
        {
            _eventSystem = FindObjectOfType<EventSystem>();

            _previous = new Vector3[3];
            _destination = new Vector3[3];
            _buttonList = new Button[3];

            for (int i = 0; i < 3; i++)
            {
                _buttonList[i] = _startList[i].GetComponent<Button>();
                _previous[i] = (Vector3)_startList[i].anchoredPosition + Vector3.down * _moveDistance;
                _destination[i] = _startList[i].anchoredPosition;
                _currentSizeVelocity[i] = 0f;
            }

#if UNITY_EDITOR
            if (_testMode)
                StartSelecting();
            // 이벤트 등록 테스트
            // _pokemonRegisterAction.AddListener((data) => { PokemonBattleManager.Instance.StartBattle(data, "C++"); });
#endif

            if (_isEnd == true)
                gameObject.SetActive(false);
        }

        public void StartSelecting()
        {
            _currentIndex = 1;
            _isEnd = false;
            gameObject.SetActive(true);
            _choice?.SetActive(false);
            _eventSystem.firstSelectedGameObject = _startList[_currentIndex].gameObject;
            _eventSystem.SetSelectedGameObject(_startList[_currentIndex].gameObject);
            SetText(_currentIndex);
        }

        public void SelectIndex(int index)
        {
            int prevIndex = _currentIndex;
            _currentIndex = index;

            SwapVector(index);
            SetText(index);

            // 상호작용 색 설정
            ColorBlock prevColor = _buttonList[prevIndex].colors;
            prevColor.disabledColor = Color.black;
            _buttonList[prevIndex].colors = prevColor;

            ColorBlock newColor = _buttonList[index].colors;
            newColor.disabledColor = Color.clear;
            _buttonList[index].colors = newColor;

            for (int i = 0; i < 3; i++)
            {
                if (i == index)
                    _targetSize[i] = _selectScale;
                else
                    _targetSize[i] = _deselectScale;
            }
        }

        public void Submit()
        {
            for (int i = 0; i < 3; i++)
                _buttonList[i].interactable = false;

            _choice.SetActive(true);
            _eventSystem.firstSelectedGameObject = _yesObject;
            _eventSystem.SetSelectedGameObject(_yesObject);

            _text.text = "정말로 " + _pokemonName[_currentIndex] + "을 선택하시겠습니까?";
        }

        public void Confirm()
        {
            _isEnd = true;

            _pokemonRegisterAction.Invoke(_pokemonName[_currentIndex]);

            gameObject.SetActive(false);
        }

        public void Cancel()
        {
            for (int i = 0; i < 3; i++)
                _buttonList[i].interactable = true;

            _choice?.SetActive(false);
            _eventSystem.firstSelectedGameObject = _startList[_currentIndex].gameObject;
            _eventSystem.SetSelectedGameObject(_startList[_currentIndex].gameObject);

            SetText(_currentIndex);
        }

        private void Update()
        {
            if (_isEnd)
                return;

            Vector3 targetPosition = _startList[_currentIndex].anchoredPosition;
            if (_spotlight && _spotlight.anchoredPosition.x != targetPosition.x)
            {
                Vector3 nextPosition = _spotlight.anchoredPosition;

                // nextPosition.x = Mathf.SmoothDamp(nextPosition.x, targetPosition.x, ref _currentSpotVelocity, _smoothTime);
                nextPosition.x = targetPosition.x;
                _spotlight.anchoredPosition = nextPosition;
            }

            for (int i = 0; i < 3; i++)
            {
                if (_targetSize[i] != _startList[i].localScale.x)
                {
                    float currentSize = _startList[i].localScale.x;

                    currentSize = Mathf.SmoothDamp(currentSize, _targetSize[i], ref _currentSizeVelocity[i], _smoothTime);
                    _startList[i].localScale = new Vector3(currentSize, currentSize);
                }

                if (_startList[i] != null && _startList[i].anchoredPosition != (Vector2)_destination[i])
                {
                    Vector3 nextPosition = _startList[i].anchoredPosition;
                    nextPosition.y = Vector3.Lerp(_startList[i].anchoredPosition, _destination[i], Time.deltaTime * 5f).y;

                    _startList[i].anchoredPosition = nextPosition;
                }
            }
        }

        private void SwapVector(int index)
        {
            Vector2 tempVector2;

            tempVector2 = _previous[index];
            _previous[index] = _destination[index];
            _destination[index] = tempVector2;
        }

        private void SetText(int index)
        {
            if (_text == null) return;

            if (index == 0)
                _text.text = "복잡한타입 포켓몬\nC++";
            else if (index == 1)
                _text.text = "커피타입 포켓몬\nJava";
            else
                _text.text = "뱀타입 포켓몬\nPython";
        }
    }
}