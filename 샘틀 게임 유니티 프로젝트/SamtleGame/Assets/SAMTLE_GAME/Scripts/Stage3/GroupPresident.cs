using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIT.SamtleGame.Stage3;

public class GroupPresident : NPCController
{
	public GameObject obj;
	void Start()
	{
		base.Start();
		StartCoroutine(Sequence());
	}
	void Update()
	{
		
	}
    IEnumerator Sequence()
	{
		//Move(new Vector3(-36.84f,0,-1.71f));
		//yield return new WaitUntil(() => !_isworking);

		_anim.SetTrigger("LookAround");
		yield return new WaitUntil(() => GameManager.Instance.AllDone());
		_anim.SetTrigger("PickingUP");
		yield return new WaitForSeconds(3f);
		PickObject();
		yield return new WaitForSeconds(2.4f);
		Move(GameManager.Instance._player.transform.position);

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
