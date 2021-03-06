﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;
using NaughtyAttributes;

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
        private Animator _spawnAnimator;
        
        [ShowNonSerializedField]
        private EnemyType _currentSpawnType;
        [ShowNonSerializedField]
        private Direction _currentSpawnDir;

        [BoxGroup("스폰 될 위치 설정")] 
        public Transform _playerPos;
        [BoxGroup("스폰 될 위치 설정")] 
        public Transform _right;
        [BoxGroup("스폰 될 위치 설정")] 
        public Transform _left;

        [BoxGroup("스폰 정보"), ReorderableList] 
        public List<SpawnInfo> _spawnInfoList = new List<SpawnInfo>();

        [HideInInspector] public List<GameObject> _civil = new List<GameObject>();
        [HideInInspector] public List<GameObject> _pegeon = new List<GameObject>();
        [HideInInspector] public List<GameObject> _boss = new List<GameObject>();

        public void PauseSpawn()
        {
            if(_spawnAnimator == null)
                _spawnAnimator = GetComponent<Animator>();

            _spawnAnimator.enabled = false;
        }

        public void StartSpawn()
        {
            if(_spawnAnimator == null)
                _spawnAnimator = GetComponent<Animator>();

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
                    enemy = _civil[Random.Range(0, _civil.Count)];
                    Debug.Log("시민 나옴");
                    break;
                case EnemyType.Pegeon:
                    enemy = _pegeon[Random.Range(0, _pegeon.Count)];;
                    Debug.Log("비둘기 나옴");
                    break;
                case EnemyType.Boss:
                    enemy = _boss[Random.Range(0, _boss.Count)];
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

        public struct EnemyCreationParams
        {
            public string _path;
        }
    }

}
