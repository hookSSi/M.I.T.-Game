using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage1
{

    public class Player : MonoBehaviour
    {
        public PlayerController _controller;
        private Vector3 _pos;

        private void Start() 
        {
            _controller = GetComponent<PlayerController>();
        }

        void Update() 
        {
            _pos = this.gameObject.transform.position;
        }

        public Vector3 Pos { get { return _pos; } }
    }
}
