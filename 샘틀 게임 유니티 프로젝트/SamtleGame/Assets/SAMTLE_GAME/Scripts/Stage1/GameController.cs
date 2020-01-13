using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Text textScore;
    public GameObject enemy;
    private float spawnDelay = 0.8f;

    private int myScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemy()
    {
        int randomSpawnDelay = UnityEngine.Random.Range(2, 5);
        int randomEnemyNumber = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < randomEnemyNumber; i++)
        {
            int randomEnemyDirection = UnityEngine.Random.Range(1, 3);
            Vector3 spawnPosition = new Vector3(0, -1.724f, 0);
            switch (randomEnemyDirection)
            {
                case 1:
                    spawnPosition.x = 10.55f;   // 오른쪽
                    Instantiate(enemy, spawnPosition, Quaternion.Euler(0, 180, 0));
                    break;
                case 2:
                    spawnPosition.x = -10.55f;  // 왼쪽
                    Instantiate(enemy, spawnPosition, Quaternion.Euler(0, 0, 0));
                    break;
            }
            yield return new WaitForSeconds(spawnDelay);
        }
        yield return new WaitForSeconds(randomSpawnDelay);
        StartCoroutine(SpawnEnemy());
    }

    public void RisingScore(int score)
    {
        myScore += score;
        textScore.text = "SCORE : " + myScore.ToString();
    }
}
