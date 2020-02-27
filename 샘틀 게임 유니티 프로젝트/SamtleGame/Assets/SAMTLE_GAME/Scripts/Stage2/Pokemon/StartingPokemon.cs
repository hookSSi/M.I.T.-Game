using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public class StartingPokemon : MonoBehaviour
    {
        public float _moveDistance = 100f;
        public bool _isEnd = true;

        [SerializeField]
        private RectTransform[] _startList = new RectTransform[3];
        private EventSystem _eventSystem;
        private Vector3[] _previous;
        private Vector3[] _destination;
        private int _currentIndex = 1;

        [Header("테스트용 인자")]
        public bool _testMode= false;

        public void Awake()
        {
            _eventSystem = FindObjectOfType<EventSystem>();

            _previous = new Vector3[3];
            _destination = new Vector3[3];

            for (int i = 0; i < 3; i++)
            {
                _previous[i] = (Vector3)_startList[i].anchoredPosition + Vector3.down * _moveDistance;
                _destination[i] = _startList[i].anchoredPosition;
            }

            if (_testMode)
                StartSelecting();

            if (_isEnd == true)
                gameObject.SetActive(false);

        }

        public void StartSelecting()
        {
            _isEnd = false;
            gameObject.SetActive(true);
            _eventSystem.firstSelectedGameObject = _startList[_currentIndex].gameObject;
        }

        public void SelectIndex(int index)
        {
            _currentIndex = index;

            Swap(index);
        }

        public void Update()
        {
            if (_isEnd)
                return;

            for (int i = 0; i < 3; i++)
            {
                if (_startList[i] == null || _startList[i].anchoredPosition == (Vector2)_destination[i])
                    continue;

                Vector3 nextPosition = _startList[i].anchoredPosition;
                nextPosition.y = Vector3.Lerp(_startList[i].anchoredPosition, _destination[i], Time.deltaTime * 5f).y;

                _startList[i].anchoredPosition = nextPosition;
            }
        }

        public void Swap(int index)
        {
            Vector2 tempVector2;

            tempVector2 = _previous[index];
            _previous[index] = _destination[index];
            _destination[index] = tempVector2;
        }
    }
}