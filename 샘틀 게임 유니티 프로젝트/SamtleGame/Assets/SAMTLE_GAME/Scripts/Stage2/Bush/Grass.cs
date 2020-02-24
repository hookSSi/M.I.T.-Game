using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.Pokemon
{
	public class Grass : MonoBehaviour
	{
		private Animator anim;
		private Bush bush;
		private void Start()
		{
			anim = GetComponent<Animator>();
			bush = transform.parent.GetComponent<Bush>();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			anim.SetTrigger("Wave");
			bush.Bushing();
		}
	}
}