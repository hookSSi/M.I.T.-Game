using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Playermovement : MonoBehaviour
{

    public static int _destX;
    public static int _destY;

    //sounds
    public string walkSound;
    private AudioManager theAudio;
    //


    private void Start()
    {
        _destX = Convert.ToInt32(this.gameObject.transform.position.x);
        _destY = Convert.ToInt32(this.gameObject.transform.position.y);

        theAudio = FindObjectOfType<AudioManager>();
    }
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 playerPos = this.gameObject.transform.position;
        Vector2 goalPos = new Vector2(_destX, _destY);

        #region 키입력 dest 설정
        if (playerPos == goalPos)
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


        #region dest 추적 실제 움직임
        else
        {
            this.gameObject.transform.Translate( ( goalPos - playerPos ).normalized * 2 * Time.deltaTime );

            if (_destY > playerPos.y || _destX > playerPos.x)
            {
                if (playerPos.x - goalPos.x > -0.1 && playerPos.y - goalPos.y > -0.1)
                {
                    this.gameObject.transform.position = goalPos;
                }
            }
            else if (playerPos.x - goalPos.x < 0.1 && playerPos.y - goalPos.y < 0.1)
            {
                this.gameObject.transform.position = goalPos;
            }

            #region walk sound
            theAudio.Play(walkSound);

            theAudio.SetVolume(walkSound, 0.5f);

            /*
            theAudio.Stop(walkSound);
            theAudio.SetLoop(walkSound);
            theAudio.SetLoopCencle(walkSound);
            */

            #endregion
        }
        #endregion
    }
}