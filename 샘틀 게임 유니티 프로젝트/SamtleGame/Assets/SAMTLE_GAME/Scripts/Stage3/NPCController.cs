using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
	public float speed;

	protected bool _isworking;
	protected Rigidbody _rb;
	protected Animator _anim;

    // Start is called before the first frame update
    protected void Start()
    {
		_isworking = false;
		_rb = GetComponent<Rigidbody>();
		_anim = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {

    }
	public void Move(Vector3 pos)
	{
		StartCoroutine(MoveTo(pos));
		StartCoroutine(Rotate(pos));
	}
	IEnumerator MoveTo(Vector3 pos)
	{
		_isworking = true;
		Vector3 startPos = transform.position;
		while ((transform.position - pos).magnitude > 0.1f)
		{
			if(_rb.velocity.magnitude < speed)
				_rb.AddForce((pos - startPos).normalized * speed);
			_anim.SetFloat("Blend", 0.5f * _rb.velocity.magnitude / speed);
			yield return null;
		}
		_rb.velocity = Vector3.zero;
		//after walk
		float timer = 0;
		float breakTime = 0.4f;
		while(timer < 0.4f)
		{
			_anim.SetFloat("Blend", 0.5f * (breakTime - timer) / breakTime);
			timer += Time.deltaTime;
			yield return null;
		}
		_anim.SetFloat("Blend", 0f);
		_isworking = false;
	}
	IEnumerator Rotate(Vector3 pos)
	{
		float startAngle = transform.eulerAngles.y;
		float angle = Vector3.Angle(pos - transform.position, transform.TransformDirection(Vector3.forward)) + startAngle;

		float timer = 0;

		while (Mathf.Abs(transform.eulerAngles.y - angle) > 0.1f)
		{
			transform.eulerAngles = Vector3.up * Mathf.LerpAngle(startAngle, angle, timer / 0.5f);
			timer += Time.deltaTime;
			yield return null;
		}
		transform.eulerAngles = Vector3.up * angle;
	}

}
