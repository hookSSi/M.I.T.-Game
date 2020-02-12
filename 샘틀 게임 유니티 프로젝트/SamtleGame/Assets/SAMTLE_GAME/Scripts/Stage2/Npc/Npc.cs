using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Stage2.Tools;
using MIT.SamtleGame.Tools;

namespace MIT.SamtleGame.Stage2.NPC
{
    [SelectionBase]
    public class Npc : InteracterbleObject
    {
        private int  _currentWalkCount = 0;
        
        /// 애니메이션
        protected Animator _animator;

        [Header("Identification")]
        public int _id = 0;

        [Header("대사 UI 프리팹")]
        public GameObject _dialogueUIPrefab;

        [Header("대사")]
        public List<DialoguePage> _textPages;
        public string _talkSound;

        [Header("Event")]
        [SerializeField]
        private DialogueEvent _talkEvent;


        [Header("현재 Npc 방향")]
        [SerializeField]
        protected Vector2 _currentDir;

        [Header("Npc 정보")]
        public float _walkSize = 1;
        public int _walkCount = 10;
        public float _walkTime = 0.1f;
        public float _speed = 1;
        public Tag[] _obstacles;

        [Header("NPC 웨이포인트")]
        [SerializeField]
        public Transform _nodeStorage;
        public GameObject _nodePrefab;
        public List<Transform> _wayPoints = new List<Transform>();
        [ColorUsage(false)]
        public Color _wayPointsGizmoColor = Color.yellow;

        #region 초기화
        protected virtual void Initialization() 
        {
            _talkEvent._id = _id;
            DialogueManager.AddNpc(this);
            _animator = GetComponent<Animator>();
        }
        #endregion

        private void Start() 
        {
            Initialization();
        }

        public virtual void Talk(bool isControllable = true)
        {
            PlayerControllerEvent.Trigger(false);
            DialogueEvent.Trigger(_talkEvent._id, _talkSound, DialogueStatus.Start, 0, isControllable);  
        }

        #region  이동처리
        public void SetDirection(Vector2 dir)
        {
            _currentDir = dir;
        }

        protected virtual IEnumerator MoveRoutine(Vector2 dir, bool isBlock = false)
        {
            if(isBlock)
            {
                Vector2 origin = ((Vector2)transform.position);
                Vector2 dest = ((Vector2)transform.position + _currentDir * _walkSize);

                if(ColliderChecker.CheckColliders(dest, dest, _obstacles))
                {
                    yield break;
                }
            }

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
        #endregion

        #region  웨이포인트 이동
        protected virtual IEnumerator WayPointsMoveRoutine()
        {
            foreach(var dest in _wayPoints)
            {
                Direction dir = Maths.Vector2ToDirection(dest.position - transform.position);
                _currentDir = Maths.DirectionToVector2(dir);

                /// 웨이포인트로 이동
                float walkAmount = ( _walkSize / _walkCount ) * _speed;
                while( Vector2.Distance(dest.position, transform.position) > walkAmount * 2 )
                {
                    yield return StartCoroutine(MoveRoutine(_currentDir));
                }
            }

            yield break;
        }
        #endregion

        #region  상호작용
        public override void Interact() 
        {
            Talk();
        }
        #endregion

        #region  스크립트 사용 편의성 관련
        public void GenerateNodes()
        {
            for(var i = _wayPoints.Count - 1; i > -1; i--)
            {
                if (_wayPoints[i] == null)
                    _wayPoints.RemoveAt(i);
                else
                    _wayPoints[i].name = string.Format("노드 - {0}", i + 1);
            }

            var newNode = Instantiate(_nodePrefab);
            newNode.transform.SetParent(_nodeStorage);
            newNode.transform.localPosition = new Vector3(0f, 0f, 0f);
            newNode.transform.localRotation = Quaternion.identity;
            _wayPoints.Add(newNode.transform);

            newNode.name = string.Format("노드 - {0}", _wayPoints.Count);
        }

        protected virtual void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.green;
            Vector2 point = ((Vector2)transform.position + _currentDir * _walkSize);
            Gizmos.DrawCube(point, Vector2.one / 2);

            Gizmos.color = _wayPointsGizmoColor;
            var prevNode = this.transform;
            foreach(var node in _wayPoints)
            {
                Gizmos.DrawLine(prevNode.position, node.position);
                prevNode = node;
            }
        }
        #endregion
    }
}
