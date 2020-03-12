using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Stage2.Tools;
using MIT.SamtleGame.Stage2.Pokemon;
using MIT.SamtleGame.Tools;

namespace MIT.SamtleGame.Stage2.NPC
{
    public enum NpcEventType { Move, Talk, Battle }

    public struct NpcEvent
    {
        public NpcEventType _type;
    }

    /*
    *   @desc 이벤트를 가진 Npc
    */
    public class EventNpc : Npc
    {
        protected bool _isEventEnd = false; 

        public bool _isWaiting = true;

        [Header("플레이어 감지 범위"), Space(20)]
        public float _detectRange; // 발견 범위
        public string _detectSound; // 발견했을 때 사운드
        public GameObject _reactMark; // 발견했을 때 표시
        public Transform _reactMarkDest; // 느낌표 목표 위치

        [Header("감지된 플레이어"), Space(20)]
        [SerializeField]
        protected PlayerController _detectedPlayer;

        protected override void Initialization() 
        {
            base.Initialization();

            if(_isWaiting)
                _reactMark.SetActive(false);
        }

        protected override void Update()
        {
            base.Update();

            if(_isWaiting)
            {
                if(DetectPlayer())
                {
                    StartCoroutine(Response());
                    StartCoroutine(EventRoutine());
                }
            }
        }

        protected virtual bool DetectPlayer()
        {
            Vector2 origin = ((Vector2)transform.position + _currentDir * _walkSize);
            Vector2 dest = ((Vector2)transform.position + (_currentDir * _walkSize) * _detectRange);

            List<Collider2D> playerCols = ColliderChecker.GetColliders(origin, dest, "Player");

            if(playerCols.Count > 0)
            {
                _detectedPlayer = playerCols[0].gameObject.GetComponent<PlayerController>();
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual IEnumerator Response()
        {
            BgmManager.Instance.Stop();
            SoundEvent.Trigger(_detectSound);
            Debug.Log("플레이어 감지됨");
            _reactMark.SetActive(true);
            _isWaiting = false;

            PlayerControllerEvent.Trigger(false, Maths.Vector2ToDirection(-_currentDir));
            
            /// 느낌표!
            yield return Tweens.MoveTransform(this, _reactMark.transform, _reactMark.transform, _reactMarkDest, new WaitForSeconds(0.1f), 0.1f, 1f, Tweens.TweenCurve.EaseInOutBounce);
            _reactMark.SetActive(false);
            yield break;
        }

        /// 상속 받는 클래스가 구현하도록 비워둠
        protected virtual IEnumerator EventRoutine()
        {
            yield break;
        }

        protected virtual IEnumerator MoveToPlayerRoutine()
        {
            for(int i = 0;  i < (int)_detectRange; i++)
            {
                yield return StartCoroutine(MoveRoutine(_currentDir, true));
            }

            yield break;
        }

        protected virtual IEnumerator WaitUntillTalkEnd(float duration = 1f)
        {
            NpcDialogueBox dialogueBox = (NpcDialogueBox)FindObjectOfType(typeof(NpcDialogueBox));

            while(!DialogueManager.Instance._isEnd)
            {
                yield return null;
            }

            yield return new WaitForSeconds(duration);
            yield break;
        }

        protected void EventEndResponse()
        {
            BgmManager.Instance.Play(0);
            _isEventEnd = true;

            Debug.Log("이벤트 끝");
        }

        public void Delete()
        {
            StartCoroutine(DeleteRoutine());
        }

        protected IEnumerator DeleteRoutine()
        {
            while(!_isEventEnd)
            {
                yield return null;
            }

            this.gameObject.SetActive(false);
            yield break;
        }

        protected virtual IEnumerator WaitUntilBattleEnd(float duration = 1f)
        {
            yield return new WaitForSeconds(duration);
            yield break;
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            
            Gizmos.color = Color.red;
            Vector2 origin = ((Vector2)transform.position + _currentDir * _walkSize);
            Vector2 dest = ((Vector2)transform.position + (_currentDir * _walkSize) * _detectRange);
            Gizmos.DrawLine(origin, dest);
        }
    }
}
