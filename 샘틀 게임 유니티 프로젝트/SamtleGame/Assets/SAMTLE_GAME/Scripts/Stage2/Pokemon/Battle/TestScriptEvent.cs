using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptEvent : MonoBehaviour
{
    public void OnSubmit()
    {
        Debug.Log(gameObject.name + "를 발동한다!");
    }

    public void OnCancel()
    {
        Debug.Log("캔슬 발동, 돌아간다!");
    }
}
