using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
	[Header("플레이어 시점 카메라")]
	public Transform camera;

	[Header("플레이어 세팅")]
	public float sprintSpeed = 4f;
	public float walkSpeed = 2f;
	public float mouseSensitivity = 2f;
	[Header("디버깅")]
	public bool canMove = true;
	public float playerSpeed;
	[SerializeField] private float yRot = 0f;
	[SerializeField] private float xRot = 0f;
	[SerializeField] private bool isMoving = false;
	[SerializeField] private bool isSprinting = false;

	// private Animator anim;
	private Rigidbody rigidBody;
	// Use this for initialization
	void Start()
	{
		playerSpeed = walkSpeed;
		// anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		if (canMove)
		{
			Rotate();
			Move();
			// SprintCheck();
		}
		// anim.SetBool("isMoving", isMoving);
		// anim.SetBool("isSprinting", isSprinting);
	}
	private void Rotate()
	{
		yRot += Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRot, transform.localEulerAngles.z);
		xRot -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		xRot = Mathf.Clamp(xRot, -90, 90);
		camera.transform.localEulerAngles = new Vector3(xRot, camera.transform.localEulerAngles.y, camera.transform.localEulerAngles.z);
	}
	private void Move()
	{
		isMoving = false;

		if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
		{
			// transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * playerSpeed);
			rigidBody.velocity += transform.right * Input.GetAxisRaw("Horizontal") * playerSpeed;
			isMoving = true;
		}
		if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
		{
			// transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * playerSpeed);
			rigidBody.velocity += transform.forward * Input.GetAxisRaw("Vertical") * playerSpeed;
			isMoving = true;
		}
	}
	private void SprintCheck()
	{
		if (Input.GetAxisRaw("Sprint") > 0f)
		{
			playerSpeed = sprintSpeed;
			isSprinting = true;
		}
		else if (Input.GetAxisRaw("Sprint") < 1f)
		{
			playerSpeed = walkSpeed;
			isSprinting = false;
		}
	}
}
