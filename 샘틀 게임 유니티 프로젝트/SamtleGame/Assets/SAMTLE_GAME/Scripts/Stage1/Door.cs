using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;

namespace MIT.SamtleGame.Stage1
{
    public class Door : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other) 
        {
            LoadingSceneManager.LoadScene("창원중앙역", "Stage1LoadingScreen");
        }
    }
}
