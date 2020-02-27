using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenUpEyes : MonoBehaviour
{
	[Header("필요 컴포넌트 세팅")]
	public Image blurImage;
	public Image up;
	public Image down;
	[Header("값 세팅")]
	public Vector3 movementAmount;
	public float blurStrenght = 15f;
	[Header("애니메이션 세팅")]
	public AnimationCurve blurCurve;
	public AnimationCurve eyeCurve;
	public float blurTime = 7f;
	public float eyeTime = 4f;
	[Header("테스트용 트리거")]
	public bool wakeUp;

	private bool _alreadyDone = false;
	// Start is called before the first frame update
	void Start()
	{
		blurImage.material.SetFloat("_Radius", blurStrenght);
		//StartCoroutine(WakeUp());
	}

	// Update is called once per frame
	void Update()
	{
		if(wakeUp && !_alreadyDone)
		{
			_alreadyDone = true;
			StartWakeUp();
		}
	}
	void StartWakeUp()
	{
		StartCoroutine(WakeUp());
	}

	IEnumerator WakeUp()
	{
		float timer = 0;

		Vector3 start = up.rectTransform.localPosition;
		Vector3 dest = up.rectTransform.localPosition + movementAmount;

		while (timer < Mathf.Max(blurTime, eyeTime))
		{
			blurImage.material.SetFloat("_Radius", Mathf.Lerp(blurStrenght, 0, blurCurve.Evaluate(timer / blurTime)));

			up.rectTransform.localPosition = Vector3.Lerp(start, dest, eyeCurve.Evaluate(timer / eyeTime));
			down.rectTransform.localPosition = Vector3.Lerp(-start, -dest, eyeCurve.Evaluate(timer / eyeTime));

			timer += Time.deltaTime;
			yield return null;
		}

		blurImage.material.SetFloat("_Radius", 0);
		up.rectTransform.localPosition = dest;
		down.rectTransform.localPosition = -dest;

		Destroy(this.gameObject);
	}
}
