using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;

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

    [RequireComponent(typeof(Animator))]
    public class Spawner : MonoBehaviour, EventListener<SpawnerEvent>
    {
        /// 스폰 타이밍을 정의한 애니메이터
        public Animator _spawnAnimator;

        [Header("스폰될 적 프리팹 설정")]
        public GameObject _civil;
        public GameObject _pegeon;
        public GameObject _boss;

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

        public void Rest() {}

        protected void Spawn()
        {
            if(_spawnInfoList.Count > 0)
            {
                _currentSpawnType = _spawnInfoList[0]._spawnType;
                _currentSpawnDir = _spawnInfoList[0]._spawnDir;
                _spawnInfoList.RemoveAt(0);
            }

            this.transform.position = new Vector3(_playerPos.transform.position.x, 0, 0);

            GameObject enemy = _civil;
            switch( _currentSpawnType )
            {
                case EnemyType.Civil:
                    enemy = _civil;
                    Debug.Log("시민 나옴");
                    break;
                case EnemyType.Pegeon:
                    enemy = _pegeon;
                    Debug.Log("비둘기 나옴");
                    break;
                case EnemyType.Boss:
                    enemy = _boss;
                    Debug.Log("보스 나옴");
                    break;
                case EnemyType.None:
                    Debug.Log("아무것도 안나옴");
                    return;
            }

            Vector3 pos = _right.position;
            switch( _currentSpawnDir )
            {
                case Direction.Right:
                    pos = _right.position;
                    Instantiate( enemy, pos, Quaternion.Euler( 0, 180, 0 ) );
                    GameManager._totalEnemyCount += 1;
                    break;
                case Direction.Left:
                    pos = _left.position;
                    Instantiate( enemy, pos, Quaternion.Euler( 0, 0, 0 ) );
                    GameManager._totalEnemyCount += 1;
                    break;
            }
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

        private void OnEnable() 
        {
            this.EventStartListening<SpawnerEvent>();
        }

        private void OnDisable() 
        {
            this.EventStopListening<SpawnerEvent>();
        }
    }
}
