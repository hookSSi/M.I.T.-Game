using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
	public Transform stair;

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
		//중력과 콜라이더로 인한 이동을 막기위해 y축이동을 막았습니다.
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
	}

	public void GoToStair(float distance)
	{
		float before = stair.position.x - transform.position.x;
		ForcedMove(new Vector3(distance, 0, 0));
		float after = stair.position.x - transform.position.x;
		if (before * after <= 0)
		{
			transform.position = new Vector3(stair.position.x, transform.position.y,transform.position.z);
			anim.SetTrigger("GoDown");
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
