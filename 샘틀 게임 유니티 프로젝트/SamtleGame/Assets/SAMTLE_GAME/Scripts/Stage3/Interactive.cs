using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage3
{	
	[RequireComponent(typeof(Outline))]
	public class Interactive : MonoBehaviour
	{
		private Outline _outline;
		public Transform _focusObj;

		private void Start() 
		{
			_outline = GetComponent<Outline>();
		}

		public virtual void Response()
		{

		}

		public virtual void Action()
		{
			
		}

		public void Watched()
		{
			_outline.enabled = true;
		}

		public void Leave()
		{
			_outline.enabled = false;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(_focusObj.position, this.transform.position);
		}
	}
}

