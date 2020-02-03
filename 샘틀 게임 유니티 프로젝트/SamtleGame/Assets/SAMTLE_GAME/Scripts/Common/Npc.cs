using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.NPC
{
    public enum NpcAnimState { Idle = 0, Walk = 1} 

    public class Npc : MonoBehaviour
    {
        private int  _currentWalkCount = 0;
        private bool _isTalking = false;

        [Header("Identification")]
        public int _id = 0;
        [Header("대사 UI 프리팹")]
        public GameObject _dialogueUIPrefab;
        [Header("대사 목록")]
        public List<string> _textPages;
        [Header("Event")]
        [SerializeField]
        private DialogueEvent _talkEvent;

        [Header("NPC의 행동을 정의")]
        [SerializeField]
        private Action _act;
        [Header("현재 Npc 방향")]
        [SerializeField]
        protected Vector2 _currentDir;

        [Header("Npc 이동 정보")]
        public float _walkSize = 1;
        public int _walkCount = 10;
        public float _walkTime = 0.1f;
        public float _speed = 1;
        public Vector2 _colSize;

        protected virtual void Initialization() 
        {
            DialogueManager.AddNpc(this);
        }

        private void Start() 
        {
            Initialization();
        }

        public string GetText(int index)
        {
            return _textPages[index];
        }
        public void SetText(int index, string text)
        {
            _textPages[index] = text;
        }

        public virtual void Talk()
        {
            if(!_isTalking)
            {
                DialogueEvent.Trigger(_talkEvent._id);
                _isTalking = true;
            }
            else
            {
                NextPage();
            }
            
        }

        public void NextPage()
        {
            DialogueEvent.Trigger(_id, DialogueStatus.Next);
        }

        protected void SetDirection(Direction dir)
        {
            switch(dir)
            {
                case Direction.UP:
                    _currentDir = Vector2.up;
                    break;
                case Direction.DONW:
                    _currentDir = Vector2.down;
                    break;
                case Direction.RIGHT:
                    _currentDir = Vector2.right;
                    break;
                case Direction.LEFT:
                    _currentDir = Vector2.left;
                    break;
                case Direction.NONE:
                    break;
            }
        }

        /// 이동처리
        IEnumerator MoveRoutine(Vector2 dir)
        {
            Vector2 walkAmount = dir * ( _walkSize / _walkCount ) * _speed;

            while(true)
            {
                this.transform.Translate(walkAmount);
                _currentWalkCount++;

                if(_currentWalkCount == _walkCount)
                {
                    _currentWalkCount = 0;
                    yield break;
                }

                yield return new WaitForSeconds(_walkTime);
            }
        }

        /// 미리 정의된 행동 목록 처리
        IEnumerator ActionRoutine()
        {
            foreach(var dir in _act.moveDirs)
            {
                this.SetDirection(dir);
                yield return StartCoroutine(MoveRoutine(_currentDir));
            }

            yield break;
        }
    }
}
