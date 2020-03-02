using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
	public float speed;

	[SerializeField]
	private bool go;

	private bool _isworking;
	private Rigidbody _rb;
	private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
		_isworking = false;
		_rb = GetComponent<Rigidbody>();
		_anim = GetComponent<Animator>();

	}

    // Update is called once per frame
    void Update()
    {
        if(go)
		{
			go = false;
			Move(new Vector3(2.91f, 0f, 5.84f));
		}
    }
	void Move(Vector3 pos)
	{
		StartCoroutine(MoveTo(pos));
		StartCoroutine(Rotate(pos));
	}
	IEnumerator MoveTo(Vector3 pos)
	{
		Vector3 startPos = transform.position;
		_rb.velocity = (pos - startPos).normalized * speed;
		_anim.SetFloat("Blend", 0.5f);
		while ((transform.position - pos).magnitude > 0.1f)
		{
			yield return null;
		}
		_rb.velocity = Vector3.zero;
		_anim.SetFloat("Blend", 0f);
		_anim.SetTrigger("LookAround");
	}
	IEnumerator Rotate(Vector3 pos)
	{
		transform.LookAt(pos);
		yield return null;
	}

}
