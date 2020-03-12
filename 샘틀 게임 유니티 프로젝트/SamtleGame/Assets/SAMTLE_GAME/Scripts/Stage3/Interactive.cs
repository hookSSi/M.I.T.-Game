using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage3
{	
	[SelectionBase]
	public class Interactive : MonoBehaviour
	{
		public Outline[] _outlines;
		public Transform _focusObj;
		public bool _isActive = true;

		protected virtual void Start() 
		{
			_outlines = GetComponentsInChildren<Outline>();
			Leave();
		}

		public virtual void Response()
		{

		}

		public virtual void Action()
		{
			
		}

		public virtual void ShowDescription()
		{
			
		}

		public void Watched()
		{
			//Debug.LogFormat("{0} 오브젝트 Outline - True", this.gameObject.name);
			if(_isActive)
			{
				foreach(var outline in _outlines)
				{
					outline.enabled = true;
				}
			}
		}

		public void Leave()
		{
			//Debug.LogFormat("{0} 오브젝트 Outline - False", this.gameObject.name);
			foreach(var outline in _outlines)
			{
				outline.enabled = false;
			}
		}

		protected virtual void OnDrawGizmosSelected()
		{
			if(_focusObj != null)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine(_focusObj.position, this.transform.position);
			}
		}
	}
}

