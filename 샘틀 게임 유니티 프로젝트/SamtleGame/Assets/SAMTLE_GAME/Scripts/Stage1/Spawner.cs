using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MIT.SamtleGame.Stage1
{
    [System.Serializable]
    public enum EnemyType { Civil, Pegeon, Boss, None }
    [System.Serializable]
    public enum Direction { Right, Left }

    public class Spawner : MonoBehaviour
    {
        [Header("스폰될 적 프리팹 설정")]
        public GameObject _civil;
        public GameObject _pegeon;
        public GameObject _boss;

        [Header("스폰 될 위치")]
        public Transform _playerPos;
        public Transform _right;
        public Transform _left;

        [Header("현재 스폰 정보")]
        [SerializeField]
        private EnemyType _currentSpawnType;
        [SerializeField]
        private Direction _currentSpawnDir;

        protected void Spawn()
        {
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
                    break;
                case Direction.Left:
                    pos = _left.position;
                    Instantiate( enemy, pos, Quaternion.Euler( 0, 0, 0 ) );
                    break;
            }
        }

        private void OnDrawGizmos() 
        {
            Gizmos.DrawIcon(_right.position, "Spawner.png");
            Gizmos.DrawIcon(_left.position, "Spawner.png");
        }
    }
}
