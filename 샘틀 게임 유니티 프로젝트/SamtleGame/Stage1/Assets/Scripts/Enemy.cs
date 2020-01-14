using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemySpeed = 1.5f;
    public float enemyForce = 5;

    private const int walk = 0;
    private const int attack = 1;
    private int state = walk;
    private Rigidbody2D rigid;
    private GameObject gameController;
    private BoxCollider2D boxColider;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController");
        rigid = GetComponent<Rigidbody2D>();
        boxColider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == walk)
        {
            transform.Translate(Vector2.right * enemySpeed * Time.deltaTime);
            state = walk;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Attacked(transform, enemyForce);
            state = attack;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log(collision);
        if (collision.transform.tag == "Player")
        {
            state = walk;
        }
    }

    public void TakeDamage(Transform collisionObjectTransform, float playerForce)
    {
        gameController.GetComponent<GameController>().RisingScore(100);
        Vector2 direction = transform.position - collisionObjectTransform.position;
        direction.y = 0.5f;
        direction = direction.normalized * enemyForce;
        transform.Translate(0, 0, -1);
        boxColider.isTrigger = true;
        rigid.AddForce(direction, ForceMode2D.Impulse);
        StartCoroutine(DestoyEnemy());
    }

    private IEnumerator DestoyEnemy()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
