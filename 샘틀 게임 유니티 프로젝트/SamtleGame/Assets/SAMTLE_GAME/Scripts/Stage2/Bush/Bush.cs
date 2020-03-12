using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.Pokemon
{
	/*
	*	@desc 잔디 클래스
	*/
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
				Battle();
				count = 0;
			}
		}

		public void Battle()
		{
			PlayerControllerEvent.Trigger(false);
			PokemonBattleManager.Instance.StartBattle("신입생", "고양이");
		}
	}
}