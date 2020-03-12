using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage3
{
	public class Paper : Interactive
	{
		public GroupPresident gp;

		public override void Action()
		{
			gp.GiveObject();
		}
	}
}
