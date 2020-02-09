using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Stage2.Tools;

namespace MIT.SamtleGame.Stage2.NPC
{
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
        public List<string> _textPages;
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

        public virtual void Talk()
        {
            PlayerControllerEvent.Trigger(false);
            DialogueEvent.Trigger(_talkEvent._id, _talkSound);  
        }

        #region  이동처리
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

        public void SetDirection(Vector2 dir)
        {
            _currentDir = dir;
        }

        protected virtual IEnumerator MoveRoutine(Vector2 dir, bool isBlock = false)
        {
            if(isBlock)
            {
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
        protected virtual IEnumerator WayPointsRoutine()
        {
            foreach(var dest in _wayPoints)
            {
                Direction dir = DirectionDecision(dest.position);
                SetDirection(dir);
                yield return StartCoroutine(MoveToWayPointRoutine(_currentDir, dest));
                Destroy(dest.gameObject);
            }

            yield break;
        }

        protected IEnumerator MoveToWayPointRoutine(Vector2 dir, Transform dest)
        {
            while( Vector3.Distance(dest.position, transform.position) > 0.1f )
            {
                yield return StartCoroutine(MoveRoutine(dir));
            }

            yield break;
        }
        
        protected Direction DirectionDecision(Vector3 dest)
        {
            Direction result = Direction.NONE;
            
            Vector2 dirVector = dest - transform.position;
            result = Action.VectorToDir(dirVector);

            return result;
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
