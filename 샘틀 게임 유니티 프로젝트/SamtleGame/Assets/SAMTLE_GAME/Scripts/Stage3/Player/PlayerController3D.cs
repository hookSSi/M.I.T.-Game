using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage3
{
	[RequireComponent(typeof(PlayerInteractive))]
	public class PlayerController3D : MonoBehaviour
	{
		public Transform obj;
		public Transform lookObj;

		private Rigidbody _rigidBody;
		private PlayerInteractive _playerInteractive;
		[SerializeField] private Animator _anim;
		[SerializeField] private float _yRot = 0f;
		[SerializeField] private float _xRot = 0f;
		[SerializeField] private bool _isMoving = false;
		[SerializeField] private bool _isSprinting = false;
		private Vector2 _currentDir = new Vector2();

		[Header("플레이어 시점 카메라"), Space(20)]
		public Transform _camera;

		[Header("플레이어 세팅"), Space(20)]
		public float _sprintSpeed = 4f;
		public float _walkSpeed = 2f;
		public float _mouseSensitivity = 2f;

		[Header("플레이어 상태"), Space(20)]
		public bool _canMove = true;
		public bool _canRotate = true;
		public bool _isSittingOn = false;
		public bool _isFocusing = false;
		public bool _isControllable = true;
		public bool _isIkActive = false;

		[Header("디버깅"), Space(20)]
		public float _playerSpeed;


		void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			_playerSpeed = _walkSpeed;

			_rigidBody = GetComponent<Rigidbody>();
			_playerInteractive = GetComponent<PlayerInteractive>();

			_yRot = transform.eulerAngles.y;
			_xRot = transform.eulerAngles.x;
		}

		void Update()
		{
			HandleInput();
		}

		public void HandleInput()
		{
			if(_isControllable)
			{
				if(_canRotate)
					Rotate(Input.GetAxis("Mouse X") * _mouseSensitivity, Input.GetAxis("Mouse Y") * _mouseSensitivity);

				if (_canMove)
				{
					_currentDir.x = Input.GetAxisRaw("Horizontal");
					_currentDir.y = Input.GetAxisRaw("Vertical");
					Move(_currentDir);

					_anim.SetFloat("Horizontal", _currentDir.x);
					_anim.SetFloat("Vertical", _currentDir.y);

					if(_currentDir.sqrMagnitude > 0)
					{
						SoundEvent.Trigger("걷는소리",SoundStatus.Play, false, 1f);
					}
					// SprintCheck();
				}

				if(_isSittingOn)
				{
					if(Input.GetKeyDown(KeyCode.E))
					{
						_anim.SetTrigger("SitUpChair");
					}
				}
				else
				{
					if (Input.GetKeyDown(KeyCode.E) && !_isFocusing)
					{
						if(GetComponent<PlayerInteractive>()._watchingObj != null)
						{
							Interact();
						}
					}
					else if (_isFocusing && Input.GetKeyDown(KeyCode.E))
					{
						FocusOut();
					}
				}

			}
		}

		private void Rotate(float xRot, float yRot)
		{
			if(_isSittingOn)
			{
				_xRot -= yRot;
				_xRot = Mathf.Clamp(_xRot, -90, 90);
				_camera.transform.localEulerAngles = new Vector3(_xRot, _camera.transform.localEulerAngles.y, _camera.transform.localEulerAngles.z);
			}
			else
			{
				_yRot += xRot;
				transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, _yRot, transform.localEulerAngles.z);
				_xRot -= yRot;
				_xRot = Mathf.Clamp(_xRot, -90, 90);
				_camera.transform.localEulerAngles = new Vector3(_xRot, _camera.transform.localEulerAngles.y, _camera.transform.localEulerAngles.z);
			}
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

		public void SitOnChair()
		{
			_anim.SetTrigger("SitOnChair");
			_isSittingOn = true;
			_canMove = false;
		}

		public void SitUpChair(float rotation)
		{
			_yRot = rotation;
			_isSittingOn = false;
			_canMove = true;
			_playerInteractive.enabled = true;
		}
		
		public void SetMovable(bool canMove)
		{
			_canMove = canMove;
			_canRotate = canMove;
		}

		public void Interact()
		{
			if(_playerInteractive._interactive._isActive)
			{
				_isFocusing = true;
				_playerInteractive.enabled = false;

				_playerInteractive._interactive.Action();
			}
		}

		void OnAnimatorIK()
		{

			if(lookObj != null) {
				_anim.SetLookAtWeight(1);
				_anim.SetLookAtPosition(lookObj.position);
			}   
			if(_isIkActive)
			{
				_anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
				_anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
				_anim.SetIKPosition(AvatarIKGoal.RightHand, obj.position);
				_anim.SetIKRotation(AvatarIKGoal.RightHand, obj.rotation);
			}
			else
			{
				_anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
				_anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
				_anim.SetLookAtWeight(0);
			}
		}

		public void FocusIn()
		{
			_isFocusing = true;
			_playerInteractive.enabled = false;

			Transform obj = _playerInteractive._watchingObj;
			Transform focusObj = _playerInteractive._interactive._focusObj;
			
			FocusingEvent.Trigger(FocusingType.FocusIn, focusObj.transform.position, obj);
			_isIkActive = true;
		}
		public void FocusOut()
		{
			SetMovable(true);
			_isFocusing = false;
			_playerInteractive.enabled = true;

			FocusingEvent.Trigger(FocusingType.FocustOut);
			_isIkActive = false;
		}
	}
}