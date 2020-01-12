using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBgm : MonoBehaviour
{
    BgmManager BGM;

    public int playMusicTrack;

    void Start()
    {
        BGM = FindObjectOfType<BgmManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            BGM.Play(playMusicTrack);
            this.gameObject.SetActive(false);
            
        }
    }
}
