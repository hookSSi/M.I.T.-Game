using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
	public GameObject targetCamera;
	public float time;

	[SerializeField]
	private Vector3 _prevPos;
	[SerializeField]
	private Vector3 _prevRot;

	private bool _isCoroutineRunning;
	private Coroutine _runningCoroutine;
    // Start is called before the first frame update
    void Start()
    {
		_isCoroutineRunning = false;
	}
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			MoveBack();
		}
	}
	public void MoveTo(Transform target)
	{
		MoveTo(target.position, target.eulerAngles);
	}
	public void MoveTo(Vector3 pos, Vector3 rot)
	{
		if (_isCoroutineRunning)
		{
			StopCoroutine(_runningCoroutine);
		}
		_prevPos = targetCamera.transform.position;
		_prevRot = targetCamera.transform.eulerAngles;
		_runningCoroutine = StartCoroutine(MoveToTarget(pos, rot));
	}
	public void MoveBack()
	{
		MoveTo(_prevPos, _prevRot);
	}
	private IEnumerator MoveToTarget(Transform target)
	{
		yield return StartCoroutine(MoveToTarget(target.position, target.eulerAngles));
	}
	private IEnumerator MoveToTarget(Vector3 pos, Vector3 rot)
	{
		_isCoroutineRunning = true;
		Vector3 startPos = targetCamera.transform.position;
		Vector3 startRot = targetCamera.transform.eulerAngles;

		startRot.x = (startRot.x + 180) % 360 - 180;
		startRot.y = (startRot.y + 180) % 360 - 180;
		startRot.z = (startRot.z + 180) % 360 - 180;
		rot.x = (rot.x + 180) % 360 - 180;
		rot.y = (rot.y + 180) % 360 - 180;
		rot.z = (rot.z + 180) % 360 - 180;

		if (Mathf.Abs(startRot.x - rot.x) > 180f)
		{
			rot.x += (rot.x > 0 ? -360 : 360);
		}
		if (Mathf.Abs(startRot.y - rot.y) > 180f)
		{
			rot.y += (rot.y > 0 ? -360 : 360);
		}
		if (Mathf.Abs(startRot.z - rot.z) > 180f)
		{
			rot.z += (rot.z > 0 ? -360 : 360);
		}

		float timer = 0f;
		while(timer/time < 1)
		{
			float ratio = Mathf.SmoothStep(0, 1, timer / time);
			targetCamera.transform.position = Vector3.Lerp(startPos, pos, ratio);
			Debug.Log(Vector3.Lerp(startRot, rot, ratio));
			targetCamera.transform.eulerAngles = Vector3.Lerp(startRot, rot, ratio);
			timer += Time.deltaTime;
			yield return null;
		}
		targetCamera.transform.position = pos;
		targetCamera.transform.eulerAngles = rot;
		_isCoroutineRunning = false;
	}
}
