using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Move_player : MonoBehaviour
{
    public static int Goal_x;
    public static int Goal_y;

    private void Start()
    {
        Goal_x = Convert.ToInt32(this.gameObject.transform.position.x);
        Goal_y = Convert.ToInt32(this.gameObject.transform.position.y);
    }
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 Player_Pos = this.gameObject.transform.position;
        Vector2 Goal_Pos = new Vector2(Goal_x, Goal_y);

        if ( Player_Pos == Goal_Pos)
        {
            if (Input.GetKey(KeyCode.W))
                Goal_y = Convert.ToInt32(this.gameObject.transform.position.y+1);

            else if (Input.GetKey(KeyCode.A))
                Goal_x = Convert.ToInt32(this.gameObject.transform.position.x-1);

            else if (Input.GetKey(KeyCode.S))
                Goal_y = Convert.ToInt32(this.gameObject.transform.position.y-1);

            else if (Input.GetKey(KeyCode.D))
                Goal_x = Convert.ToInt32(this.gameObject.transform.position.x+1);
        }

        else
        {
            this.gameObject.transform.Translate((Goal_Pos - Player_Pos).normalized * 3 * Time.deltaTime);


            if ( Goal_y > Player_Pos.y || Goal_x > Player_Pos.x)
            {
                if ((Player_Pos.x - Goal_Pos.x > -0.1 && Player_Pos.y - Goal_Pos.y > -0.1))
                    this.gameObject.transform.position = Goal_Pos;
            }

            else if((Player_Pos.x - Goal_Pos.x < 0.1  && Player_Pos.y - Goal_Pos.y < 0.1))
                this.gameObject.transform.position = Goal_Pos;
         }
    }
}