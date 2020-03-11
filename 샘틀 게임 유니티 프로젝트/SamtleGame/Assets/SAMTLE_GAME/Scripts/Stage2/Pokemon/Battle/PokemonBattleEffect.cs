using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MIT.SamtleGame.Stage2
{
    public class PokemonBattleEffect : MonoBehaviour
    {
        public Animator _animator;
        public RectTransform _enemyRect;
        public RectTransform _playerRect;
        public bool _isAnimating = false;
        private bool _toEnemyBool;
        private string _nextAnimClipName;
        private RectTransform _rect;

        private void Start()
        {
            _rect = GetComponent<RectTransform>();
            _nextAnimClipName = "";
            _toEnemyBool = false;
        }

        public void SetAnim(string animClipName, bool toEnemy)
        {
            Debug.Log(animClipName);
            _toEnemyBool = toEnemy;
            _nextAnimClipName = animClipName;
        }

        public void ResetAnim()
        {
            _nextAnimClipName = "";
            _toEnemyBool = false;
        }

        public void StartAnim(int index)
        {
            _rect.anchoredPosition =
                (index == 0 && _toEnemyBool == false) || (index == 1 && _toEnemyBool == true) ?
                _playerRect.anchoredPosition : _enemyRect.anchoredPosition;

            _rect.anchoredPosition += Vector2.down * 30f;
            _rect.sizeDelta = new Vector2(300f, 300f);

            _isAnimating = true;
            StartCoroutine("AnimationCoroutine");
        }

        private IEnumerator AnimationCoroutine()
        {
            switch(_nextAnimClipName)
            {
                case "Blizzard":
                    _animator.SetTrigger("Blizzard");
                    break;
                case "HeartAttack":
                    _animator.SetTrigger("HeartAttack");
                    break;
                case "Sleep":
                    _rect.anchoredPosition = _rect.anchoredPosition
                        + (Vector2.up * 150f);
                    _rect.sizeDelta = new Vector2(200f, 200f);
                    _animator.SetTrigger("Sleep");
                    break;
            }

            yield return new WaitWhile(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false);
            _isAnimating = false;
        }
    }
}