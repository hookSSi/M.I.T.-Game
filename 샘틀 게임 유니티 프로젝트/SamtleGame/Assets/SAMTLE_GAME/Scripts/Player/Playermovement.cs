using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Playermovement : MonoBehaviour
{
    public static int _destX;
    public static int _destY;

    private void Start()
    {
        _destX = Convert.ToInt32(this.gameObject.transform.position.x);
        _destY = Convert.ToInt32(this.gameObject.transform.position.y);
    }
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 _playerPos = this.gameObject.transform.position;
        Vector2 _goalPos = new Vector2(_destX, _destY);


        #region 키입력
        if (_playerPos == _goalPos)
        {
            if (Input.GetKey(KeyCode.W))
                _destY = Convert.ToInt32(this.gameObject.transform.position.y + 1);
            else if (Input.GetKey(KeyCode.A))
                _destX = Convert.ToInt32(this.gameObject.transform.position.x - 1);
            else if (Input.GetKey(KeyCode.S))
                _destY = Convert.ToInt32(this.gameObject.transform.position.y - 1);
            else if (Input.GetKey(KeyCode.D))
                _destX = Convert.ToInt32(this.gameObject.transform.position.x + 1);
        }
        #endregion

        #region dest 추적
        else
        {
            this.gameObject.transform.Translate((_goalPos - _playerPos).normalized * 3 * Time.deltaTime);

            if (_destY > _playerPos.y || _destX > _playerPos.x)
            {
                if (_playerPos.x - _goalPos.x > -0.1 && _playerPos.y - _goalPos.y > -0.1)
                    this.gameObject.transform.position = _goalPos;
            }

            else if ((_playerPos.x - _goalPos.x < 0.1 && _playerPos.y - _goalPos.y < 0.1))
                this.gameObject.transform.position = _goalPos;
        }
        #endregion

    }
}