using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage1
{

    public class Player : MonoBehaviour
    {
        private Vector3 _pos; 

        void Update() 
        {
            _pos = this.gameObject.transform.position;
        }

        public Vector3 Pos { get { return _pos; } }
    }
}
