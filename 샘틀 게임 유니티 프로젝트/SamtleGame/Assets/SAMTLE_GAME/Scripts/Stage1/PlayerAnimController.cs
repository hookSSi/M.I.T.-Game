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

	public void StartToGoToStair(Transform stair)
	{
		this.stair = stair;
		anim.SetTrigger("GoToStair");
		// something other?

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
