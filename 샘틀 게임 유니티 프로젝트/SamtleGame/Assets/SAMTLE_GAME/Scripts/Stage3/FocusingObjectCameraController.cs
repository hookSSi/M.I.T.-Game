using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
public class FocusingObjectCameraController : MonoBehaviour
{
	private Cinemachine.CinemachineVirtualCamera _vc;
	[SerializeField]
	private Transform _lookAt;
	[SerializeField]
	private Vector3 _pos;
	void Start()
    {
		_vc = GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    public void FocusIn(Vector3 pos, Transform target)
	{
		_vc.transform.position = pos;
		_vc.LookAt = target;
		_vc.Priority = 11;
	}

	public void FocusOut()
	{
		_vc.Priority = 9;
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (_vc.Priority == 9)
				FocusIn(_pos, _lookAt);
			else
				FocusOut();
		}
	}
}
