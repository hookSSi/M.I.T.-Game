using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;
using NaughtyAttributes;

namespace MIT.SamtleGame.Stage1
{
    public class Door : MonoBehaviour
    {
        [Scene] public string _nextScene;
        [Scene] public string _loadingScene;

        private void OnTriggerStay2D(Collider2D other) 
        {
            LoadingSceneManager.LoadScene(_nextScene, _loadingScene);
        }
    }
}
