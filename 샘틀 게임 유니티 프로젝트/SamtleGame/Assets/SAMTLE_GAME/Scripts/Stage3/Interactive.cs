using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Interactive : MonoBehaviour
{
    public void Action()
	{
		
	}
	public void Watched()
	{
		GetComponent<Outline>().enabled = true;
	}
	public void Leave()
	{
		GetComponent<Outline>().enabled = false;
	}
}
