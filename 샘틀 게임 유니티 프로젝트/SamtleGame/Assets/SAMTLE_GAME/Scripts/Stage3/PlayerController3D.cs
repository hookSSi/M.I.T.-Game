﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage3
{
	public class PlayerController3D : MonoBehaviour
	{
		// private Animator anim;
		private Rigidbody _rigidBody;
		[SerializeField] private Animator _anim;
		[SerializeField] private float _yRot = 0f;
		[SerializeField] private float _xRot = 0f;
		[SerializeField] private bool _isMoving = false;
		[SerializeField] private bool _isSprinting = false;
		private Vector2 _currentDir = new Vector2();

		[Header("플레이어 시점 카메라")]
		public Transform _camera;
		public FocusingObjectCameraController _focusingCamera;
		[Header("플레이어 세팅")]
		public float _sprintSpeed = 4f;
		public float _walkSpeed = 2f;
		public float _mouseSensitivity = 2f;
		[Header("플레이어 상태")]
		public bool _canMove = true;
		public bool _isFocusing = false;
		public bool _isControllable = true;
		[Header("디버깅")]
		public float _playerSpeed;


		void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			_playerSpeed = _walkSpeed;
			_rigidBody = GetComponent<Rigidbody>();
			_yRot = transform.eulerAngles.y;
			_xRot = transform.eulerAngles.x;
			_canMove = true;
			_isFocusing = false;
		}

		// Update is called once per frame
		void Update()
		{
			HandleInput();
		}

		public void HandleInput()
		{
			if(_isControllable)
			{
				if (_canMove)
				{
					Rotate(Input.GetAxis("Mouse X") * _mouseSensitivity, Input.GetAxis("Mouse Y") * _mouseSensitivity);
					_currentDir.x = Input.GetAxisRaw("Horizontal");
					_currentDir.y = Input.GetAxisRaw("Vertical");
					Move(_currentDir);

					_anim.SetFloat("Horizontal", _currentDir.x);
					_anim.SetFloat("Vertical", _currentDir.y);
					// SprintCheck();
				}

				if (Input.GetKeyDown(KeyCode.E) && !_isFocusing)
				{
					if(GetComponent<PlayerInteractive>().watchingObj != null)
					{
						FocusObject();
					}
				}
				else if (_isFocusing && Input.GetKeyDown(KeyCode.E))
				{
					FocusOut();
				}
			}
		}

		private void Rotate(float xRot, float yRot)
		{
			_yRot += xRot;
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, _yRot, transform.localEulerAngles.z);
			_xRot -= yRot;
			_xRot = Mathf.Clamp(_xRot, -90, 90);
			_camera.transform.localEulerAngles = new Vector3(_xRot, _camera.transform.localEulerAngles.y, _camera.transform.localEulerAngles.z);
		}
		private void Move(Vector2 dir)
		{
			_rigidBody.velocity += transform.right * dir.x * _playerSpeed;
			_rigidBody.velocity += transform.forward * dir.y * _playerSpeed;
		}
		private void SprintCheck()
		{
			if (Input.GetAxisRaw("Sprint") > 0f)
			{
				_playerSpeed = _sprintSpeed;
				_isSprinting = true;
			}
			else if (Input.GetAxisRaw("Sprint") < 1f)
			{
				_playerSpeed = _walkSpeed;
				_isSprinting = false;
			}
		}
		public void FocusObject()
		{
			_canMove = false;
			_isFocusing = true;
			GetComponent<PlayerInteractive>().enabled = false;
			Transform obj = GetComponent<PlayerInteractive>().watchingObj;
			//this has to be fixed
			_focusingCamera.FocusIn(obj.GetChild(0).position, obj);
			obj.GetComponent<Interactive>().Action();
			//
		}
		public void FocusOut()
		{
			_canMove = true;
			_isFocusing = false;
			GetComponent<PlayerInteractive>().enabled = true;
			_focusingCamera.FocusOut();
		}
	}
}
