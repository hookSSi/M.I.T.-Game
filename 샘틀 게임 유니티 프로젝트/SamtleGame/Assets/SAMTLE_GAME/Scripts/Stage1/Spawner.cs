using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIT.SamtleGame.Tools;

/// 스포너도 인스펙터 수정 필요

namespace MIT.SamtleGame.Stage1
{
    public enum EnemyType { Civil, Pegeon, Boss, None }
    public enum Direction { Right, Left }

    [System.Serializable]
    public struct SpawnInfo
    {
        public EnemyType _spawnType;
        public Direction _spawnDir;
    }

    public enum SpawnerState { Play, Pasue }

    public struct SpawnerEvent
    {
        public SpawnerState _state;
        public int _id;

        SpawnerEvent(SpawnerState state = SpawnerState.Play, int id = 0)
        {
            _id = id;
            _state = state;
        }

        public static SpawnerEvent _event;

        public static void Trigger(SpawnerState state = SpawnerState.Play, int id = 0)
        {
            _event._id = id;
            _event._state = state;
            EventManager.TriggerEvent(_event);
        }
    }

    public struct SpawnEvent
    {
        public int _id;
        public Direction _dir;
        public EnemyType _type;

        SpawnEvent(Direction dir, EnemyType type, int id = 0)
        {
            _dir = dir;
            _type = type;
            _id = id;
        }

        public static SpawnEvent _event;

        public static void Trigger(Direction dir, EnemyType type, int id = 0)
        {
            _event._dir = dir;
            _event._type = type;
            _event._id = id;
            EventManager.TriggerEvent(_event);
        }
    }

    [RequireComponent(typeof(Animator))]
    public class Spawner : MonoBehaviour, EventListener<SpawnerEvent>, EventListener<SpawnEvent>
    {
        /// 스폰 타이밍을 정의한 애니메이터
        public Animator _spawnAnimator;

        [Header("스폰될 적 프리팹 설정")]
        public GameObject[] _civil;
        public GameObject[] _pegeon;
        public GameObject[] _boss;

        [Header("스폰 될 위치")]
        public Transform _playerPos;
        public Transform _right;
        public Transform _left;

        [Header("현재 스폰 정보")]
        public List<SpawnInfo> _spawnInfoList = new List<SpawnInfo>();
        [SerializeField]
        private EnemyType _currentSpawnType;
        [SerializeField]
        private Direction _currentSpawnDir;

        public void PauseSpawn()
        {
            _spawnAnimator.enabled = false;
        }

        public void StartSpawn()
        {
            _spawnAnimator.enabled = true;
            _spawnAnimator.Play("SpawnTest", -1, 0f);
        }

        public void Reset() {}

        protected void Spawn()
        {
            if(_spawnInfoList.Count > 0)
            {
                _currentSpawnType = _spawnInfoList[0]._spawnType;
                _currentSpawnDir = _spawnInfoList[0]._spawnDir;
                _spawnInfoList.RemoveAt(0);
            }
            else
            {
                int rndNum = Random.Range(0, 100);

                if(rndNum <= 60)
                    _currentSpawnType = EnemyType.Civil;
                else
                    _currentSpawnType = EnemyType.Pegeon;
                
                if(rndNum <= 50)
                    _currentSpawnDir = Direction.Right;
                else
                    _currentSpawnDir = Direction.Left;
            }

            Spawn(_currentSpawnDir, _currentSpawnType);
        }

        public void Spawn(Direction dir, EnemyType type) 
        {
            this.transform.position = new Vector3(_playerPos.transform.position.x, 0, 0);

            GameObject enemy = null;
            switch( type )
            {
                case EnemyType.Civil:
                    enemy = _civil[Random.Range(0, _civil.Length)];
                    Debug.Log("시민 나옴");
                    break;
                case EnemyType.Pegeon:
                    enemy = _pegeon[Random.Range(0, _pegeon.Length)];;
                    Debug.Log("비둘기 나옴");
                    break;
                case EnemyType.Boss:
                    enemy = _boss[Random.Range(0, _boss.Length)];
                    break;
                case EnemyType.None:
                    Debug.Log("아무것도 안나옴");
                    return;
            }

            Enemy enemyObj = null;
            switch( dir )
            {
                /// 오른쪽에서 스폰 될 경우
                case Direction.Right:
                    enemyObj = Instantiate( enemy, _right.position, _right.rotation ).GetComponent<Enemy>();
                    enemyObj.SetDirection(Vector2.left);
                    break;
                /// 왼쪽에서 스폰 될 경우
                case Direction.Left:
                    enemyObj = Instantiate( enemy, _left.position, _left.rotation ).GetComponent<Enemy>();
                    enemyObj.SetDirection(Vector2.right);
                    break;
            }

            GameManager._totalEnemyCount += 1;
        }

        private void OnDrawGizmos() 
        {
            Gizmos.DrawIcon(_right.position, "Spawner.png");
            Gizmos.DrawIcon(_left.position, "Spawner.png");
        }

        public virtual void OnEvent(SpawnerEvent spawnerEvent)
        {
            switch(spawnerEvent._state)
            {
                case SpawnerState.Play:
                    StartSpawn();
                    break;
                case SpawnerState.Pasue:
                    PauseSpawn();
                    break;
            }
        }
        public virtual void OnEvent(SpawnEvent spawnEvent)
        {
            Spawn(spawnEvent._dir, spawnEvent._type);
        }

        private void OnEnable() 
        {
            this.EventStartListening<SpawnerEvent>();
            this.EventStartListening<SpawnEvent>();
        }

        private void OnDisable() 
        {
            this.EventStopListening<SpawnerEvent>();
            this.EventStopListening<SpawnEvent>();
        }
    }
}
