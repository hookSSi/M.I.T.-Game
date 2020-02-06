using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Stage2.Tool;
using MIT.SamtleGame.Tools;

namespace MIT.SamtleGame.Stage2.NPC
{
    /*
    *   @desc 이벤트를 가진 Npc
    */
    public class EventNpc : Npc
    {
        public bool _isWaiting = true;

        [Header("플레이어 감지 범위")]
        public float _detectRange; // 발견 범위
        public string _detectSound; // 발견했을 때 사운드
        public GameObject _reactMark; // 발견했을 때 표시
        public Transform _reactMarkDest; // 느낌표 목표 위치

        protected override void Initialization() 
        {
            base.Initialization();

            if(_isWaiting)
                _reactMark.SetActive(false);
        }

        protected virtual void Update()
        {
            if(_isWaiting)
            {
                if(DetectPlayer())
                {
                    EventProcess();  
                }
            }
        }

        protected virtual bool DetectPlayer()
        {
            Vector2 origin = ((Vector2)transform.position + _currentDir * _walkSize);
            Vector2 dest = ((Vector2)transform.position + (_currentDir * _walkSize) * _detectRange);

            if(ColliderChecker.CheckColliders(origin, dest, "Player", true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void EventProcess()
        {
            SoundEvent.Trigger("발견");
            Debug.Log("플레이어 감지됨");
            _reactMark.SetActive(true);
            _isWaiting = false;
            StartCoroutine(EventRoutine());
        }

        protected virtual IEnumerator EventRoutine()
        {
            yield return Tweens.MoveTransform(this, _reactMark.transform, _reactMark.transform, _reactMarkDest, new WaitForSeconds(0.1f), 0.1f, 1f, Tweens.TweenCurve.EaseInOutBounce);
            Debug.Log("이벤트 끝");
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
