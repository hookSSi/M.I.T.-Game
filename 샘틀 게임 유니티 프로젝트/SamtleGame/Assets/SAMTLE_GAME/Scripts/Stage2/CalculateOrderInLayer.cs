using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateOrderInLayer : MonoBehaviour
{
	public int offset = 0;
	public bool isWorkedOnce = true;

	private Renderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
		GetComponent<Renderer>().sortingOrder = (int)(transform.position.y * -100) + offset;
		if(!isWorkedOnce)
		{
			_renderer = GetComponent<Renderer>();
		}
		else
		{
			Destroy(this);
		}
    }
	private void Update()
	{
		_renderer.sortingOrder = (int)(transform.position.y * -100) + offset;
	}
}
