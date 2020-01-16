using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

using MIT.SamtleGame.DesignPattern;

namespace MIT.SamtleGame.Stage1
{
    public class GameController : Singleton<GameController>
    {
        [Header("스코어")]
        [SerializeField]
        private int _myScore = 0;
        [Header("스폰 딜레이")]
        private float _spawnDelay = 0.8f;
        public Text _textScore;
        public GameObject _enemy;
        public Vector3 _spawnPosRight = new Vector3(10.55f, -1.724f, 0);
        public Vector3 _spawnPosLeft = new Vector3(-10.55f, -1.724f, 0);


        void Initialization()
        {
            _myScore = 0;
        }

        void Start()
        {
            Initialization();
            StartCoroutine(SpawnEnemy());
        }

        IEnumerator SpawnEnemy()
        {
            int randomSpawnDelay = UnityEngine.Random.Range(2, 5);
            int randomEnemyNumber = UnityEngine.Random.Range(1, 3);
            
            for (int i = 0; i < randomEnemyNumber; i++)
            {
                int randomEnemyDirection = UnityEngine.Random.Range(1, 3);
                switch (randomEnemyDirection)
                {
                    case 1:
                        Instantiate(_enemy, _spawnPosRight, Quaternion.Euler(0, 180, 0));
                        break;
                    case 2:
                        Instantiate(_enemy, _spawnPosLeft, Quaternion.Euler(0, 0, 0));
                        break;
                }
                yield return new WaitForSeconds(_spawnDelay);
            }
            yield return new WaitForSeconds(randomSpawnDelay);
            StartCoroutine(SpawnEnemy());
        }

        public void RisingScore(int score)
        {
            _myScore += score;
            _textScore.text = "SCORE : " + _myScore.ToString();
        }
    }   
}
