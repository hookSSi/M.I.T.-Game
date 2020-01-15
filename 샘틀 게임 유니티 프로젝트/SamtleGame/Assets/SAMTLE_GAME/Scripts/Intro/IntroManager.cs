using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    void Start()
    {
        BgmManager.Instance.Play(0);
        BgmManager.Instance.SetVolume(0.15f);
    }
}
