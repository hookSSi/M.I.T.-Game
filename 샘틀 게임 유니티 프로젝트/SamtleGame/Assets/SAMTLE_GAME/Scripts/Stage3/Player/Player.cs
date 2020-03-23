using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage3
{
    public class Player : MonoBehaviour
    {
        public GameObject _playerModel;
        public PlayerController3D _controller;

        public float _smooth = 0.3f;
        private float _yVelocity = 0.0f;

        private void LateUpdate() {
            _playerModel.transform.localPosition = Vector3.zero;

            // 플레이어 모델 방향 보정
            if (_controller._canMove)
            {
                float yAngle = Mathf.SmoothDampAngle(_playerModel.transform.localRotation.eulerAngles.y, 0f, ref yVelocity, smooth);
                _playerModel.transform.localRotation = Quaternion.Euler(0, yAngle, 0);
            }
        }
    }   
}
