using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

namespace MIT.SamtleGame.Stage1
{

    public class Player : MonoBehaviour
    {
        public PlayerController _controller;
        public static Vector3 _pos;

        [BoxGroup("플레이어 정보"), ProgressBar("Health", 100, EColor.Red)]
        public int _currentHp = 100;

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
