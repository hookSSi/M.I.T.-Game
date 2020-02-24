using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractive : MonoBehaviour
{
	public Transform watchingObj = null;

    // Start is called before the first frame update
    void Start()
    {
		watchingObj = null;
	}

    // Update is called once per frame
    void Update()
    {
		Transform hit = HitCheckt();
		if(hit != watchingObj)
		{
			if(watchingObj != null)
			{
				watchingObj.GetComponent<Interact>().Leave();
			}

			watchingObj = hit;

			if (hit != null)
			{
				hit.GetComponent<Interact>().Watched();
			}
		}
	}
	private Transform HitCheckt()
	{
		RaycastHit hit;

		Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity);
		if (hit.transform != null && hit.transform.tag == "Interactable")
		{
			return hit.transform;
		}
		return null;
	}
	private void OnEnable()
	{
		watchingObj = null;
	}
	private void OnDisable()
	{
		if (watchingObj != null)
		{
			watchingObj.GetComponent<Interact>().Leave();
		}
	}
}
