using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupPresident : NPCController
{
	public GameObject obj;
	void Update()
	{
		if (go)
		{
			go = false;
			StartCoroutine(Sequence());
		}
	}
    IEnumerator Sequence()
	{
		Move(new Vector3(2.91f, 0f, 5.84f));

		yield return new WaitUntil(() => !_isworking);
		_anim.SetTrigger("LookAround");
		yield return new WaitForSeconds(4f);
		_anim.SetTrigger("PickingUP");
		yield return new WaitForSeconds(3f);
		PickObject();
		yield return new WaitForSeconds(2.4f);
		Move(new Vector3(1.36f, 0f, 1.83f));

		yield return new WaitUntil(() => !_isworking);
		_anim.SetTrigger("Give");
	}
	void PickObject()
	{
		obj.SetActive(true);
	}
	void GiveObject()
	{
		obj.SetActive(false);
	}
}
