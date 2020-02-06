using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame
{
	public class Bush : MonoBehaviour
	{
		public bool isWorking = true;
		public float encounterChance = 0.05f;
		private int count = 0;

		public void Bushing()
		{
			if (!isWorking)
				return;

			float tmp = Random.Range(0f, 1f);
			count++;
			// Debug.Log(tmp);
			if (tmp < encounterChance + (count/10f) * encounterChance )
			{
				Debug.Log("발동!!");
				count = 0;
			}
		}
	}
}