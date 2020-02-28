using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage3
{
	public class PlayerInteractive : MonoBehaviour
	{
		public Transform _watchingObj = null;
		public Interactive _interactive = null;

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
				if( hit.transform.tag == "Interactable" && hit != _watchingObj )
				{
					if(_watchingObj != null)
						_watchingObj.GetComponentInParent<Interactive>().Leave();
					_watchingObj = hit;

					_interactive = hit.GetComponentInParent<Interactive>();
					_interactive.Watched();
				}
			}
			else if( hit == null )
			{
				if( _watchingObj != null )
				{
					Interactive  interact = _watchingObj.GetComponentInParent<Interactive>();
					if(interact != null)
					{
						interact.Leave();
					}
					else
					{
						Debug.Log("Interactive 스크립트가 없습니다.");
					}
				}
				
				_watchingObj = null;
				_interactive = null;
			}
		}
		private Transform HitCheckt()
		{
			RaycastHit hit;
			Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity);
			return hit.transform;
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
	}
}

