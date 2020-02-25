using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerStage3 : MonoBehaviour
{
    [SerializeField]
    private float _walkSpeed;

    [SerializeField]
    private float _lookSensitivity;

    [SerializeField]
    private float _cameraRotationLimit;

    private float _currentCameraRotationX = 0;

    [SerializeField]
    private Camera _camera;

    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void Move()
    {

        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * _walkSpeed;

        _rigidbody.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    //마우스 좌우, 캐릭터와 함깨 돌아감
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * _lookSensitivity;

        _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(_characterRotationY));
    }

    //마우스 상하, 카메라만 돌아감
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * _lookSensitivity;

        _currentCameraRotationX -= _cameraRotationX;
        _currentCameraRotationX = Mathf.Clamp(_currentCameraRotationX, -_cameraRotationLimit, _cameraRotationLimit);

        _camera.transform.localEulerAngles = new Vector3(_currentCameraRotationX, 0f, 0f);
    }

}
