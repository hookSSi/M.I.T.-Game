using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;

public class Door : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other) 
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            LoadingSceneManager.LoadScene("창원중앙역", "Stage1LoadingScreen");
        }
    }
}
