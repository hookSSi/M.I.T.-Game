using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
	public Transform stair;
	public Transform cameraPoint;

	public Cinemachine.CinemachineVirtualCamera cinemachine;

	private Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void StartToGoToStair(Transform stair = null)
	{
		if(stair != null)
			this.stair = stair;

		anim.SetTrigger("GoToStair");
		//스프라이트 반전 방지
		this.gameObject.GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(0, 180, 0);
	}

	public void GoToStair(float distance)
	{
		float before = stair.position.x - transform.position.x;
		float beforeCamera = cameraPoint.position.x - transform.position.x;
		ForcedMove(new Vector3(distance, 0, 0));
		float after = stair.position.x - transform.position.x;
		float afterCamera = cameraPoint.position.x - transform.position.x;
		if (before * after <= 0)
		{
			transform.position = new Vector3(stair.position.x, transform.position.y,transform.position.z);
			//중력과 콜라이더로 인한 이동을 막기위해 y축이동을 막았습니다.
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			anim.SetTrigger("GoDown");
		}
		if (beforeCamera * afterCamera <= 0)
		{
			cinemachine.Follow = null;
			cinemachine.transform.position = new Vector3(cameraPoint.position.x, cinemachine.transform.position.y, cinemachine.transform.position.z);
		}

	}

	public void ForcedMove(string xyz)
	{
		string[] value = xyz.Split(new char[] { ',', ' ', ':', '/' });
		ForcedMove(new Vector3(float.Parse(value[0]), float.Parse(value[1]), float.Parse(value[2])));
	}
	public void ForcedMove(Vector3 distance)
	{
		transform.position += distance;
	}
}
