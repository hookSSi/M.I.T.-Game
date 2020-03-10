using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;

public enum FocusingType { FocusIn, FocustOut, None }

public struct FocusingEvent
{
	/// 물체를 바라보는 오브젝트
	public Vector3 _focusingObjPos;
	/// 물체의 위치
	public Transform _target;
	public FocusingType _type;
	public static FocusingEvent _event;

	public static void Trigger(FocusingType type = FocusingType.FocusIn, Vector3 focusingObjPos = default(Vector3), Transform target = null)
	{
		_event._focusingObjPos = focusingObjPos;
		_event._target = target;
		_event._type = type;
		EventManager.TriggerEvent(_event);
	}
}

[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
public class FocusingObjectCameraController : MonoBehaviour, EventListener<FocusingEvent>
{
	private Cinemachine.CinemachineVirtualCamera _vc;
	[SerializeField]
	private Transform _lookAt;
	[SerializeField]
	private Vector3 _pos;
	void Start()
    {
		_vc = GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    public void FocusIn(Vector3 pos, Transform target)
	{
		_vc.transform.position = pos;
		_vc.LookAt = target;
		_vc.Priority = 11;
	}

	public void FocusOut()
	{
		_vc.Priority = 9;
	}
	
	public virtual void OnEvent(FocusingEvent focusingEvent)
	{
		switch(focusingEvent._type)
		{
			case FocusingType.FocusIn:
				FocusIn(focusingEvent._focusingObjPos, focusingEvent._target);
				break;
			case FocusingType.FocustOut:
				FocusOut();
				break;
			case FocusingType.None:
				break;
		}
	}

	private void OnEnable() 
	{
		this.EventStartListening<FocusingEvent>();
	}

	private void OnDisable() 
	{
		this.EventStopListening<FocusingEvent>();
	}
}
