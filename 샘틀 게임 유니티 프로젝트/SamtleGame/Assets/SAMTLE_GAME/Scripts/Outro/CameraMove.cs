using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	public enum MoveType { Linear, Smooth }
	[System.Serializable]
	public struct Work { public Vector3 pos; public float time; public MoveType moveType; };
	[Header("Initiation")]
	public bool isNeededInit;
	public Vector3 initPosition;
	public GameObject target;
	[Header("Work")]
	public List<Work> workQueue;

	// private Work now;
	private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
		if (target == null)
			target = gameObject;
		if (isNeededInit)
			target.transform.position = initPosition;
		StartCoroutine(Run());
    }

	IEnumerator Run()
	{
		foreach(Work w in workQueue)
		{
			startPos = target.transform.position;
			yield return StartCoroutine(ProcessWork(w));
		}
	}
	IEnumerator ProcessWork(Work work)
	{
		float timer = 0;

		while (timer < work.time)
		{
			float rate = 0f;
			switch (work.moveType)
			{
				case MoveType.Linear:
					rate = timer / work.time;
					break;
				case MoveType.Smooth:
					rate = Mathf.SmoothStep(0, 1, timer / work.time);
					break;
			}

			target.transform.position = startPos + (work.pos - startPos) * rate;

			timer += Time.deltaTime;
			if (timer > work.time)
				timer = work.time;
			yield return null;
		}
		target.transform.position = work.pos;
	}
}
