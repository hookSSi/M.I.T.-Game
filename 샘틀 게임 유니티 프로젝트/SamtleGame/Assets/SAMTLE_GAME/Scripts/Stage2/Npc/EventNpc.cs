using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Stage2.Tools;
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
            SoundEvent.Trigger(_detectSound);
            Debug.Log("플레이어 감지됨");
            _reactMark.SetActive(true);
            _isWaiting = false;
            StartCoroutine(EventRoutine());
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
