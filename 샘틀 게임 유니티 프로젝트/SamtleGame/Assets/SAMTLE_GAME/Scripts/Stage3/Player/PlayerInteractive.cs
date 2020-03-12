using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage3
{
	public class PlayerInteractive : MonoBehaviour
	{
		public Transform _watchingObj = null;
		public Interactive _interactive = null;
		public float _range = 100;

		// Start is called before the first frame update
		void Start()
		{
			_watchingObj = null;
		}

		// Update is called once per frame
		void Update()
		{
			Transform hit = HitCheckt();
			if( hit != null )
			{
				if( hit.transform.tag.Equals("Interactable" ) )
				{
					Interactive hitInteractive = hit.GetComponentInParent<Interactive>();
					if (hitInteractive == null)
					{
						hitInteractive = hit.GetComponentInChildren<Interactive>();
					}
					
					if( hitInteractive != _interactive )
					{
						if(_watchingObj != null)
						{
							_interactive.Leave();
						}

						_watchingObj = hit;
						_interactive = hitInteractive;
						_interactive.Watched();
					}
				}
			}
			
			if( hit == null || !hit.transform.tag.Equals("Interactable") )
			{
				if( _interactive != null )
				{
					_interactive.Leave();
				}
				
				_watchingObj = null;
				_interactive = null;
			}
		}

		private Transform HitCheckt()
		{
			RaycastHit hit;

			if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _range))
			{
				return hit.collider.transform;
			}
			else
				return null;
		}
		private void OnEnable()
		{
			_watchingObj = null;
		}
		private void OnDisable()
		{
			if (_watchingObj != null)
			{
				_interactive.Leave();
			}
		}

		private void OnDrawGizmos() {
			Gizmos.color = Color.red;
			Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * _range);
		}
	}
}

